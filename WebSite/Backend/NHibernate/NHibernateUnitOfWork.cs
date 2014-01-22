using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Context;

namespace RetroShark.Application.Backend.NHibernate
{
    public class NHibernateUnitOfWork: IDisposable
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateUnitOfWork(ISessionFactory sessionFactory = null)
        {
            if (sessionFactory == null)
            {
                sessionFactory = DependencyResolver.Current.GetService<ISessionFactory>();
            }

            _sessionFactory = sessionFactory;

            var session = _sessionFactory.OpenSession();

            session.BeginTransaction();

            CurrentSessionContext.Bind(session);
        }

        public void Dispose()
        {
            var session = CurrentSessionContext.Unbind(_sessionFactory);
           
            if (session == null)
            {
                return;
            }

            if (!session.Transaction.IsActive)
            {
                return;
            }

            bool exceptionThrown = false;

            if (HttpContext.Current != null && HttpContext.Current.Server != null && HttpContext.Current.Server.GetLastError() != null)
            {
                exceptionThrown = true;
            }
            else if (Marshal.GetExceptionCode() != 0)
            {
                exceptionThrown = true;
            }

            using (session)
            {
                if (exceptionThrown)
                {
                    session.Transaction.Rollback();
                }
                else
                {
                    session.Transaction.Commit();
                }
            }
        }
    }
}