using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cinchcast.Framework.Commands;
using NHibernate;

namespace RetroShark.Application.Controllers
{
    public abstract class BaseController: Controller
    {
        public ICommandProcessor CommandProcessor { set; protected get; }
    }
}