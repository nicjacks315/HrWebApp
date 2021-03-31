using System;
using System.DirectoryServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using HrWebApp.Controllers;

namespace CIS.HR.Controllers
{
    public class HomeController : Controller
    {
        private CIS.HR.Models.Context db = new CIS.HR.Models.Context();

        public ActionResult Index()
        {
            return View();
        }
    }
}