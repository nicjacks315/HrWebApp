using System;
using CIS.HR.Models.DTO;

namespace CIS.HR.Models
{
    namespace DTO
    {
        public class EmployeeManagerDTO : EmployeeDTO
        {
            public int AssignmentId { get; set; }
            public int ManagerId { get; set; }
            public DateTime DateEffective { get; set; }
        }
    }

    public partial class Mapper
    {
        public virtual void MapToDTO(Employee model, EmployeeManagerDTO dto)
        {
            dto.ManagerId = model.Id;
            dto.FirstName = model.FirstName;
            dto.LastName = model.LastName;
            dto.Adp = model.Adp;
            dto.Username = model.Username;
            dto.IsSupervisor = model.IsSupervisor;
            dto.IsCoordinator = model.IsCoordinator;
            dto.IsDirector = model.IsDirector;
            dto.IsLeader = model.IsLeader;
        }

        public virtual void MapToDTO(SupervisorAssignment model, EmployeeManagerDTO dto)
        {
            MapToDTO(model.Supervisor, dto);
            dto.AssignmentId = model.Id;
            dto.EmployeeId = model.EmployeeId;
            dto.DateEffective = model.DateEffective;
        }

        public virtual void MapToModel(EmployeeManagerDTO dto, SupervisorAssignment model)
        {
            model.Id = dto.AssignmentId;
            model.EmployeeId = dto.EmployeeId;
            model.SupervisorId = dto.ManagerId;
            model.DateEffective = dto.DateEffective;
        }

        public virtual void MapToDTO(CoordinatorAssignment model, EmployeeManagerDTO dto)
        {
            MapToDTO(model.Coordinator, dto);
            dto.AssignmentId = model.Id;
            dto.EmployeeId = model.EmployeeId;
            dto.DateEffective = model.DateEffective;
        }

        public virtual void MapToModel(EmployeeManagerDTO dto, CoordinatorAssignment model)
        {
            model.Id = dto.AssignmentId;
            model.EmployeeId = dto.EmployeeId;
            model.CoordinatorId = dto.ManagerId;
            model.DateEffective = dto.DateEffective;
        }
    }
}