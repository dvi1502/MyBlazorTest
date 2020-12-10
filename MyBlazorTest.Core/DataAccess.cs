using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MyBlazorTest.Core.Models;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MyBlazorTest.Core
{
    public class DataAccess: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string memberName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        #endregion

        #region Public and private fields and properties

        private ISessionFactory _sessionFactory;

        public ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory is null)
                {
                    _sessionFactory = Fluently.Configure()
                        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(x => x
                            .Server("PC208\\SQL2019DEV")
                            .Database("MyTestHibernateDB")
                            .TrustedConnection()))
                        //.Cache(c => c.UseQueryCache().ProviderClass<HashtableCacheProvider>())
                        .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
                        //.ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false))
                        //.BuildConfiguration()
                        .BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public int SessionsCountDefault => 100;

        private int _sessionsCount;
        public int SessionsCount
        {
            get => _sessionsCount;
            set
            {
                if (value > 0)
                {
                    if (value > _sessionsCount)
                    {
                        var sessionsNew = new ISession[value];
                        if (!(_sessions is null))
                        {
                            for (var i = 0; i < _sessions.Length; i++)
                            {
                                sessionsNew[i] = _sessions[i];
                            }
                        }
                        _sessions = sessionsNew;
                    }
                    else if (value < _sessionsCount)
                    {
                        var sessionsNew = new ISession[value];
                        for (var i = 0; i < sessionsNew.Length; i++)
                        {
                            if (!(_sessions is null))
                            {
                                sessionsNew[i] = _sessions[i];
                            }
                        }
                        if (!(_sessions is null))
                        {
                            for (var i = sessionsNew.Length; i < _sessions.Length; i++)
                            {
                                if (!(_sessions[i] is null))
                                {
                                    //Console.WriteLine("SessionCount call");
                                    SessionClose(_sessions[i], null);
                                }
                            }
                        }
                        _sessions = sessionsNew;
                    }
                }
                _sessionsCount = value;
                OnPropertyRaised();
            }
        }

        private bool _sessionsIsConnected;
        public bool SessionsIsConnected
        {
            get
            {
                if (!(_sessions is null) && _sessions.Length > 0)
                {
                    foreach (var session in _sessions)
                    {
                        if (!(session is null) && session.IsConnected)
                            return true;
                    }
                }
                return false;
            }
            set
            {
                _sessionsIsConnected = value;
                OnPropertyRaised();
            }
        }

        private bool _sessionsIsOpen;
        public bool SessionsIsOpen
        {
            get
            {
                if (!(_sessions is null) && _sessions.Length > 0)
                {
                    foreach (var session in _sessions)
                    {
                        if (!(session is null) && session.IsOpen)
                            return true;
                    }
                }
                return false;
            }
            set
            {
                _sessionsIsOpen = value; 
                OnPropertyRaised();
            }

        }

        private ISession[] _sessions;
        public ISession SessionOpen
        {
            get
            {
                if (_sessions is null || _sessions.Length == 0)
                    return null;
                for (var i = 0; i < _sessions.Length; i++)
                {
                    if (_sessions[i] is null)
                    {
                        _sessions[i] = SessionFactory.OpenSession();
                        //Console.WriteLine($"SessionOpen: _sessions[{i}]: IsOpen: {_sessions[i].IsOpen} IsConnected: {_sessions[i].IsConnected} ");
                        return _sessions[i];
                    }
                }
                return null;
            }
        }

        public void SessionClose(ISession session, ITransaction transaction)
        {
            transaction?.Dispose();
            session?.Close();
            session?.Disconnect();
        }

        public DataConfiguration Configuration { get; set; }

        #endregion

        #region Design pattern "Lazy Singleton"

        private static DataAccess _instance;
        public static DataAccess Instance => LazyInitializer.EnsureInitialized(ref _instance);

        #endregion

        #region Constructor and destructor

        public DataAccess()
        {
            SessionsCount = SessionsCountDefault;
            SetupDefault();
        }

        public void SetupDefault()
        {
            if (SessionsCount < 1)
                SessionsCount = SessionsCountDefault;
            _sessions = new ISession[SessionsCount];
            _ = SessionsIsConnected;
            _ = SessionsIsOpen;
            Configuration = new Core.DataConfiguration();
        }

        public void Setup()
        {
            // Setup methods
        }

        #endregion

        #region Public and private methods - CRUD

        public async Task CreateAsync(BaseEntity entity, [CallerMemberName] string memberName = "")
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            if (SessionFactory is null)
                return;
            var session = SessionOpen;
            ITransaction transaction = null;

            try
            {
                transaction = session.BeginTransaction();
                await session.SaveOrUpdateAsync(entity).ConfigureAwait(false);
                await session.FlushAsync().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (!(transaction is null))
                    await transaction.RollbackAsync().ConfigureAwait(false);
                Console.WriteLine($"{memberName}. {ex.Message}");
            }
            finally
            {
                SessionClose(session, transaction);
            }
        }

        public async Task<BaseEntity[]> ReadAsync([CallerMemberName] string memberName = "")
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            if (SessionFactory is null)
                return new BaseEntity[0];
            using var session = SessionOpen;
            
            try
            {
                if (!(Configuration is null) && Configuration.Use)
                {
                    List<BaseEntity> items;
                    if (Configuration.OrderAsc)
                    {
                        items = await session.Query<BaseEntity>()
                            //.Where(ent => !string.IsNullOrEmpty(ent.Name))
                            .OrderBy(p => p.Id)
                            .Skip(Configuration.PageNo * Configuration.PageSize)
                            .Take(Configuration.PageSize)
                            //.Fetch(ent => ent.Id)
                            .ToListAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        items = await session.Query<BaseEntity>()
                            //.Where(ent => !string.IsNullOrEmpty(ent.Name))
                            .OrderByDescending(p => p.Id)
                            .Skip(Configuration.PageNo * Configuration.PageSize)
                            .Take(Configuration.PageSize)
                            //.Fetch(ent => ent.Id)
                            .ToListAsync().ConfigureAwait(false);
                    }
                    var ids = items.Select(ent => ent.Id).ToList();
                    var entities = await session.Query<BaseEntity>()
                        .Where(ent => ids.Contains(ent.Id))
                        //.OrderByDescending(ent => ent.Id)
                        //.FetchMany(ent => ent.Id)
                        .ToListAsync().ConfigureAwait(false);
                    return entities.ToArray();
                }
                var entitiesAll = await session.Query<BaseEntity>()
                    .ToListAsync().ConfigureAwait(false);
                return entitiesAll.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{memberName}. {ex.Message}");
            }
            return new BaseEntity[0];
        }

        public async Task UpdateAsync(BaseEntity entity)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

        }

        public async Task DeleteAsync(BaseEntity entity)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

        }

        #endregion
    }
}
