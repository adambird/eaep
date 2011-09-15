using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using eaep;
using eaep.http;
using eaepwiredwebapp.Models;

namespace eaepwiredwebapp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Configuration.EAEPEnabled)
            {
                EAEPMessage message = new EAEPMessage(Environment.MachineName, Configuration.ApplicationName, "ActionExecuted");
                message["Controller"] = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                message["Action"] = filterContext.ActionDescriptor.ActionName;

                if (filterContext.HttpContext.User.Identity.Name.Length > 0)
                {
                    message[EAEPMessage.PARAM_USER] = filterContext.HttpContext.User.Identity.Name;
                }

                IEAEPHttpClient client = new EAEPHttpClient(Configuration.EAEPHttpClientTimeout);
                client.SendMessage(Configuration.EAEPMonitorURI, message);
            }
        }

    }
}
