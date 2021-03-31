using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIS.HR;
using CIS.HR.Models;
using CIS.HR.DAL;
using PagedList;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using CIS.HR.ViewModels;

namespace CIS.HR.Controllers
{
    public class EmployeeController : Controller
    {
        private Context db = new Context();

        // GET: Employee
        public ActionResult Index(string searchField, string searchString, string sortField, string sortOrder, int? page)
        {
            //set read-only
            ViewBag.ReadOnly = true;

            //get viewmodels
            var today = DateTime.Today.ToShortDateString();
            IEnumerable<EmployeeIndexViewModel> filteredEmployees = db.Database.SqlQuery<EmployeeIndexViewModel>("SELECT * FROM dbo.ufn_EmployeeExtendedAsOf('"+today+"')");

            //filter view model criteria
            if( !String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (searchField)
                {
                    case "employee":
                        filteredEmployees = filteredEmployees.Where(e =>
                                                !String.IsNullOrEmpty(e.LastName) &&
                                                !String.IsNullOrEmpty(e.FirstName) &&
                                                !String.IsNullOrEmpty(e.Adp));
                        filteredEmployees = filteredEmployees.Where(e => 
                                                e.LastName.ToLower().Contains(searchString) || 
                                                e.FirstName.ToLower().Contains(searchString) || 
                                                e.Adp.ToLower().Contains(searchString));
                        break;
                    case "status":
                        filteredEmployees = filteredEmployees.Where(e =>
                                                !String.IsNullOrEmpty(e.EmploymentStatus));
                        filteredEmployees = filteredEmployees.Where(e =>
                                                e.EmploymentStatus.ToLower().Contains(searchString));
                        break;
                    case "position":
                        filteredEmployees = filteredEmployees.Where(e =>
                                                !String.IsNullOrEmpty(e.PositionCode) &&
                                                !String.IsNullOrEmpty(e.PositionTitle));
                        filteredEmployees = filteredEmployees.Where(e => 
                                                e.PositionCode.ToLower().Contains(searchString) || 
                                                e.PositionTitle.ToLower().Contains(searchString));
                        break;
                    case "shiftType":
                        filteredEmployees = filteredEmployees.Where(e =>
                                                !String.IsNullOrEmpty(e.ShiftType));
                        filteredEmployees = filteredEmployees.Where(e =>
                                                e.ShiftType.ToLower().Contains(searchString));
                        break;
                    case "department":
                        filteredEmployees = filteredEmployees.Where(e =>
                                                !String.IsNullOrEmpty(e.Department));
                        filteredEmployees = filteredEmployees.Where(e =>
                                                e.Department.ToLower().Contains(searchString));
                        break;
                    case "supervisor":
                        filteredEmployees = filteredEmployees.Where(e => 
                                                !String.IsNullOrEmpty(e.Supervisor));
                        filteredEmployees = filteredEmployees.Where(e =>
                                                e.Supervisor.ToLower().Contains(searchString));
                        break;
                    case "coordinator":
                        filteredEmployees = filteredEmployees.Where(e =>
                                                !String.IsNullOrEmpty(e.Coordinator));
                        filteredEmployees = filteredEmployees.Where(e =>
                                                e.Coordinator.ToLower().Contains(searchString));
                        break;
                }
                ViewBag.CurrentSearchField = searchField;
                ViewBag.CurrentSearchString = searchString;
            }

            //pass current sort order to viewdata
            ViewBag.CurrentSortField = sortField;
            ViewBag.CurrentSortOrder = sortOrder;

            //pass next sort order for each field to viewdata
            ViewBag.EmployeeSortParam = Util.GetSortParameter("employee", sortField, sortOrder);
            ViewBag.StatusSortParam = Util.GetSortParameter("status", sortField, sortOrder);
            ViewBag.PositionSortParam = Util.GetSortParameter("position", sortField, sortOrder);
            ViewBag.ShiftTypeSortParam = Util.GetSortParameter("shiftType", sortField, sortOrder);
            ViewBag.DepartmentSortParam = Util.GetSortParameter("department", sortField, sortOrder);
            ViewBag.SupervisorSortParam = Util.GetSortParameter("supervisor", sortField, sortOrder);
            ViewBag.CoordinatorSortParam = Util.GetSortParameter("coordinator", sortField, sortOrder);  

            //alternatively, set next sort order
            //string[] columns = { "Employee", "Status", "Position", "ShiftType", "Department", "Supervisor", "Coordinator" };
            //foreach( string c in columns )
            //{
            //    ViewData.Add(c + "SortParam", Util.GetSortParameter(c, sortField, sortOrder));
            //}

            //sort viewmodel
            var orderedEmployees = filteredEmployees.OrderBy(e => 1);
            switch (ViewBag.CurrentSortOrder)
            {
                case "descending":
                    switch (ViewBag.CurrentSortField)
                    {
                        case "employee":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.LastName))
                                .ThenByDescending(e => e.LastName)
                                .ThenByDescending(e => e.FirstName);
                            break;
                        case "status":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.EmploymentStatus))
                                .ThenByDescending(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "position":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.PositionCode))
                                .ThenByDescending(e => e.PositionCode)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "shiftType":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.ShiftType))
                                .OrderByDescending(e => e.ShiftType)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "department":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.Department))
                                .ThenByDescending(e => e.Department)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "supervisor":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.Supervisor))
                                .ThenByDescending(e => e.Supervisor)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "coordinator":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.Coordinator))
                                .ThenByDescending(e => e.Coordinator)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                    }
                    break;
                case "ascending":
                    switch (ViewBag.CurrentSortField)
                    {
                        case "employee":
                            //employees = employees.OrderBy(e => String.IsNullOrEmpty(e.LastName)).ThenBy(e => e.LastName).ThenBy(e => e.FirstName);
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.LastName))
                                .ThenBy(e => e.LastName)
                                .ThenBy(e => e.FirstName);
                            break;
                        case "status":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.EmploymentStatus))
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "position":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.PositionCode))
                                .ThenBy(e => e.PositionCode)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "shiftType":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.ShiftType))
                                .ThenBy(e => e.ShiftType)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "department":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.Department))
                                .ThenBy(e => e.Department)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "supervisor":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.Supervisor))
                                .ThenBy(e => e.Supervisor)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                        case "coordinator":
                            orderedEmployees = orderedEmployees
                                .OrderBy(e => String.IsNullOrEmpty(e.Coordinator))
                                .ThenBy(e => e.Coordinator)
                                .ThenBy(e => e.EmploymentStatus)
                                .ThenBy(e => e.LastName);
                            break;
                    }
                    break;
                default:
                    orderedEmployees = orderedEmployees
                        .OrderBy(e => e.EmploymentStatus)
                        .ThenBy(e => e.LastName);
                    break;
            }

            //configure pagination
            int pageSize = 30;
            int pageNumber = (page ?? 1);

            return View(orderedEmployees.ToPagedList(pageNumber,pageSize));
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            //set read-only
            ViewBag.ReadOnly = Context.IsReadOnly();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Details2/5
        public ActionResult Details2(int? id)
        {
            //set read-only
            ViewBag.ReadOnly = Context.IsReadOnly();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employeeEntity = db.Employees.Find(id);

            if (employeeEntity == null)
            {
                return HttpNotFound();
            }

            var dbService = new TemporalService(db);

            var employee = dbService.GetEmployee(id);
            var employeeContact = dbService.GetContactPreferences(id);

            var supervisor = dbService.GetSupervisor(id);
            var supervisorContact = dbService.GetContactPreferences(supervisor.ManagerId);

            var coordinator = dbService.GetCoordinator(id);
            var coordinatorContact = dbService.GetContactPreferences(coordinator.ManagerId);

            var departments = dbService.GetDepartments(id);
            string departmentsList = departments.Count() > 0 ? departments[0].DepartmentAbbreviation : String.Empty;
            for( int i = 1; i < departments.Count(); i++)
            {
                departmentsList += ", " + departments[i].DepartmentAbbreviation;
            }

            var positions = dbService.GetAllEmployeePositions(id);

            string startDate = dbService.GetStartDate(id)?.ToShortDateString();

            var shiftType = dbService.GetShiftType(id);

            var viewModel = new ExtendedEmployeeDetailsViewModel()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Username = employee.Username,
                Adp = employee.Adp,
                ContactPreferences = new ContactPreferencesViewModel(employeeContact),
                Supervisor = new ManagerViewModel(supervisor, supervisorContact),
                Coordinator = new ManagerViewModel(coordinator, coordinatorContact),
                Positions = new PositionHistoryViewModel(positions),
                StartDate = startDate,
                Departments = departmentsList,
                ShiftType = shiftType.ShiftType
            };
            return View(viewModel);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Adp,Username")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Adp,Username")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
