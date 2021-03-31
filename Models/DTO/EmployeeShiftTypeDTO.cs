using System;
using CIS.HR.Models.DTO;

namespace CIS.HR.Models
{
    namespace DTO
    {
        public class EmployeeShiftTypeDTO
        {
            public int EmployeeId { get; set; }
            public int AssignmentId { get; set; }
            public int ShiftTypeId { get; set; }
            public DateTime DateEffective { get; set; }
            public string ShiftType { get; set; }
        }
    }

    public partial class Mapper
    {
        public virtual void MapToDTO(ShiftTypeAssignment model, EmployeeShiftTypeDTO dto)
        {
            dto.EmployeeId = model.EmployeeId;
            dto.AssignmentId = model.Id;
            dto.ShiftTypeId = model.ShiftTypeId;
            dto.ShiftType = model.ShiftType.ShiftTypeName;
            dto.DateEffective = model.DateEffective;
        }

        public virtual void MapToModel(EmployeeShiftTypeDTO dto, ShiftTypeAssignment model)
        {
            model.Id = dto.AssignmentId;
            model.EmployeeId = dto.EmployeeId;
            model.ShiftTypeId = dto.ShiftTypeId;
            model.DateEffective = dto.DateEffective;
        }
    }
}