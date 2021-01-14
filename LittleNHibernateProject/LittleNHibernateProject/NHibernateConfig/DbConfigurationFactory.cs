using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using LittleNHibernateProject.Model;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using System.Diagnostics;
using System.IO;
using NHibernate.Cfg;

namespace LittleNHibernateProject.NHibernateConfig
{
    public class DbConfigurationFactory
    {
        public ISessionFactory CreateFactory(string connectionString)
        {
            var nhibcfg = new NHibernate.Cfg.Configuration();

            var cfg = Fluently.Configure(nhibcfg)
                .Database(
                    PostgreSQLConfiguration
                        .Standard
                        .ConnectionString(connectionString)
                    )
                .Mappings(m =>
                {

                    m.AutoMappings.Add(AutoMap.AssemblyOf<Banan>(new AutomappingConfiguration())
                        .Conventions.Setup
                        (
                            con =>
                            {
                                con.Add<CommonConventions>();
                                con.Add(DefaultLazy.Never());
                                con.Add(DefaultCascade.SaveUpdate());
                                con.Add(DynamicInsert.AlwaysTrue());
                                con.Add(DynamicUpdate.AlwaysTrue());

                            }).UseOverridesFromAssemblyOf<OvverideMappingsFatherBanan>());

                    m.FluentMappings.Conventions.Add<CommonConventions>();
                    m.FluentMappings.Conventions.Add(DefaultLazy.Never(), DefaultCascade.SaveUpdate(), DynamicInsert.AlwaysTrue(), DynamicUpdate.AlwaysTrue());
                    m.MergeMappings();
                });

            if (true)
            {
                var directory = $"BananFatherDatabase.HBM";
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                cfg.Mappings(m => m.AutoMappings.ExportTo(directory));
            }

            cfg.ExposeConfiguration(c => SchemaMetadataUpdater.QuoteTableAndColumns(c, new PostgreSQLDialect()));
            cfg.ExposeConfiguration(c => c.SetProperty("command_timeout", "30"));

            cfg.ExposeConfiguration(x =>
            {
                x.SetInterceptor(new SqlStatementInterceptor());
            });

            UpdateSchema(cfg.BuildConfiguration());

            return cfg.BuildSessionFactory();
        }


        public bool ValidateSchema(Configuration configuration)
        {
            var validator = new SchemaValidator(configuration);
            try
            {
                validator.Validate();
                return true;
            }
            catch (HibernateException ex)
            {
                if (ex is SchemaValidationException)
                {
                    foreach (var error in (ex as SchemaValidationException).ValidationErrors)
                    {
                        Console.WriteLine(error);
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void UpdateSchema(NHibernate.Cfg.Configuration configuration)
        {
            if (!ValidateSchema(configuration))
            {
                try
                {
                    Console.WriteLine($"Need update schema {configuration}");

                    var update = new SchemaUpdate(configuration);
                    update.Execute(s => Console.WriteLine(s), true);
                }
                catch (HibernateException innerex)
                {
                    Console.WriteLine(innerex);
                }
            }
        }
    }

    public class SqlStatementInterceptor : EmptyInterceptor
    {
        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            Trace.WriteLine(sql.ToString());
            return sql;
        }
    }
}
