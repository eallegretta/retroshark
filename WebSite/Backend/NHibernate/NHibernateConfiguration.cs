using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace RetroShark.Application.Backend.NHibernate
{
    public static class NHibernateConfiguration
    {
        public static ISessionFactory Initialize()
        {
            var cfg = new Configuration();

            cfg.Configure();

            return Fluently
                .Configure(cfg)
                .Mappings(m => m.AutoMappings.Add(
                        AutoMap.Assembly(Assembly.GetExecutingAssembly(), new EntitiesMappingConfiguration())
                               .Conventions.AddFromAssemblyOf<StringLengthConvention>()
                                       ))
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

        private class StringLengthConvention : AttributePropertyConvention<StringLengthAttribute>
        {
            protected override void Apply(StringLengthAttribute attribute, FluentNHibernate.Conventions.Instances.IPropertyInstance instance)
            {
                if (attribute.MaximumLength == int.MaxValue)
                {
                    instance.CustomSqlType("TEXT");
                }
                else
                {
                    instance.Length(attribute.MaximumLength);
                }
            }
        }

    }
}