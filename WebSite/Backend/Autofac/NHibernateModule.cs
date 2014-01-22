using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;
using RetroShark.Application.Backend.NHibernate;

namespace RetroShark.Application.Backend.Autofac
{
    public class NHibernateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(NHibernateConfiguration.Initialize()).As<ISessionFactory>().SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().GetCurrentSession()).As<ISession>().InstancePerHttpRequest();
        }
    }
}