using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using CIS.HR.Models;

namespace CIS.HR.DAL
{
    public interface IEmployeeService
    {
        int CreateEmployee(string firstName, string lastName, string adp, string username);
        List<PositionAssignment> GetPositionAssignments(int employeeId, DateTime? asOfDate = null);
        SupervisorAssignment GetSupervisorAssignment(int employeeId, DateTime? asOfDate = null);
        CoordinatorAssignment GetCoordinatorAssignment(int employeeId, DateTime? asOfDate = null);
        DepartmentAssignment GetDepartmentAssignment(int employeeId, DateTime? asOfDate = null);
        void AssignPosition(int employeeId, int positionId, DateTime dateEffective, bool isPrimary = true);
        void AssignSupervisor(int employeeId, int supervisorId, DateTime dateEffective);    
        void AssignCoordinator(int employeeId, int coordinatorId, DateTime dateEffective);   
        void AssignDepartment(int employeeId, int departmentId, DateTime dateEffective);
    }


    public class EmployeeService : Service, IEmployeeService
    {
        #region cctors
        public EmployeeService(Context context) : base(context)
        {
            _context.Entry(DefaultEmployee).State = EntityState.Detached;
        }

        static EmployeeService()
        {
            //empty employee
            DefaultEmployee = new Employee()
            {
                Id = 0,
                FirstName = "N/A"
            };
            
        }
        #endregion

        #region Methods
        public IQueryable<PositionAssignment> GetPositionAssignments( DateTime? asOfDate = null )
        {
            DateTime testDate = ((asOfDate == null) ? DateTime.Now : asOfDate.Value);
            var query = _context.PositionAssignments
                .Where(a => a.DateEffective <= testDate && (a.DateExited == null || a.DateExited >= testDate))
                .OrderByDescending(b => b.DateAsPrimary)
                .OrderByDescending(c => c.DateEffective); 
            return query;
        }



        //return the id of a newly created employee
        public int CreateEmployee(string firstName, string lastName, string adp, string username)
        {
            //todo: dupe checks on username and adp
            Employee employee = new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                Adp = adp,
                Username = username
            };
            _context.Employees.Add(employee);
            return employee.Id;
        }

        //return a list of active position assignments, where the first element is the employee's active primary position
        //      if no assignment exists in the specified time period, the default assignment will be returned
        public List<PositionAssignment> GetPositionAssignments(int employeeId, DateTime? asOfDate = null)
        {
            Employee employee = _context.Employees.Find(employeeId);
            DateTime testDate = ((asOfDate == null) ? DateTime.Now : asOfDate.Value);
            var outList = employee.PositionAssignmentHistory
                .Where(a => a.DateEffective <= testDate && (a.DateExited == null || a.DateExited >= testDate))
                .OrderByDescending(b => b.DateAsPrimary)
                .OrderByDescending(c => c.DateEffective).ToList();
            if (outList.Count == 0)
            {
                outList.Add(PositionService.DefaultPositionAssignment);
            }
            return outList;
        }

        //add a new position assignment
        public void AssignPosition(int employeeId, int positionId, DateTime dateEffective, bool isPrimary = true)
        {
            Position position = _context.Positions.Find(positionId);
            Employee employee = _context.Employees.Find(employeeId);
            employee.PositionAssignmentHistory.Add(new PositionAssignment()
            {
                DateEffective = dateEffective,
                DateAsPrimary = isPrimary ? DateTime.Now : DateTime.MinValue,
                Position = position,
            });
        }

        //return the supervisor assignment as of a specific date
        public SupervisorAssignment GetSupervisorAssignment(int employeeId, DateTime? asOfDate = null)
        {
            Employee employee = _context.Employees.Find(employeeId);
            DateTime testDate = ((asOfDate == null) ? DateTime.Now : asOfDate.Value);
            return employee.SupervisorAssignmentHistory
                .Where(a => a.DateEffective <= testDate)
                .OrderByDescending(b => b.DateEffective).FirstOrDefault()
            ?? new SupervisorAssignment()
            {
                Employee = employee,
                Supervisor = EmployeeService.DefaultEmployee,
                DateEffective = DateTime.MinValue
            };
        }

        //add(change to) a new supervisor
        public void AssignSupervisor(int employeeId, int supervisorId, DateTime dateEffective)
        {
            Employee employee = _context.Employees.Find(employeeId);
            Employee supervisor = _context.Employees.Find(supervisorId);
            employee.SupervisorAssignmentHistory.Add(new SupervisorAssignment()
            {
                Supervisor = supervisor,
                DateEffective = dateEffective
            });
        }

        //return the coordinator assignment as of a specific date
        public CoordinatorAssignment GetCoordinatorAssignment(int employeeId, DateTime? asOfDate = null)
        {
            Employee employee = _context.Employees.Find(employeeId);
            DateTime testDate = ((asOfDate == null) ? DateTime.Now : asOfDate.Value);
            return employee.CoordinatorAssignmentHistory
                .Where(a => a.DateEffective <= testDate)
                .OrderByDescending(b => b.DateEffective).FirstOrDefault()
            ?? new CoordinatorAssignment()
            {
                Employee = employee,
                Coordinator = EmployeeService.DefaultEmployee,
                DateEffective = DateTime.MinValue
            };
        }

        //add(change to) a new coordinator
        public void AssignCoordinator(int employeeId, int coordinatorId, DateTime dateEffective)
        {
            Employee employee = _context.Employees.Find(employeeId);
            Employee coordinator = _context.Employees.Find(coordinatorId);
            employee.CoordinatorAssignmentHistory.Add(new CoordinatorAssignment()
            {
                Coordinator = coordinator,
                DateEffective = dateEffective
            });
        }

        //return the department assignment as of a specific date
        public DepartmentAssignment GetDepartmentAssignment(int employeeId, DateTime? asOfDate = null)
        {
            Employee employee = _context.Employees.Find(employeeId);
            DateTime testDate = ((asOfDate == null) ? DateTime.Now : asOfDate.Value);
            return employee.DepartmentAssignmentHistory
                .Where(a => a.DateEffective <= testDate)
                .OrderByDescending(b => b.DateEffective).FirstOrDefault()
            ?? new DepartmentAssignment()
            {
                Department = DepartmentService.DefaultDepartment,
                Employee = employee,
                DateEffective = DateTime.MinValue
            };
        }
         
        //add(change to) a new department
        public void AssignDepartment(int employeeId, int departmentId, DateTime dateEffective)
        {
            Department department = _context.Departments.Find(departmentId);
            Employee employee = _context.Employees.Find(employeeId);
            employee.DepartmentAssignmentHistory.Add(new DepartmentAssignment()
            {
                Department = department,
                DateEffective = dateEffective
            });
        }

        ////return the employment status as of a specific date
        //public EmploymentStatusAssignment GetEmploymentStatusAssignment(int employeeId, DateTime? asOfDate = null)
        //{
        //    Employee employee = _context.Employees.Find(employeeId);
        //    DateTime testDate = ((asOfDate == null) ? DateTime.Now : asOfDate.Value);
        //    return employee.EmploymentStatusAssignmentHistory
        //        .Where(a => a.DateEffective <= testDate)
        //        .OrderByDescending(b => b.DateEffective).FirstOrDefault()
        //    ?? Context.GetDefaultEmploymentStatusAssignment(employee);
        //}

        ////add(change to) a new employment status
        //public void AssignEmploymentStatus(int employeeId, int statusId, DateTime dateEffective)
        //{
        //    EmploymentStatus status = _context.EmploymentStatuses.Find(statusId);
        //    Employee employee = _context.Employees.Find(employeeId);
        //    employee.EmploymentStatusAssignmentHistory.Add(new EmploymentStatusAssignment()
        //    {
        //        EmploymentStatus = status,
        //        DateEffective = dateEffective
        //    });
        //}

        ////return the shift type assignment as of a specific date
        //public ShiftTypeAssignment GetShiftTypeAssignment(int employeeId, DateTime? asOfDate = null)
        //{
        //    Employee employee = _context.Employees.Find(employeeId);
        //    DateTime testDate = ((asOfDate == null) ? DateTime.Now : asOfDate.Value);
        //    return employee.ShiftTypeAssignmentHistory
        //        .Where(a => a.DateEffective <= testDate)
        //        .OrderByDescending(b => b.DateEffective).FirstOrDefault()
        //    ?? Context.GetDefaultShiftTypeAssignment(employee);
        //}

        ////add(change to) a new shift type
        //public void AssignShiftType(int employeeId, int shiftTypeId, DateTime dateEffective)
        //{
        //    ShiftType shiftType = _context.ShiftTypes.Find(shiftTypeId);
        //    Employee employee = _context.Employees.Find(employeeId);
        //    employee.ShiftTypeAssignmentHistory.Add(new ShiftTypeAssignment()
        //    {
        //        ShiftType = shiftType,
        //        DateEffective = dateEffective
        //    });

        //}
        #endregion

        #region fields
        public static Employee DefaultEmployee { get; set; }
        #endregion
    }
}