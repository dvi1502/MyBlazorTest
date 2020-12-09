using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MyBlazorTest.Core.Models;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlazorTest.Core
{
    public class DataAccess 
    {
        #region Public and private fields and properties

        public ISessionFactory SessionFactory { get; set; }

        #endregion

        #region Constructor and destructor

        public DataAccess()
        {
            InitSessionFactory();
        }

        public void InitSessionFactory()
        {
            //var connectionString =
            //    "Data Source=PC208\\SQL2019DEV;Initial Catalog=MyTestHibernateDB;Integrated Security=SSPI;Connect Timeout=60;";
            SessionFactory = Fluently.Configure()
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

        #endregion

        #region Public and private methods - CRUD

        public async Task CreateAsync(BaseEntity entity)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

        }

        public async Task<BaseEntity[]> ReadAsync(ReadConfiguration configuration)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            if (SessionFactory is null)
                return new BaseEntity[0];

            using var session = SessionFactory.OpenSession();
            if (!(configuration is null) && configuration.Use)
            {
                List<BaseEntity> items;
                if (configuration.OrderAsc)
                {
                    items = await session.Query<BaseEntity>()
                        //.Where(ent => !string.IsNullOrEmpty(ent.Name))
                        .OrderBy(p => p.Id)
                        .Skip(configuration.PageNo * configuration.PageSize)
                        .Take(configuration.PageSize)
                        //.Fetch(ent => ent.Id)
                        .ToListAsync().ConfigureAwait(false);
                }
                else
                {
                    items = await session.Query<BaseEntity>()
                        //.Where(ent => !string.IsNullOrEmpty(ent.Name))
                        .OrderByDescending(p => p.Id)
                        .Skip(configuration.PageNo * configuration.PageSize)
                        .Take(configuration.PageSize)
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
