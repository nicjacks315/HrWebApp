using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIS.HR.Models;
using CIS.HR.DAL;
using CIS.HR.ViewModels;
using PagedList;
using System.Data.SqlClient;

namespace HrWebApp.Controllers
{


    public class DepartmentController : Controller
    {
        private Context db = new Context();

        // GET: Employee
        public ActionResult Index(int? page)
        {
            //get viewmodels
            var today = DateTime.Today.ToShortDateString();
            IEnumerable<Department> departments = db.Database.SqlQuery<Department>("SELECT * FROM dbo.Department").OrderBy(d => d.DepartmentName);

            //configure pagination
            int pageSize = 30;
            int pageNumber = (page ?? 1);

            return View(departments.ToPagedList(pageNumber, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
