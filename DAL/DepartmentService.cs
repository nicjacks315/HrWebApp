using System.Data.Entity;
using CIS.HR.Models;

namespace CIS.HR.DAL
{
    public class DepartmentService : Service
    {
        #region cctors
        public DepartmentService(Context context) : base(context)
        {
            _context.Entry(DefaultDepartment).State = EntityState.Detached;
        }

        static DepartmentService()
        {
            DefaultDepartment = new Department()
            {
                Id = 0,
                DepartmentName = "N/A"
            };
        }
        #endregion

        #region methods
        public int CreateDepartment(string name, string abbreviation)
        {
            Department department = new Department()
            {
                DepartmentName = name,
                Abbreviation = abbreviation
            };
            _context.Departments.Add(department);
            return department.Id;
        }
        #endregion

        #region fields
        public static Department DefaultDepartment { get; set; }
        #endregion
    }
}