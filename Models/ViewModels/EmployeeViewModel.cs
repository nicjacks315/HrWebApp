using CIS.HR.Models;
using CIS.HR.ViewModels;
using CIS.HR.Validators;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;
using CIS.HR.Models.DTO;

namespace CIS.HR
{
    namespace ViewModels
    {
        public class EmployeeIndexViewModel
        {
            public int EmployeeId { get; set; }
            public string Adp { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            public string PositionCode { get; set; }
            public string PositionTitle { get; set; }
            public string ShiftType { get; set; }
            public string EmploymentStatus { get; set; }
            public string Department { get; set; }
            public string Supervisor { get; set; }
            public string Coordinator { get; set; }
        }


        public class EmployeeDetailsViewModel
        {
            #region fields
            [DisplayName("First Name")]
            public string FirstName { get; set; }

            [DisplayName("Last Name")]
            public string LastName { get; set; }

            [DisplayName("Username")]
            public string Username { get; set; }

            [DisplayName("ADP")]
            public string Adp { get; set; }
            #endregion

            #region properties
            public ManagerViewModel Supervisor { get; set; }
            public ManagerViewModel Coordinator { get; set; }
            public ContactPreferencesViewModel ContactPreferences { get; set; }
            public PositionHistoryViewModel Positions { get; set; }
            #endregion
        }

        public class ExtendedEmployeeDetailsViewModel : EmployeeDetailsViewModel
        {
            #region fields
            [DisplayName("Start Date")]
            public string StartDate { get; set; }

            [DisplayName("Department(s)")]
            public string Departments { get; set; }

            [DisplayName("Shift Type")]
            public string ShiftType { get; set; }
            #endregion
        }

        public class PositionHistoryViewModel : List<EmployeePositionViewModel>
        {
            public PositionHistoryViewModel()
            { }

            public PositionHistoryViewModel( List<EmployeePositionDTO> positionDtoList )
            {
                foreach( EmployeePositionDTO dto in positionDtoList )
                {
                    Add(new EmployeePositionViewModel(dto));
                }
            }
        }

        public class ContactPreferencesViewModel
        {
            public ContactPreferencesViewModel()
            { }

            public ContactPreferencesViewModel( ContactPreferencesDTO dto )
            {
                Phone1 = dto.Phone1;
                Phone2 = dto.Phone2;
                Email1 = dto.Email1;
                Email2 = dto.Email2;
                Extension = dto.Extension;
                DateEffective = dto.DateEffective.ToShortDateString();
            }

            #region fields
            [DisplayName("Primary Phone")]
            public string Phone1 { get; set; }

            [DisplayName("Secondary Phone")]
            public string Phone2 { get; set; }

            [DisplayName("Primary Email")]
            public string Email1 { get; set; }

            [DisplayName("Secondary Email")]
            public string Email2 { get; set; }

            [DisplayName("Office Extension")]
            public string Extension { get; set; }

            [DisplayName("Last Updated")]
            public string DateEffective { get; set; }
            #endregion
        }

        public class ManagerViewModel
        {
            public ManagerViewModel()
            { }

            public ManagerViewModel( EmployeeManagerDTO managerDto, ContactPreferencesDTO contactPreferenceDto )
            {
                var bTwoNames = !String.IsNullOrEmpty(managerDto.FirstName) && !String.IsNullOrEmpty(managerDto.LastName);
                Name = managerDto.FirstName + (bTwoNames ? " " : "") + managerDto.LastName;
                DateEffective = managerDto.DateEffective.ToShortDateString();
                ContactPreferences = new ContactPreferencesViewModel(contactPreferenceDto);
            }

            #region fields
            [DisplayName("Name")]
            public string Name { get; set; }

            [DisplayName("Effective Date")]
            public string DateEffective { get; set; }
            #endregion

            #region properties
            public ContactPreferencesViewModel ContactPreferences { get; set; }
            #endregion
        }

        public class EmployeePositionViewModel
        {
            public EmployeePositionViewModel()
            { }

            public EmployeePositionViewModel( EmployeePositionDTO dto )
            {
                Id = dto.EmployeeId;
                IsPrimary = (dto.DateAsPrimary != null);
                DateEffective = dto.DateEffective.ToShortDateString();
                DateStarted = dto.DateStarted?.ToShortDateString();
                DateExited = dto.DateExited?.ToShortDateString();
                Code = dto.Code;
                Title = dto.Title;
            }

            #region fields
            [DisplayName("Position ID")]
            public int Id { get; set; }

            [DisplayName("Primary")]
            public bool IsPrimary { get; set; }

            [DisplayName("Effective Date")]
            public string DateEffective { get; set; }

            [DisplayName("Date Started")]
            public string DateStarted { get; set; }

            [DisplayName("Date Exited")]
            public string DateExited { get; set; }

            [DisplayName("Code")]
            public string Code { get; set; }

            [DisplayName("Title")]
            public string Title { get; set; }
            #endregion
        }
    }

    //public class TemporalPositionService
    //{
    //    private Position _position;
    //    private DateTime _asOfDate;

    //    public TemporalPositionService(Position position, DateTime? asOfDate = null)
    //    {
    //        _position = position;
    //        _asOfDate = asOfDate ?? DateTime.Now;
    //    }

    //    public PositionDescriptionDTO GetPositionDescription()
    //    {
    //        var pd = _position.PositionDescriptionHistory.EffectiveOrOldest(_asOfDate).FirstOrDefault();
    //        var dto = new PositionDescriptionDTO();
    //        if( pd != null )
    //        {
    //            dto.Id = pd.Id;
    //            dto.Title = pd.Title;
    //            dto.DateEffective = pd.DateEffective;
    //        }
    //        return dto;
    //    }

    //    public PositionDTO GetPosition()
    //    {
    //        return new PositionDTO()
    //        {
    //            Id = _position.Id,
    //            Code = _position.Code//,
    //            //PositionDescriptionId = GetPositionDescription().Id
    //        };
    //    }
    //}

    //class TemporalEmployeeService
    //{
    //    private Employee _employee;
    //    private DateTime _asOfDate;

    //    public TemporalEmployeeService(Employee employee, DateTime? asOfDate = null)
    //    {
    //        _employee = employee;
    //        _asOfDate = asOfDate ?? DateTime.Now;
    //    }

    //    private PositionViewModel Map(PositionAssignment pa)
    //    {
    //        var positionViewModel = new PositionViewModel();
    //        positionViewModel.Id = pa.Id;
    //        positionViewModel.PositionId = pa.Position.Id;
    //        positionViewModel.IsPrimary = pa.DateAsPrimary != null;
    //        positionViewModel.DateEffective = pa.DateEffective.ToShortDateString();
    //        positionViewModel.DateStarted = pa.DateStarted?.ToShortDateString();
    //        positionViewModel.DateExited = pa.DateExited?.ToShortDateString();
    //        positionViewModel.Code = pa.Position.Code;
    //        return positionViewModel;
    //    }

    //    public PositionViewModel GetPrimaryPosition()
    //    {
    //        //round up the effective, non-exited position assignments
    //        var positionAssignments = _employee.PositionAssignmentHistory.Effective(_asOfDate);
    //        positionAssignments = positionAssignments.Where(pa => pa.DateExited == null || pa.DateExited > _asOfDate).ToList();

    //        var positionViewModel = new PositionViewModel();
    //        if (positionAssignments.Count > 0)
    //        {
    //            //positionViewModel.Id = positionAssignments.First().Id;
    //            //positionViewModel.PositionId = positionAssignments.First().Position.Id;
    //            //positionViewModel.IsPrimary = positionAssignments.First().DateAsPrimary != null;
    //            //positionViewModel.DateEffective = positionAssignments.First().DateEffective.ToShortDateString();
    //            //positionViewModel.DateStarted = positionAssignments.First().DateStarted?.ToShortDateString();
    //            //positionViewModel.DateExited = positionAssignments.First().DateExited?.ToShortDateString();
    //            //positionViewModel.Code = positionAssignments.First().Position.Code;
    //            positionViewModel = Map(positionAssignments.First());
    //            //var positionDescriptions = positionAssignments.First().Position.PositionDescriptionHistory.EffectiveOrOldest(_asOfDate);
    //            var service = new TemporalPositionService(positionAssignments.First().Position, _asOfDate);
    //            //if (service.positionDescriptions.Count > 0)
    //            //{
    //            //    positionViewModel.Title = positionDescriptions.First().Title;
    //            //}
    //            positionViewModel.Title = service.GetPositionDescription().Title;
    //        }
    //        return positionViewModel;
    //    }

    //    public List<PositionViewModel> GetEffectivePositions()
    //    {
    //        var positions = new List<PositionViewModel>();
    //        foreach (PositionAssignment pa in _employee.PositionAssignmentHistory.Effective(_asOfDate))
    //        {
    //            //if the employee exited before the test date, use the exit date to resolve position details
    //            var positionDate = (pa.DateExited ?? _asOfDate) > _asOfDate ? pa.DateExited : _asOfDate;
    //            var service = new TemporalPositionService(pa.Position, positionDate);
    //            var pvm = Map(pa);
    //            pvm.Title = service.GetPositionDescription().Title;
    //            positions.Add(pvm);
    //        }
    //        return positions;
    //    }

    //    public ManagerViewModel GetSupervisor()
    //    {
    //        var managerViewModel = new ManagerViewModel();
    //        var effectiveSupervisorAssignments = _employee.SupervisorAssignmentHistory.Effective(_asOfDate);
    //        if (effectiveSupervisorAssignments.Count > 0)
    //        {
    //            var supervisorAssignment = effectiveSupervisorAssignments.First();
    //            managerViewModel.DateEffective = supervisorAssignment.DateEffective.ToShortDateString();
    //            managerViewModel.Name = supervisorAssignment.Supervisor.FirstName + " " + supervisorAssignment.Supervisor.LastName;

    //            //always show current information for manager
    //            var supervisor = new TemporalEmployeeService(supervisorAssignment.Supervisor, DateTime.Now);
    //            managerViewModel.ContactPreferences = supervisor.GetContactPreferences();
    //        }
    //        return managerViewModel;
    //    }

    //    public ManagerViewModel GetCoordinator()
    //    {
    //        var managerViewModel = new ManagerViewModel();
    //        var effectiveCoordinatorAssignments = _employee.CoordinatorAssignmentHistory.Effective(_asOfDate);
    //        if (effectiveCoordinatorAssignments.Count > 0)
    //        {
    //            var coordinatorAssignment = effectiveCoordinatorAssignments.First();
    //            managerViewModel.DateEffective = coordinatorAssignment.DateEffective.ToShortDateString();
    //            managerViewModel.Name = coordinatorAssignment.Coordinator.FirstName + " " + coordinatorAssignment.Coordinator.LastName;

    //            //always show current information for manager
    //            var coordinator = new TemporalEmployeeService(coordinatorAssignment.Coordinator, DateTime.Now);
    //            managerViewModel.ContactPreferences = coordinator.GetContactPreferences();
    //        }
    //        return managerViewModel;
    //    }

    //    public ContactPreferencesViewModel GetContactPreferences()
    //    {
    //        var contactPreferencesViewModel = new ContactPreferencesViewModel();
    //        var effectiveContactPreferences = _employee.ContactPreferencesHistory.Effective(_asOfDate);
    //        if (effectiveContactPreferences.Count > 0)
    //        {
    //            var contactPreferences = effectiveContactPreferences.First();
    //            contactPreferencesViewModel.Phone1 = contactPreferences.Phone1;
    //            contactPreferencesViewModel.Phone2 = contactPreferences.Phone2;
    //            contactPreferencesViewModel.Email1 = contactPreferences.Email1;
    //            contactPreferencesViewModel.Email2 = contactPreferences.Email2;
    //            contactPreferencesViewModel.Extension = contactPreferences.Extension;
    //            contactPreferencesViewModel.DateEffective = contactPreferences.DateEffective.ToString();
    //        }
    //        return contactPreferencesViewModel;
    //    }

    //    public EmployeeDetailsViewModel GetEmployeeDetails()
    //    {
    //        var employeeDetailsViewModel = new EmployeeDetailsViewModel();
    //        MapEmployeeDetailsType(employeeDetailsViewModel);
    //        return employeeDetailsViewModel;
    //    }

    //    public ExtendedEmployeeDetailsViewModel GetExtendedEmployeeDetails()
    //    {
    //        var extendedEmployeeDetailsViewModel = new ExtendedEmployeeDetailsViewModel();
    //        MapEmployeeDetailsType(extendedEmployeeDetailsViewModel);
    //        if (_employee.PositionAssignmentHistory.Count > 0)
    //        {
    //            extendedEmployeeDetailsViewModel.StartDate = _employee.PositionAssignmentHistory.OrderBy(e => e.DateStarted).First().DateStarted?.ToShortDateString();
    //        }
    //        return extendedEmployeeDetailsViewModel;
    //    }

    //    private void MapEmployeeDetailsType(EmployeeDetailsViewModel employeeDetailsViewModel)
    //    {
    //        employeeDetailsViewModel.FirstName = _employee.FirstName;
    //        employeeDetailsViewModel.LastName = _employee.LastName;
    //        employeeDetailsViewModel.Username = _employee.Username;
    //        employeeDetailsViewModel.Adp = _employee.Adp;

    //        employeeDetailsViewModel.Supervisor = GetSupervisor();
    //        employeeDetailsViewModel.Coordinator = GetCoordinator();
    //        employeeDetailsViewModel.ContactPreferences = GetContactPreferences();
    //        employeeDetailsViewModel.Positions = GetEffectivePositions();
    //    }
    //}
}