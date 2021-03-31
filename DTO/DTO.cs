using System;

namespace CIS.HR
{
    namespace Models.DTO
    {
        public class DTO { }

        public class EmployeeDTO : DTO
        {
            public int EmployeeId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Adp { get; set; }
            public string Username { get; set; }
            public bool IsSupervisor { get; set; }
            public bool IsCoordinator { get; set; }
            public bool IsDirector { get; set; }
            public bool IsLeader { get; set; }
        }

        public class ContactPreferencesDTO : DTO
        {
            public int ContactPreferencesId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime DateEffective { get; set; }
            public string Phone1 { get; set; }
            public string Phone2 { get; set; }
            public string Email1 { get; set; }
            public string Email2 { get; set; }
            public string Extension { get; set; }
        }

        public class PositionDTO : DTO
        {
            public int PositionId { get; set; }
            public int PositionDescriptionId { get; set; }
            public int ClassificationId { get; set; }
            public string Code { get; set; }
            public string Title { get; set; }
            public DateTime LastUpdated { get; set; }
            public string Classification { get; set; }
        }

        public class EmployeePositionDTO : PositionDTO
        {
            public int AssignmentId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime DateEffective { get; set; }
            public DateTime? DateAsPrimary { get; set; }
            public DateTime? DateStarted { get; set; }
            public DateTime? DateExited { get; set; }
        }

        public class EmployeeManagerDTO : EmployeeDTO
        {
            public int AssignmentId { get; set; }
            public int ManagerId { get; set; }
            public DateTime DateEffective { get; set; }
        }

        public class EmployeeStatusDTO : DTO
        {
            public int EmployeeId { get; set; }
            public int AssignmentId { get; set; }
            public int EmploymentStatusId { get; set; }
            public DateTime DateEffective { get; set; }
            public string StatusName { get; set; }
        }

        public class EmployeeDepartmentDTO : DTO
        {
            public int EmployeeId { get; set; }
            public int AssignmentId { get; set; }
            public int DepartmentId { get; set; }
            public DateTime DateEffective { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentAbbreviation { get; set; }
        }

        public class EmployeeShiftTypeDTO
        {
            public int EmployeeId { get; set; }
            public int AssignmentId { get; set; }
            public int ShiftTypeId { get; set; }
            public DateTime DateEffective { get; set; }
            public string ShiftType { get; set; }
        }
    }
}