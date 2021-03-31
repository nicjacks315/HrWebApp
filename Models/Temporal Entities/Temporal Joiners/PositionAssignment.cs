using System;

namespace CIS.HR.Models
{
    public class PositionAssignment : TemporalEntity
    {
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string Uid { get; set; }
        public DateTime? DateAsPrimary { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateExited { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Position Position { get; set; }
    }

    public class PositionAssignmentConfig : EntityConfig<PositionAssignment>
    {
        public PositionAssignmentConfig()
        {
            this.HasRequired<Position>(e => e.Position)
                .WithMany()
                .HasForeignKey<int>(k => k.PositionId)
                .WillCascadeOnDelete(false);

            this.HasRequired<Employee>(e => e.Employee)
                .WithMany(e => e.PositionAssignmentHistory)
                .HasForeignKey<int>(k => k.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }
}