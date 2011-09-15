using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using echomonitor.Models;

namespace echomonitor.Controllers
{
    public class AppController : Controller
    {
        //
        // GET: /App/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Summary(string name)
        {
            return View(new AppViewModel() { Name = name});
        }

    }
}
