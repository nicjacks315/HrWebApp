using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CIS.HR.Models;

namespace CIS.HR.Controllers
{
    public class AdministrationController : Controller
    {
        private Context db = new Context();

        private void GetDatabaseInfo(Context db)
        {
            var lastSync = Util.GetLastSync(db);
            ViewBag.LastSync = lastSync?.ToShortDateString() + " at " + lastSync?.ToShortTimeString();

            var readOnly = Util.GetReadOnlyState(db);
            ViewBag.ReadOnly = readOnly ? "Read Only" : "Writable";
        }

        public ActionResult SyncDatabase()
        {
            var User = System.Web.HttpContext.Current.User;
            if (User.IsInRole(AppRoles.Admin))
            {
                db.Database.ExecuteSqlCommand("EXEC dbo.usp_syncdatabase");
                //return RedirectToAction("Index", "Administration");
            }
            //return view();
            return RedirectToAction("Index", "Administration");
        }

        public ActionResult ToggleReadOnly()
        {
            return RedirectToAction("Index", "Administration");
        }

        // GET: Administration
        //[Authorize(Roles = AppRoles.Admin)]
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.User;
            if (User.IsInRole(AppRoles.Admin))
            {
                GetDatabaseInfo(db);
                return View();
            }
            TempData["alertmessage"] = "<script>alert('You lack the permissions necessary to access this area.');</script>";
            return RedirectToAction("Index", "Home");
        }
    }
}