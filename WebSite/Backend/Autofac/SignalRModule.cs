using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using RetroShark.Application.Backend.NHibernate;
using Module = Autofac.Module;

namespace RetroShark.Application.Backend.Autofac
{
    public class SignalRModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
        }
    }
}