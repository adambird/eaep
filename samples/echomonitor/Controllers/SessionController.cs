using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using echomonitor.Models;

namespace echomonitor.Controllers
{
    public class SessionController : Controller
    {

        ISessionsService sessionsService;

        public SessionController()
            : this (new SessionsService())
        { 
        }

        public SessionController(ISessionsService service)
        {
            this.sessionsService = service;
        }


        public JsonResult DaySummary(string app)
        {
            return Json(sessionsService.GetDaySummary(app, DateTime.Now.Date));
        }

        public JsonResult ActiveUsers(string app)
        {
            return Json(sessionsService.ActiveUsers(app));
        }
    }
}
