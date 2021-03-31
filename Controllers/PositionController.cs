using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIS.HR.Models;
using CIS.HR.ViewModels;
using CIS.HR.DAL;

namespace CIS.HR.Controllers
{
    public class PositionController : Controller
    {
        //private Context db = new Context();
        //private PositionService ps;

        // GET: Position
        public ActionResult Index()
        {
            Context ctx = new Context();
            var dbService = new TemporalService(ctx);
            //var dbService = new TemporalService();

            var positions = ctx.Positions.ToList();

            List<PositionViewModel> pvmList = new List<PositionViewModel>();
            foreach( var p in positions )
            {
                pvmList.Add(new PositionViewModel(dbService.GetPosition(p.Id)));
            }

            return View(pvmList);
        }

        // GET: Position/Create
        public ActionResult Create(FormCollection form)
        {
            //deserialize
            var cpvm = new CreatePositionViewModel();
            TryUpdateModel(cpvm, new string[] { "Code", "Title", "DateEffective" }, form.ToValueProvider());

            //validate <!----handled in FluentValidation
            //if (String.IsNullOrEmpty(cpvm.Code)) ModelState.AddModelError("Code", "A unique position code is required!");
            //if (String.IsNullOrEmpty(cpvm.Title)) ModelState.AddModelError("Title", "An initial position title must be specified!");
            //if (cpvm.DateEffective == null) ModelState.AddModelError("DateEffective", "An effective date is required!");

            //save
            if (ModelState.IsValid)
            {
                //var ctx = new Context();
                //var ps = new PositionService(ctx);
                //ps.CreatePosition(cpvm.Code, cpvm.Title, cpvm.DateEffective);
                //ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}