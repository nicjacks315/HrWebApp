using CIS.HR.Models;
using CIS.HR.Models.DTO;

namespace CIS.HR.DAL
{
    //handles conversions between model and dto
    public class Mapper
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

        public virtual void MapToDTO(ContactPreferences model, ContactPreferencesDTO dto)
        {
            dto.EmployeeId = model.EmployeeId;
            dto.ContactPreferencesId = model.Id;
            dto.Phone1 = model.Phone1;
            dto.Phone2 = model.Phone2;
            dto.Email1 = model.Email1;
            dto.Email2 = model.Email2;
            dto.Extension = model.Extension;
            dto.DateEffective = model.DateEffective;
        }

        public virtual void MapToModel(ContactPreferencesDTO dto, ContactPreferences model)
        {
            model.Id = dto.ContactPreferencesId;
            model.EmployeeId = dto.EmployeeId;
            model.Phone1 = dto.Phone1;
            model.Phone2 = dto.Phone2;
            model.Email1 = dto.Email1;
            model.Email2 = dto.Email2;
            model.Extension = dto.Extension;
            model.DateEffective = dto.DateEffective;
        }

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