using System;
using CIS.HR.Models.DTO;

namespace CIS.HR.Models
{
    namespace DTO
    {
        public class EmployeeDepartmentDTO : DTO
        {
            public int EmployeeId { get; set; }
            public int AssignmentId { get; set; }
            public int DepartmentId { get; set; }
            public DateTime DateEffective { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentAbbreviation { get; set; }
        }
    }

    public partial class Mapper
    {
        public virtual void MapToDTO(DepartmentAssignment model, EmployeeDepartmentDTO dto)
        {
            dto.AssignmentId = model.Id;
            dto.EmployeeId = model.EmployeeId;
            dto.DepartmentId = model.DepartmentId;
            dto.DateEffective = model.DateEffective;
            dto.DepartmentAbbreviation = model.Department.Abbreviation;
            dto.DepartmentName = model.Department.DepartmentName;
        }

        public virtual void MapToModel(EmployeeDepartmentDTO dto, DepartmentAssignment model)
        {
            model.Id = dto.AssignmentId;
            model.DateEffective = dto.DateEffective;
            model.EmployeeId = dto.EmployeeId;
            model.DepartmentId = dto.DepartmentId;
        }
    }
}