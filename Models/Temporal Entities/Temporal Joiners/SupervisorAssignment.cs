using System;

namespace CIS.HR.Models
{
    public class SupervisorAssignment : TemporalEntity
    {
        public int SupervisorId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Supervisor { get; set; }
    }

    public class SupervisorAssignmentConfig : EntityConfig<SupervisorAssignment>
    {
        public SupervisorAssignmentConfig()
        {
            this.HasRequired<Employee>(e => e.Supervisor)
                .WithMany()
                .HasForeignKey<int>(k => k.SupervisorId)
                .WillCascadeOnDelete(false);

            this.HasRequired<Employee>(e => e.Employee)
                .WithMany(e => e.SupervisorAssignmentHistory)
                .HasForeignKey<int>(k => k.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }
} 