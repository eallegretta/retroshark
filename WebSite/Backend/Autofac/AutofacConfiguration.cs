﻿using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.SignalR;

namespace RetroShark.Application.Backend.Autofac
{
    public static class AutofacConfiguration
    {
        public static IContainer Container { get; private set; }

        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
            GlobalHost.DependencyResolver = new global::Autofac.Integration.SignalR.AutofacDependencyResolver(Container);
        }
    }
}