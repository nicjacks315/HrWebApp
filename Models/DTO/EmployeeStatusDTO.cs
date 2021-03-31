using System;
using CIS.HR.Models.DTO;

namespace CIS.HR.Models
{
    namespace DTO
    {
        public class EmployeeStatusDTO : DTO
        {
            public int EmployeeId { get; set; }
            public int AssignmentId { get; set; }
            public int EmploymentStatusId { get; set; }
            public DateTime DateEffective { get; set; }
            public string StatusName { get; set; }
        }
    }

    public partial class Mapper
    {
        public virtual void MapToDTO(EmploymentStatusAssignment model, EmployeeStatusDTO dto)
        {
            dto.EmployeeId = model.EmployeeId;
            dto.AssignmentId = model.Id;
            dto.EmploymentStatusId = model.EmploymentStatus.Id;
            dto.DateEffective = model.DateEffective;
            dto.StatusName = model.EmploymentStatus.StatusName;
        }

        public virtual void MapToModel(EmployeeStatusDTO dto, EmploymentStatusAssignment model)
        {
            model.EmployeeId = dto.EmployeeId;
            model.Id = dto.AssignmentId;
            model.EmploymentStatus.Id = dto.EmploymentStatusId;
            model.DateEffective = dto.DateEffective;
        }
    }
}