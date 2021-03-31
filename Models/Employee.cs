using System;
using System.Collections.Generic;
using System.Linq;



namespace CIS.HR.Models
{
    public class EmployeeConfig : EntityConfig<Employee>
    {
        public EmployeeConfig()
        {
            //this.HasOptional<TemporalCollection<CoordinatorAssignment>>(e => e.CoordinatorAssignmentHistory)
            //    .WithMany()
            //    .HasForeignKey(e => e.Id)
            //    .WillCascadeOnDelete(true);
        }
    }


    public class Employee : Entity
    {
        #region fields
        public string Uid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adp { get; set; }
        public string Username { get; set; }
        #endregion

        #region legacyFields
        public int? LegacyId { get; set; }

        //determine if these should be temporal attributes i.e. RoleTypeId
        public bool IsSupervisor { get; set; }
        public bool IsCoordinator { get; set; }
        public bool IsDirector { get; set; }
        public bool IsLeader { get; set; }
        #endregion

        #region collections
        public virtual TemporalCollection<PositionAssignment> PositionAssignmentHistory { get; set; }
        public virtual TemporalCollection<EmploymentStatusAssignment> EmploymentStatusAssignmentHistory { get; set; }
        public virtual TemporalCollection<SupervisorAssignment> SupervisorAssignmentHistory { get; set; }
        public virtual TemporalCollection<CoordinatorAssignment> CoordinatorAssignmentHistory { get; set; }
        public virtual TemporalCollection<ShiftTypeAssignment> ShiftTypeAssignmentHistory { get; set; }
        public virtual TemporalCollection<DepartmentAssignment> DepartmentAssignmentHistory { get; set; }
        public virtual TemporalCollection<Address> AddressHistory { get; set; }
        public virtual TemporalCollection<ContactPreferences> ContactPreferencesHistory { get; set; }
        #endregion

        //cctor
        public Employee()
        {
            PositionAssignmentHistory = new TemporalCollection<PositionAssignment>();
            EmploymentStatusAssignmentHistory = new TemporalCollection<EmploymentStatusAssignment>();
            SupervisorAssignmentHistory = new TemporalCollection<SupervisorAssignment>();
            CoordinatorAssignmentHistory = new TemporalCollection<CoordinatorAssignment>();
            ShiftTypeAssignmentHistory = new TemporalCollection<ShiftTypeAssignment>();
            DepartmentAssignmentHistory = new TemporalCollection<DepartmentAssignment>();
            AddressHistory = new TemporalCollection<Address>();
            ContactPreferencesHistory = new TemporalCollection<ContactPreferences>();
        }
    }

    /*
        The Controller calls the Service Layer which returns Entity Models.
        The Controller then creates and populates a View Model using the Entity Model data.
        Once the View Model is complete the Controller pass it on to the View.


        Controller method is called
        Controller uses repository(business logic, in other words) to get model data
        Controller converts the model data if necessary and creates a view model object
        Controller passes the view model to the view
        The view displays the data in the view model with limited logic to show or hide things, etc

        The model shouldn't leak into the view and neither should the services.
    */
}