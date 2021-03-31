using System;
using CIS.HR.Models.DTO;

namespace CIS.HR.Models
{
    namespace DTO
    {
        public class EmployeePositionDTO : PositionDTO
        {
            public int AssignmentId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime DateEffective { get; set; }
            public DateTime? DateAsPrimary { get; set; }
            public DateTime? DateStarted { get; set; }
            public DateTime? DateExited { get; set; }
        }
    }

    public partial class Mapper
    {
        public virtual void MapToDTO(PositionAssignment modelA, PositionDescription modelB, EmployeePositionDTO dto)
        {
            dto.PositionId = modelA.PositionId;
            dto.EmployeeId = modelA.EmployeeId;
            dto.AssignmentId = modelA.Id;
            dto.DateAsPrimary = modelA.DateAsPrimary;
            dto.DateEffective = modelA.DateEffective;
            dto.DateExited = modelA.DateExited;
            dto.DateStarted = modelA.DateStarted;
            dto.Code = modelA.Position.Code;
            dto.Title = modelB.Title;
            dto.Classification = modelB.Classification.ClassificationName;
            dto.ClassificationId = modelB.ClassificationId;
            dto.PositionDescriptionId = modelB.Id;
        }

        public virtual void MapToModel(EmployeePositionDTO dto, PositionAssignment model)
        {
            model.Id = dto.AssignmentId;
            model.DateAsPrimary = dto.DateAsPrimary;
            model.DateEffective = dto.DateEffective;
            model.DateExited = dto.DateExited;
            model.DateStarted = dto.DateStarted;
            model.EmployeeId = dto.EmployeeId;
            model.PositionId = dto.PositionId;
        }
    }
}