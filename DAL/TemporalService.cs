using System;
using System.Collections.Generic;
using System.Linq;
using CIS.HR.Validators;
using CIS.HR.Models;
using CIS.HR.Models.DTO;
using FluentValidation;
using Microsoft.Ajax.Utilities;

namespace CIS.HR
{
    //namespace Models.DTO
    //{
    //    public class DTO { }

    //    public class EmployeeDTO : DTO
    //    {
    //        public int EmployeeId { get; set; }
    //        public string FirstName { get; set; }
    //        public string LastName { get; set; }
    //        public string Adp { get; set; }
    //        public string Username { get; set; }
    //        public bool IsSupervisor { get; set; }
    //        public bool IsCoordinator { get; set; }
    //        public bool IsDirector { get; set; }
    //        public bool IsLeader { get; set; }
    //    }

    //    public class ContactPreferencesDTO : DTO
    //    {
    //        public int ContactPreferencesId { get; set; }
    //        public int EmployeeId { get; set; }
    //        public DateTime DateEffective { get; set; }
    //        public string Phone1 { get; set; }
    //        public string Phone2 { get; set; }
    //        public string Email1 { get; set; }
    //        public string Email2 { get; set; }
    //        public string Extension { get; set; }
    //    }

    //    public class PositionDTO : DTO
    //    {
    //        public int PositionId { get; set; }
    //        public int PositionDescriptionId { get; set; }
    //        public int ClassificationId { get; set; }
    //        public string Code { get; set; }
    //        public string Title { get; set; }
    //        public DateTime LastUpdated { get; set; }
    //        public string Classification { get; set; }
    //    }

    //    public class EmployeePositionDTO : PositionDTO
    //    {
    //        public int AssignmentId { get; set; }
    //        public int EmployeeId { get; set; }
    //        public DateTime DateEffective { get; set; }
    //        public DateTime? DateAsPrimary { get; set; }
    //        public DateTime? DateStarted { get; set; }
    //        public DateTime? DateExited { get; set; }
    //    }

    //    public class EmployeeManagerDTO : EmployeeDTO
    //    {
    //        public int AssignmentId { get; set; }
    //        public int ManagerId { get; set; }
    //        public DateTime DateEffective { get; set; }
    //    }

    //    public class EmployeeStatusDTO : DTO
    //    {
    //        public int EmployeeId { get; set; }
    //        public int AssignmentId { get; set; }
    //        public int EmploymentStatusId { get; set; }
    //        public DateTime DateEffective { get; set; }
    //        public string StatusName { get; set; }
    //    }

    //    public class EmployeeDepartmentDTO : DTO
    //    {
    //        public int EmployeeId { get; set; }
    //        public int AssignmentId { get; set; }
    //        public int DepartmentId { get; set; }
    //        public DateTime DateEffective { get; set; }
    //        public string DepartmentName { get; set; }
    //        public string DepartmentAbbreviation { get; set; }
    //    }

    //    public class EmployeeShiftTypeDTO
    //    {
    //        public int EmployeeId { get; set; }
    //        public int AssignmentId { get; set; }
    //        public int ShiftTypeId { get; set; }
    //        public DateTime DateEffective { get; set; }
    //        public string ShiftType { get; set; }
    //    }
    //}

    namespace DAL
    {
        //handles conversions between model and dto
        //public class Mapper
        //{
        //    public virtual void MapToDTO(Employee model, EmployeeDTO dto)
        //    {
        //        dto.EmployeeId = model.Id;
        //        dto.FirstName = model.FirstName;
        //        dto.LastName = model.LastName;
        //        dto.Adp = model.Adp;
        //        dto.Username = model.Username;
        //        dto.IsSupervisor = model.IsSupervisor;
        //        dto.IsCoordinator = model.IsCoordinator;
        //        dto.IsDirector = model.IsDirector;
        //        dto.IsLeader = model.IsLeader;
        //    }

        //    public virtual void MapToDTO(Employee model, EmployeeManagerDTO dto)
        //    {
        //        dto.ManagerId = model.Id;
        //        dto.FirstName = model.FirstName;
        //        dto.LastName = model.LastName;
        //        dto.Adp = model.Adp;
        //        dto.Username = model.Username;
        //        dto.IsSupervisor = model.IsSupervisor;
        //        dto.IsCoordinator = model.IsCoordinator;
        //        dto.IsDirector = model.IsDirector;
        //        dto.IsLeader = model.IsLeader;
        //    }

        //    public virtual void MapToModel(EmployeeDTO dto, Employee model)
        //    {
        //        model.Id = dto.EmployeeId;
        //        model.FirstName = dto.FirstName;
        //        model.LastName = dto.LastName;
        //        model.Adp = dto.Adp;
        //        model.Username = dto.Username;
        //        model.IsDirector = dto.IsDirector;
        //        model.IsSupervisor = dto.IsSupervisor;
        //        model.IsCoordinator = dto.IsCoordinator;
        //        model.IsLeader = dto.IsLeader;
        //    }

        //    public virtual void MapToDTO(PositionAssignment modelA, PositionDescription modelB, EmployeePositionDTO dto)
        //    {
        //        dto.PositionId = modelA.PositionId;
        //        dto.EmployeeId = modelA.EmployeeId;
        //        dto.AssignmentId = modelA.Id;
        //        dto.DateAsPrimary = modelA.DateAsPrimary;
        //        dto.DateEffective = modelA.DateEffective;
        //        dto.DateExited = modelA.DateExited;
        //        dto.DateStarted = modelA.DateStarted;
        //        dto.Code = modelA.Position.Code;
        //        dto.Title = modelB.Title;
        //        dto.Classification = modelB.Classification.ClassificationName;
        //        dto.ClassificationId = modelB.ClassificationId;
        //        dto.PositionDescriptionId = modelB.Id;
        //    }

        //    public virtual void MapToModel(EmployeePositionDTO dto, PositionAssignment model)
        //    {
        //        model.Id = dto.AssignmentId;
        //        model.DateAsPrimary = dto.DateAsPrimary;
        //        model.DateEffective = dto.DateEffective;
        //        model.DateExited = dto.DateExited;
        //        model.DateStarted = dto.DateStarted;
        //        model.EmployeeId = dto.EmployeeId;
        //        model.PositionId = dto.PositionId;
        //    }

        //    public virtual void MapToDTO(ContactPreferences model, ContactPreferencesDTO dto)
        //    {
        //        dto.EmployeeId = model.EmployeeId;
        //        dto.ContactPreferencesId = model.Id;
        //        dto.Phone1 = model.Phone1;
        //        dto.Phone2 = model.Phone2;
        //        dto.Email1 = model.Email1;
        //        dto.Email2 = model.Email2;
        //        dto.Extension = model.Extension;
        //        dto.DateEffective = model.DateEffective;
        //    }

        //    public virtual void MapToModel(ContactPreferencesDTO dto, ContactPreferences model)
        //    {
        //        model.Id = dto.ContactPreferencesId;
        //        model.EmployeeId = dto.EmployeeId;
        //        model.Phone1 = dto.Phone1;
        //        model.Phone2 = dto.Phone2;
        //        model.Email1 = dto.Email1;
        //        model.Email2 = dto.Email2;
        //        model.Extension = dto.Extension;
        //        model.DateEffective = dto.DateEffective;
        //    }

        //    public virtual void MapToDTO(EmploymentStatusAssignment model, EmployeeStatusDTO dto)
        //    {
        //        dto.EmployeeId = model.EmployeeId;
        //        dto.AssignmentId = model.Id;
        //        dto.EmploymentStatusId = model.EmploymentStatus.Id;
        //        dto.DateEffective = model.DateEffective;
        //        dto.StatusName = model.EmploymentStatus.StatusName;
        //    }

        //    public virtual void MapToModel(EmployeeStatusDTO dto, EmploymentStatusAssignment model)
        //    {
        //        model.EmployeeId = dto.EmployeeId;
        //        model.Id = dto.AssignmentId;
        //        model.EmploymentStatus.Id = dto.EmploymentStatusId;
        //        model.DateEffective = dto.DateEffective;
        //    }

        //    public virtual void MapToDTO(SupervisorAssignment model, EmployeeManagerDTO dto)
        //    {
        //        MapToDTO(model.Supervisor, dto);
        //        dto.AssignmentId = model.Id;
        //        dto.EmployeeId = model.EmployeeId;
        //        dto.DateEffective = model.DateEffective;
        //    }

        //    public virtual void MapToModel(EmployeeManagerDTO dto, SupervisorAssignment model)
        //    {
        //        model.Id = dto.AssignmentId;
        //        model.EmployeeId = dto.EmployeeId;
        //        model.SupervisorId = dto.ManagerId;
        //        model.DateEffective = dto.DateEffective;
        //    }

        //    public virtual void MapToDTO(CoordinatorAssignment model, EmployeeManagerDTO dto)
        //    {
        //        MapToDTO(model.Coordinator, dto);
        //        dto.AssignmentId = model.Id;
        //        dto.EmployeeId = model.EmployeeId;
        //        dto.DateEffective = model.DateEffective;
        //    }

        //    public virtual void MapToModel(EmployeeManagerDTO dto, CoordinatorAssignment model)
        //    {
        //        model.Id = dto.AssignmentId;
        //        model.EmployeeId = dto.EmployeeId;
        //        model.CoordinatorId = dto.ManagerId;
        //        model.DateEffective = dto.DateEffective;
        //    }

        //    public virtual void MapToDTO(DepartmentAssignment model, EmployeeDepartmentDTO dto)
        //    {
        //        dto.AssignmentId = model.Id;
        //        dto.EmployeeId = model.EmployeeId;
        //        dto.DepartmentId = model.DepartmentId;
        //        dto.DateEffective = model.DateEffective;
        //        dto.DepartmentAbbreviation = model.Department.Abbreviation;
        //        dto.DepartmentName = model.Department.DepartmentName;
        //    }

        //    public virtual void MapToModel(EmployeeDepartmentDTO dto, DepartmentAssignment model)
        //    {
        //        model.Id = dto.AssignmentId;
        //        model.DateEffective = dto.DateEffective;
        //        model.EmployeeId = dto.EmployeeId;
        //        model.DepartmentId = dto.DepartmentId;
        //    }

        //    public virtual void MapToDTO(Position model, PositionDTO dto)
        //    {
        //        dto.PositionId = model.Id;
        //        dto.Code = model.Code;
        //    }

        //    public virtual void MapToModel(PositionDTO dto, Position model)
        //    {
        //        model.Id = dto.PositionId;
        //        model.Code = dto.Code;
        //    }

        //    public virtual void MapToDTO(PositionDescription model, PositionDTO dto)
        //    {
        //        dto.PositionDescriptionId = model.Id;
        //        dto.Title = model.Title;
        //        dto.LastUpdated = model.DateEffective;
        //        dto.ClassificationId = model.ClassificationId;
        //        dto.Classification = model.Classification.ClassificationName;
        //    }

        //    public virtual void MapToModel(PositionDTO dto, PositionDescription model)
        //    {
        //        model.Id = dto.PositionDescriptionId;
        //        model.DateEffective = dto.LastUpdated;
        //        model.PositionId = dto.PositionId;
        //        model.Title = dto.Title;
        //        model.ClassificationId = dto.ClassificationId;
        //    }

        //    public virtual void MapToDTO(ShiftTypeAssignment model, EmployeeShiftTypeDTO dto)
        //    {
        //        dto.EmployeeId = model.EmployeeId;
        //        dto.AssignmentId = model.Id;
        //        dto.ShiftTypeId = model.ShiftTypeId;
        //        dto.ShiftType = model.ShiftType.ShiftTypeName;
        //        dto.DateEffective = model.DateEffective;
        //    }

        //    public virtual void MapToModel(EmployeeShiftTypeDTO dto, ShiftTypeAssignment model)
        //    {
        //        model.Id = dto.AssignmentId;
        //        model.EmployeeId = dto.EmployeeId;
        //        model.ShiftTypeId = dto.ShiftTypeId;
        //        model.DateEffective = dto.DateEffective;
        //    }
        //}


        //resolves the point-in-time architecture and returns flattened DTOs for use in viewmodels and additional business logic
        //dependency injection for context and custom mappers
        public class TemporalService : Service
        {
            #region members
            private DateTime _timestamp;
            private Mapper _mapper;
            #endregion

            #region cctors
            public TemporalService(CIS.HR.Models.Context context, Mapper mapper = null) : base(context)
            {
                _timestamp = DateTime.Now;
                _mapper = mapper ?? new Mapper();
            }
            #endregion

            #region methods
            //returns employee by id, throws an exception on invalid employee
            private Employee GetEmployeeById(int? employeeId)
            {
                if (employeeId == null)
                {
                    throw new Exception("Missing employee id!");
                }
                var employee = _context.Employees.Find(employeeId);
                //if ( employee == null )
                //{
                //    throw new Exception("Employee with id (" + employeeId.ToString() + ") does not exist in current context!");
                //}
                return employee;
            }

            //returns position by id, throws an exception on invalid position
            private Position GetPositionById(int? positionId)
            {
                if( positionId == null )
                {
                    throw new Exception("Missing position id!");
                }
                var position = _context.Positions.Find(positionId);
                //if (position == null)
                //{
                //    throw new Exception("Position with id (" + positionId.ToString() + ") does not exist in current context!");
                //}
                return position;
            }

            //returns employee by id
            public EmployeeDTO GetEmployee(int? employeeId)
            {
                var employee = GetEmployeeById(employeeId);
                var dto = new EmployeeDTO();
                _mapper.MapToDTO(employee, dto);
                return dto;
            }

            //create an employee record in the service context, return successs
            public bool CreateEmployee(EmployeeDTO employee)
            {
                try
                {
                    var newEmployee = new Employee();
                    _mapper.MapToModel(employee, newEmployee);
                    _context.Employees.Add(newEmployee);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //resolve proper position information and map position dto to employee position dto
            private PositionDescription GetRelevantPositionDescription(PositionAssignment positionAssignment, DateTime? asOfDate = null)
            {
                var relevantDate = (positionAssignment.DateExited ?? asOfDate) < asOfDate ? positionAssignment.DateExited : asOfDate;
                var pd = positionAssignment.Position.PositionDescriptionHistory
                    .Where(e => e.DateEffective <= relevantDate)
                    .OrderByDescending(e => e.DateEffective)
                    .FirstOrDefault();

                return pd ?? new PositionDescription()
                {
                    Title = "No Effective Position Description",
                    DateEffective = DateTime.MinValue,
                    PositionId = positionAssignment.Position.Id,
                    ClassificationId = 1,
                    Classification = _context.PositionClassifications.Find(1)
                };
            }

            //return a list of all employee's positions
            public List<EmployeePositionDTO> GetAllEmployeePositions(int? employeeId, DateTime? asOfDate = null)
            {
                var employee = GetEmployeeById(employeeId);
                var dateTime = asOfDate ?? _timestamp;
                var epa = employee.PositionAssignmentHistory
                    .OrderByDescending( e => e.DateEffective )
                    .OrderByDescending( e => e.DateStarted )
                    //.OrderByDescending(e => e.DateAsPrimary)
                    //.ThenByDescending(e => e.DateEffective)
                    .ToList();

                var dtoList = new List<EmployeePositionDTO>();
                foreach (PositionAssignment pa in epa)
                {
                    var dto = new EmployeePositionDTO();
                    var pd = GetRelevantPositionDescription(pa, dateTime);
                    _mapper.MapToDTO(pa, pd, dto);
                    //AppendTemporalDataToEmployeePositionDTO(dto, pa, dateTime);
                    dtoList.Add(dto);
                }
                return dtoList;
            }

            //return a list of the employee's effective positions as of
            public List<EmployeePositionDTO> GetEffectiveEmployeePositions(int? employeeId, DateTime? asOfDate = null)
            {
                var employee = GetEmployeeById(employeeId);
                var dateTime = asOfDate ?? _timestamp;
                var epa = employee.PositionAssignmentHistory
                    .Where(e => e.DateEffective <= dateTime)
                    .Where(e => e.DateExited == null || e.DateExited > dateTime)
                    .OrderByDescending(e => e.DateAsPrimary)
                    .ThenByDescending(e => e.DateEffective)
                    .ToList();

                var dtoList = new List<EmployeePositionDTO>();
                foreach (PositionAssignment pa in epa)
                {
                    var dto = new EmployeePositionDTO();
                    var pd = GetRelevantPositionDescription(pa, dateTime);
                    _mapper.MapToDTO(pa, pd, dto);
                    //AppendTemporalDataToEmployeePositionDTO(dto, pa, dateTime);
                    dtoList.Add(dto);
                }
                return dtoList;
            }

            //create a new position assignment in the service context, return success
            public bool CreateEmployeePosition(EmployeePositionDTO employeePosition)
            {
                try
                {
                    var model = new PositionAssignment();
                    _mapper.MapToModel(employeePosition, model);
                    _context.PositionAssignments.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //return employee's contact info as of
            public ContactPreferencesDTO GetContactPreferences(int? employeeId, DateTime? asOfDate = null)
            {
                var dto = new ContactPreferencesDTO();
                var employee = GetEmployeeById(employeeId);
                if (employee != null)
                {
                    var cp = employee.ContactPreferencesHistory
                        .Where(e => e.DateEffective <= (asOfDate ?? _timestamp))
                        .OrderByDescending(e => e.DateEffective)
                        .FirstOrDefault();

                    if (cp != null)
                    {
                        _mapper.MapToDTO(cp, dto);
                    }
                }
                return dto;
            }

            //create a contact preference record in the service context, return success
            public bool CreateContactPreferences(ContactPreferencesDTO contactPreferences)
            {
                try
                {
                    var model = new ContactPreferences();
                    _mapper.MapToModel(contactPreferences, model);
                    _context.ContactPreferences.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //returns the employee's status as of
            public EmployeeStatusDTO GetEmploymentStatus(int? employeeId, DateTime? asOfDate = null)
            {
                var dto = new EmployeeStatusDTO();
                var employee = GetEmployeeById(employeeId);
                if (employee != null)
                {
                    var esa = employee.EmploymentStatusAssignmentHistory
                        .Where(e => e.DateEffective <= (asOfDate ?? _timestamp))
                        .OrderByDescending(e => e.DateEffective)
                        .FirstOrDefault();

                    if (esa != null)
                    {
                        _mapper.MapToDTO(esa, dto);
                    }
                }
                return dto;
            }

            //create an employment status record in service context, return success
            public bool CreateEmploymentStatus(EmployeeStatusDTO employmentStatus)
            {
                try
                {
                    var model = new EmploymentStatusAssignment();
                    _mapper.MapToModel(employmentStatus, model);
                    _context.EmploymentStatusAssignments.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //returns the employee's effective supervisor as of
            public EmployeeManagerDTO GetSupervisor(int? employeeId, DateTime? asOfDate = null)
            {
                EmployeeManagerDTO dto = new EmployeeManagerDTO();

                var employee = GetEmployeeById(employeeId);
                if (employee != null)
                {
                    var sa = employee.SupervisorAssignmentHistory
                            .Where(e => e.DateEffective <= (asOfDate ?? _timestamp))
                            .OrderByDescending(e => e.DateEffective)
                            .FirstOrDefault();


                    if (sa != null)
                    {
                        _mapper.MapToDTO(sa, dto);
                    }
                }
                return dto;
            }

            //create a supervisor assignment record in service context, return success
            public bool CreateEmployeeSupervisor(EmployeeManagerDTO employeeManager)
            {
                try
                {
                    var model = new SupervisorAssignment();
                    _mapper.MapToModel(employeeManager, model);
                    _context.SupervisorAssignments.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //returns the employee's effective coordinator as of
            public EmployeeManagerDTO GetCoordinator(int? employeeId, DateTime? asOfDate = null)
            {
                EmployeeManagerDTO dto = new EmployeeManagerDTO();
                var employee = GetEmployeeById(employeeId);
                if (employee != null)
                {
                    var eca = employee.CoordinatorAssignmentHistory
                        .Where(e => e.DateEffective <= (asOfDate ?? _timestamp))
                        .OrderByDescending(e => e.DateEffective)
                        .FirstOrDefault();

                    if (eca != null)
                    {
                        _mapper.MapToDTO(eca, dto);
                    }
                }
                return dto;
            }

            //create a coordinator assignment record in service context, return success
            public bool CreateEmployeeCoordinator(EmployeeManagerDTO employeeManager)
            {
                try
                {
                    var model = new CoordinatorAssignment();
                    _mapper.MapToModel(employeeManager, model);
                    _context.CoordinatorAssignments.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //returns employee's assigned department(s), or an empty list
            public List<EmployeeDepartmentDTO> GetDepartments(int? employeeId, DateTime? asOfDate = null)
            {
                var employee = GetEmployeeById(employeeId);

                var dateTime = asOfDate ?? _timestamp;
                var eda = employee.DepartmentAssignmentHistory
                    .DistinctBy(e => e.DepartmentId)
                    .Where(e => e.DateEffective <= dateTime)
                    .OrderByDescending(e => e.DateEffective);

                var dtoList = new List<EmployeeDepartmentDTO>();
                foreach( DepartmentAssignment da in eda )
                {
                    var dto = new EmployeeDepartmentDTO();
                    _mapper.MapToDTO(da, dto);
                    dtoList.Add(dto);
                }
                return dtoList;
            }

            //returns employee's most recently assigned department, or an empty DTO
            public EmployeeDepartmentDTO GetDepartment(int? employeeId, DateTime? asOfDate = null)
            {
                var departments = GetDepartments(employeeId, asOfDate);
                return departments.Count() > 0 ? departments[0] : new EmployeeDepartmentDTO();
            }

            //create department assignment record in service context, return success
            public bool CreateEmployeeDepartment(EmployeeDepartmentDTO employeeDepartment)
            {
                try
                {
                    var model = new DepartmentAssignment();
                    _mapper.MapToModel(employeeDepartment, model);
                    _context.DepartmentAssignments.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //returns a position dto from id
            public PositionDTO GetPosition(int? positionId, DateTime? asOfDate = null)
            {   
                var p = GetPositionById(positionId);

                var dateTime = asOfDate ?? _timestamp;
                var pd = p.PositionDescriptionHistory
                    .Where(e => e.DateEffective <= dateTime)
                    .OrderByDescending(e => e.DateEffective)
                    .FirstOrDefault();

                var dto = new PositionDTO();
                if( p != null )
                {
                    _mapper.MapToDTO(p, dto);
                }
                if( pd!= null )
                {
                    _mapper.MapToDTO(pd, dto);
                }
                return dto;
            }

            //create a new position and position description in service context, return success
            public bool CreatePosition(PositionDTO position)
            {
                try
                {
                    var modelA = new Position();
                    var modelB = new PositionDescription();
                    _mapper.MapToModel(position, modelA);
                    _context.Positions.Add(modelA);
                    _mapper.MapToModel(position, modelB);
                    _context.PositionDescriptions.Add(modelB);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //returns employee's shift type
            public EmployeeShiftTypeDTO GetShiftType(int? employeeId, DateTime? asOfDate = null)
            {
                var dto = new EmployeeShiftTypeDTO();
                var employee = GetEmployeeById(employeeId);
                if (employee != null)
                {
                    var esa = employee.ShiftTypeAssignmentHistory
                        .Where(e => e.DateEffective <= (asOfDate ?? _timestamp))
                        .OrderByDescending(e => e.DateEffective)
                        .FirstOrDefault();

                    if (esa != null)
                    {
                        _mapper.MapToDTO(esa, dto);
                    }
                }
                return dto;
            }

            //create a shift type assignment record in service context, return success
            public bool CreateEmployeeShiftType(EmployeeShiftTypeDTO employeeShiftType)
            {
                try
                {
                    var model = new ShiftTypeAssignment();
                    _mapper.MapToModel(employeeShiftType, model);
                    _context.ShiftTypeAssignments.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //return the employee's hire date, accounting for terminations and rehires
            public DateTime? GetStartDate(int? employeeId, DateTime? asOfDate = null)
            {
                var employee = _context.Employees.Find(employeeId);
                if (employee != null)
                {
                    var contextDateTime = (asOfDate ?? _timestamp);

                    var mostRecentTermination = employee.EmploymentStatusAssignmentHistory
                                                    .Where(e => e.EmploymentStatusId == 5)
                                                    .Where(e => e.DateEffective >= contextDateTime)
                                                    .OrderBy(e => e.DateEffective)
                                                    .FirstOrDefault();

                    var terminationDateTime = mostRecentTermination?.DateEffective;

                    var oldestPosition = employee.PositionAssignmentHistory
                                                    .Where(e => e.DateEffective >= (terminationDateTime ?? DateTime.MinValue) )
                                                    .OrderBy(e => e.DateStarted)
                                                    .FirstOrDefault();

                    return oldestPosition?.DateStarted;
                }
                return null;
            }

            //return the employee's first start date
            public DateTime? GetOriginalStartDate(int? employeeId, DateTime? asOfDate = null)
            {
                var employee = _context.Employees.Find(employeeId);

                return employee?
                    .PositionAssignmentHistory
                    .OrderBy(e => e.DateStarted)
                    .FirstOrDefault()?
                    .DateStarted;

                //if (employee != null)
                //{
                //    var oldestPosition = employee.PositionAssignmentHistory
                //                                    .OrderBy(e => e.DateStarted)
                //                                    .FirstOrDefault();

                //    return oldestPosition?.DateStarted;
                //}
                //return null;
            }
            #endregion

            #region backup
            //public class PositionAssignmentDTO : TemporalEntityDTO
            //{
            //    public DateTime? DateAsPrimary { get; set; }
            //    public DateTime? DateStarted { get; set; }
            //    public DateTime? DateExited { get; set; }
            //    public int PositionId { get; set; }
            //    public int PositionDescriptionId { get; set; }
            //}

            //public List<PositionAssignmentDTO> GetEmployeePositions(int? employeeId, DateTime? asOfDate = null)
            //{
            //    ValidateEmployee(employeeId);
            //    var dateTime = asOfDate ?? _timestamp;
            //    var employee = _context.Employees.Find(employeeId);
            //    var epa = employee.PositionAssignmentHistory.Effective(dateTime);
            //    epa = epa.Where(e => e.DateExited == null || e.DateExited > dateTime).OrderByDescending(e => e.DateAsPrimary).ToList();
            //    var dtoList = new List<PositionAssignmentDTO>();
            //    if (epa.Count == 0)
            //    {
            //        dtoList.Add(new PositionAssignmentDTO());
            //    }
            //    else
            //    {
            //        foreach (PositionAssignment pa in epa)
            //        {
            //            //resolve the position info at the exit date if necessary
            //            var positionDate = (pa.DateExited ?? dateTime) < dateTime ? pa.DateExited : dateTime;
            //            dtoList.Add(new PositionAssignmentDTO()
            //            {
            //                Id = pa.Id,
            //                DateAsPrimary = pa.DateAsPrimary,
            //                DateEffective = pa.DateEffective,
            //                DateExited = pa.DateExited,
            //                DateStarted = pa.DateStarted,
            //                PositionId = pa.Position.Id,
            //                PositionDescriptionId = GetPositionDescription(pa.Position.Id, positionDate).Id
            //            });
            //        }
            //    }
            //    return dtoList;
            //}


            //public SupervisorAssignmentDTO GetSupervisor(int? employeeId, DateTime? asOfDate = null)
            //{
            //    ValidateEmployee(employeeId);
            //    var dateTime = asOfDate ?? _timestamp;
            //    var employee = _context.Employees.Find(employeeId);
            //    var sa = employee.SupervisorAssignmentHistory.Effective(dateTime).FirstOrDefault();

            //    var mdta = (EmployeeManagerDTO)(GetEmployee(sa.SupervisorId));

            //    var dto = new SupervisorAssignmentDTO();
            //    if (sa != null)
            //    {
            //        dto.Id = sa.Id;
            //        dto.DateEffective = sa.DateEffective;
            //        dto.SupervisorId = sa.Supervisor.Id;
            //    }
            //    return dto;
            //}


            //public CoordinatorAssignmentDTO GetCoordinator(int? employeeId, DateTime? asOfDate = null)
            //{
            //    ValidateEmployee(employeeId);
            //    var dateTime = asOfDate ?? _timestamp;
            //    var employee = _context.Employees.Find(employeeId);
            //    var ca = employee.CoordinatorAssignmentHistory.Effective(dateTime).FirstOrDefault();
            //    var dto = new CoordinatorAssignmentDTO();
            //    if (ca != null)
            //    {
            //        dto.Id = ca.Id;
            //        dto.DateEffective = ca.DateEffective;
            //        dto.CoordinatorId = ca.Coordinator.Id;
            //    }
            //    return dto;
            //}

            //returns the effective position description as of 
            //public PositionDescriptionDTO GetPositionDescription(int? positionId, DateTime? asOfDate = null)
            //{
            //    ValidatePosition(positionId);
            //    var dateTime = asOfDate ?? _timestamp;
            //    var p = _context.Positions.Find(positionId);
            //    var pd = p.PositionDescriptionHistory.Effective(dateTime).FirstOrDefault();
            //    var dto = new PositionDescriptionDTO();
            //    if (pd != null)
            //    {
            //        dto.Id = pd.Id;
            //        dto.Title = pd.Title;
            //        dto.DateEffective = pd.DateEffective;
            //    }
            //    return dto;
            //}

            //resolve proper position information and map position dto to employee position dto
            //private void AppendTemporalDataToEmployeePositionDTO( EmployeePositionDTO dto, PositionAssignment model, DateTime asOfDate )
            //{
            //    //resolve the position info at the exit date if it is before the test date
            //    var relevantDate = (model.DateExited ?? asOfDate) < asOfDate ? model.DateExited : asOfDate;
            //    var p = GetPosition(model.Position.Id, relevantDate);
            //    if (p != null)
            //    {
            //        dto.PositionId = p.PositionId;
            //        dto.Code = p.Code;
            //        dto.Title = p.Title;
            //        dto.Classification = p.Classification;
            //        dto.ClassificationId = p.ClassificationId;
            //    }
            //}
            #endregion
        }
    }
}