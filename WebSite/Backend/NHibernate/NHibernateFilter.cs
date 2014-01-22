using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Context;

namespace RetroShark.Application.Backend.NHibernate
{
    public class NHibernateFilter: ActionFilterAttribute
    {
        private NHibernateUnitOfWork _unitOfWork;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _unitOfWork.Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _unitOfWork = new NHibernateUnitOfWork();
        }
    }
}