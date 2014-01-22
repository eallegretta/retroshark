using System;
using System.IO;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using RetroShark.Application.Backend.Entities;

namespace RetroShark.Application.Backend.NHibernate
{
    public static class NHibernateConfiguration
    {
        public static ISessionFactory Initialize()
        {
            var cfg = new Configuration();
            cfg.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nhibernate.config"));

            return Fluently
                .Configure(cfg)
                .Mappings(m => m.AutoMappings.Add(AutoMap.Assembly(Assembly.GetExecutingAssembly(), new EntitiesMappingConfiguration())))
                .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true))
                .BuildSessionFactory();
        }

        private class EntitiesMappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Type type)
            {
                if (string.IsNullOrWhiteSpace(type.Namespace))
                {
                    return false;
                }

                return type.IsClass && !type.IsAbstract && type.BaseType != null && type.BaseType.Name.Contains("Entity");
            }

            public override bool ShouldMap(FluentNHibernate.Member member)
            {
                return member.IsProperty && member.CanWrite;
            }
        }
    }
}