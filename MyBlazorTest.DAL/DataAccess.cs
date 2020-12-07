using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlazorTest.DAL
{
    public static class DataAccess 
    {

        public static ISessionFactory ConfigureServices(string connStr, Type _type)
        {

#if DEBUG
            ISessionFactory _sessionFactory = Fluently.Configure()
                                      .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connStr).ShowSql())
                                      //.Mappings(m => m.FluentMappings.AddFromAssemblyOf<CatMap>())
                                      .Mappings(m => m.FluentMappings.AddFromAssembly(_type.Assembly))
                                      .BuildSessionFactory();

#else
            ISesionFactory _sessionFactory = Fluently.Configure()
                                      .Database( MsSqlConfiguration.MsSql2012.ConnectionString(connStr) )
                                      //.Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
                                      .Mappings(m => m.FluentMappings.AddFromAssembly(_type.Assembly))
                                      .BuildSessionFactory();
#endif            

            return _sessionFactory;
        }

    }
}
