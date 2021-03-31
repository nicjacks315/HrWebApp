using System;

namespace CIS.HR.Models
{
    public class CoordinatorAssignment : TemporalEntity
    {
        public int CoordinatorId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Coordinator { get; set; }
    }

    public class CoordinatorAssignmentConfig : EntityConfig<CoordinatorAssignment>
    {
        public CoordinatorAssignmentConfig()
        {
            this.HasRequired<Employee>(e => e.Coordinator)
                .WithMany()
                .HasForeignKey<int>(k => k.CoordinatorId)
                .WillCascadeOnDelete(false);

            this.HasRequired<Employee>(e => e.Employee)
                .WithMany(e => e.CoordinatorAssignmentHistory)
                .HasForeignKey<int>(k => k.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }
}