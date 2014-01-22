
using System.Linq;
using Autofac;
using Autofac.Integration.Mvc;
using Cinchcast.Framework.Commands;
using RetroShark.Application.Backend.Commands;

namespace RetroShark.Application.Backend.Autofac
{
    public class BusinessLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            RegisterTypeWithSameInterfaceName(assembly, builder);
            RegisterGenericTypesWithSameInterfaceName(assembly, builder);

            builder.RegisterType<DefaultCommandProcessor>()
                .As<ICommandProcessor>()
                .InstancePerHttpRequest();

            var commandTypes = (from t in assembly.GetTypes()
                                where t.IsSubclassOf(typeof(Command))
                                select t).ToArray();

            builder.RegisterTypes(commandTypes).InstancePerHttpRequest();
        }

        private static void RegisterGenericTypesWithSameInterfaceName(System.Reflection.Assembly assembly, ContainerBuilder builder)
        {
            var query = from t in assembly.GetTypes()
                        let @interface = t.GetInterface("I" + t.Name)
                        where @interface != null && t.IsGenericType && !t.IsAbstract
                        select new
                        {
                            Interface = @interface,
                            Implementation = t
                        };

            foreach (var type in query)
            {
                builder
                    .RegisterGeneric(type.Implementation)
                    .As(type.Interface)
                    .InstancePerHttpRequest();
            }
        }

        private static void RegisterTypeWithSameInterfaceName(System.Reflection.Assembly assembly, ContainerBuilder builder)
        {
            var query = from t in assembly.GetTypes()
                        let @interface = t.GetInterface("I" + t.Name)
                        where @interface != null && !t.IsGenericType && !t.IsAbstract
                        select new
                        {
                            Interface = @interface,
                            Implementation = t
                        };

            foreach (var type in query)
            {
                builder
                    .RegisterType(type.Implementation)
                    .As(type.Interface)
                    .InstancePerHttpRequest();
            }
        }
    }
}