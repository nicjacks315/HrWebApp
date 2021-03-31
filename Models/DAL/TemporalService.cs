using System;
using System.Collections.Generic;
using System.Linq;
using CIS.HR.Models;
using CIS.HR.Models.DTO;
using FluentValidation;
using Microsoft.Ajax.Utilities;
using System.Data.Entity;

namespace CIS.HR
{
    namespace DAL
    {
        //public interface IRepository<T> where T : Entity
        //{
        //    void Add(T entity);
        //    void Update(T entity);
        //    void Delete(T entity);
        //    T Get(int? id);
        //}

        //public abstract class RepositoryBase<D, T> : IRepository<T> where T : Entity where D : DbContext
        //{
        //    protected IDbFactory<D> DbFactory
        //    {
        //        get;
        //        private set;
        //    }

        //    protected RepositoryBase()
        //    {
        //        _dbSet = _context.Set<T>();
        //    }

        //    protected D DbContext
        //    {
        //        get { return _context ?? (_context = DbFactory.Init()); }
        //    }

        //    private D _context;
        //    private readonly IDbSet<T> _dbSet;


        //    void IRepository<T>.Add(T entity)
        //    {
        //        _dbSet.Add(entity);
        //    }
        //    void IRepository<T>.Update(T entity)
        //    {
        //        _dbSet.Attach(entity);
        //        DbContext.Entry(entity).State = EntityState.Modified;
        //    }
        //    void IRepository<T>.Delete(T entity)
        //    {
        //        _dbSet.Remove(entity);
        //    }
        //    T IRepository<T>.Get(int? id)
        //    {
        //        return _dbSet.Find(id);
        //    }
        //}

        public abstract class Repository<T> where T : class
        {
            private DbContext _context;
            private IDbSet<T> _dbSet;

            Repository(DbContext context)
            {
                _context = context;
                _dbSet = context.Set<T>();
            }

            void Add(T entity)
            {
                _dbSet.Add(entity);
            }

            void Update(T entity)
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }

            void Remove(T entity)
            {
                _dbSet.Remove(entity);
            }

            T Get(int? id)
            {
                return _dbSet.Find(id);
            }
        }

        //resolves the point-in-time architecture and returns flattened DTOs for use in viewmodels and additional business logic
        //dependency injection for context and custom mappers
        public class TemporalService : Service
        {
            #region members
            private DateTime _timestamp;
            private Mapper _mapper;
            #endregion

            #region cctors
            public TemporalService(CIS.HR.Models.Context context = null, Mapper mapper = null) : base(context)
            {
                _timestamp = DateTime.Now;
                _mapper = mapper ?? new Mapper();
            }

            //~TemporalService()
            //{
            //    _context.Dispose();
            //}
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

            //update an employee record
            public bool UpdateEmployee(EmployeeDTO employee)
            {
                try
                {
                    var entity = GetEmployeeById(employee.EmployeeId);
                    //var entity = _context.Set<Employee>().Find(employee.EmployeeId);
                    if(entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.Employees.Attach(entity);
                    _mapper.MapToModel(employee, entity);
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

            //update an employee position
            public bool UpdateEmployeePosition(EmployeePositionDTO employeePosition)
            {
                try
                {
                    var entity = _context.PositionAssignments.Find(employeePosition.AssignmentId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.PositionAssignments.Attach(entity);
                    _mapper.MapToModel(employeePosition, entity);
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

            //update a contact preference record
            public bool UpdateContactPreference(ContactPreferencesDTO contactPreferences)
            {
                try
                {
                    var entity = _context.ContactPreferences.Find(contactPreferences.ContactPreferencesId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.ContactPreferences.Attach(entity);
                    _mapper.MapToModel(contactPreferences, entity);
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

            //update an employment status record
            public bool UpdateEmploymentStatus(EmployeeStatusDTO employeeStatus)
            {
                try
                {
                    var entity = _context.EmploymentStatusAssignments.Find(employeeStatus.AssignmentId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.EmploymentStatusAssignments.Attach(entity);
                    _mapper.MapToModel(employeeStatus, entity);
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

            //update an employee's supervisor assignment
            public bool UpdateEmployeeSupervisor(EmployeeManagerDTO employeeManager)
            {
                try
                {
                    var entity = _context.SupervisorAssignments.Find(employeeManager.AssignmentId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.SupervisorAssignments.Attach(entity);
                    _mapper.MapToModel(employeeManager, entity);
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

            //update an employee's coordinator assignment
            public bool UpdateEmployeeCoordinator(EmployeeManagerDTO employeeManager)
            {
                try
                {
                    var entity = _context.CoordinatorAssignments.Find(employeeManager.AssignmentId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.CoordinatorAssignments.Attach(entity);
                    _mapper.MapToModel(employeeManager, entity);
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

            //update an employee's department assignment
            public bool UpdateEmployeeDepartment(EmployeeDepartmentDTO employeeDepartment)
            {
                try
                {
                    var entity = _context.DepartmentAssignments.Find(employeeDepartment.AssignmentId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.DepartmentAssignments.Attach(entity);
                    _mapper.MapToModel(employeeDepartment, entity);
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

            //create a new position in service context, return success
            public bool CreatePosition(PositionDTO position)
            {
                try
                {
                    var model = new Position();
                    _mapper.MapToModel(position, model);
                    _context.Positions.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //create a new position description in service context, return success
            public bool CreatePositionDescription(PositionDTO position)
            {
                try
                {
                    var model = new PositionDescription();
                    _mapper.MapToModel(position, model);
                    _context.PositionDescriptions.Add(model);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //update a position
            public bool UpdatePosition(PositionDTO position)
            {
                try
                {
                    var entity = _context.Positions.Find(position.PositionId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.Positions.Attach(entity);
                    _mapper.MapToModel(position, entity);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //update a position description
            public bool UpdatePositionDescription(PositionDTO position)
            {
                try
                {
                    var entity = _context.PositionDescriptions.Find(position.PositionDescriptionId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.PositionDescriptions.Attach(entity);
                    _mapper.MapToModel(position, entity);
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

            //update an employee shift type
            public bool UpdateEmployeeShiftType(EmployeeShiftTypeDTO employeeShiftType)
            {
                try
                {
                    var entity = _context.ShiftTypeAssignments.Find(employeeShiftType.AssignmentId);
                    if (entity == null)
                    {
                        throw new InvalidOperationException();
                    }
                    _context.ShiftTypeAssignments.Attach(entity);
                    _mapper.MapToModel(employeeShiftType, entity);
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

        }
    }
}