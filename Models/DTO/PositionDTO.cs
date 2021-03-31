using System;
using CIS.HR.Models.DTO;

namespace CIS.HR.Models
{
    namespace DTO
    {
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
    }

    public partial class Mapper
    {
        public virtual void MapToDTO(Position model, PositionDTO dto)
        {
            dto.PositionId = model.Id;
            dto.Code = model.Code;
        }

        public virtual void MapToModel(PositionDTO dto, Position model)
        {
            model.Id = dto.PositionId;
            model.Code = dto.Code;
        }

        public virtual void MapToDTO(PositionDescription model, PositionDTO dto)
        {
            dto.PositionDescriptionId = model.Id;
            dto.Title = model.Title;
            dto.LastUpdated = model.DateEffective;
            dto.ClassificationId = model.ClassificationId;
            dto.Classification = model.Classification.ClassificationName;
        }

        public virtual void MapToModel(PositionDTO dto, PositionDescription model)
        {
            model.Id = dto.PositionDescriptionId;
            model.DateEffective = dto.LastUpdated;
            model.PositionId = dto.PositionId;
            model.Title = dto.Title;
            model.ClassificationId = dto.ClassificationId;
        }
    }
}