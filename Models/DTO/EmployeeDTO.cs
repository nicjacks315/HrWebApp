using CIS.HR.Models.DTO;

namespace CIS.HR.Models
{
    namespace DTO
    {
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
    }

    public partial class Mapper
    {
        public virtual void MapToDTO(Employee model, EmployeeDTO dto)
        {
            dto.EmployeeId = model.Id;
            dto.FirstName = model.FirstName;
            dto.LastName = model.LastName;
            dto.Adp = model.Adp;
            dto.Username = model.Username;
            dto.IsSupervisor = model.IsSupervisor;
            dto.IsCoordinator = model.IsCoordinator;
            dto.IsDirector = model.IsDirector;
            dto.IsLeader = model.IsLeader;
        }

        public virtual void MapToModel(EmployeeDTO dto, Employee model)
        {
            model.Id = dto.EmployeeId;
            model.FirstName = dto.FirstName;
            model.LastName = dto.LastName;
            model.Adp = dto.Adp;
            model.Username = dto.Username;
            model.IsDirector = dto.IsDirector;
            model.IsSupervisor = dto.IsSupervisor;
            model.IsCoordinator = dto.IsCoordinator;
            model.IsLeader = dto.IsLeader;
        }
    }
}