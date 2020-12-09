using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyBlazorTest.Core.Models;

namespace MyBlazorTest.Core
{
    public class DataAccess 
    {
        public ISessionFactory SessionFactory { get; set; }

        public DataAccess()
        {
            var connectionString = 
                "Data Source=PC208\\SQL2019DEV;Initial Catalog=MyTestHibernateDB;Integrated Security=SSPI;Connect Timeout=60;";
            ConfigureServices(connectionString);
        }

        public void ConfigureServices(string connectionString)
        {
            SessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                //.Cache(c => c.UseQueryCache().ProviderClass<HashtableCacheProvider>())
                .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false))
                //.BuildConfiguration()
                .BuildSessionFactory();
        }

        public Cat[] GetCats(int pageNo, int pageSize, ISession session)
        {
            var entities = session.Query<Cat>()
                //.Where(ent => !string.IsNullOrEmpty(ent.Name))
                .OrderByDescending(p => p.Id)
                .Skip(pageNo * pageSize)
                .Take(pageSize)
                //.Fetch(ent => ent.Id)
                .ToList();
            var ids = entities.Select(ent => ent.Id).ToList();
            var cats = session.Query<Cat>()
                .Where(ent => ids.Contains(ent.Id))
                //.OrderByDescending(ent => ent.Id)
                //.FetchMany(ent => ent.Id)
                .ToList();
            return cats.ToArray();
        }

        public async Task<Cat[]> GetCatsAsync(int pageNo, int pageSize, [CallerMemberName] string memberName = "")
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            if (SessionFactory is null)
                return new Cat[0];
            using var session = SessionFactory.OpenSession();
            return GetCats(pageNo, pageSize, session);
        }
    }
}
