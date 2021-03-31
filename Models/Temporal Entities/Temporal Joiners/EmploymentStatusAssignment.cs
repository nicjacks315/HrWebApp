namespace CIS.HR.Models
{
    public class EmploymentStatusAssignment : TemporalEntity
    {
        public int EmployeeId { get; set; }
        public int EmploymentStatusId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual EmploymentStatus EmploymentStatus { get; set; }
    }

    public class EmploymentStatusAssignmentConfig : EntityConfig<EmploymentStatusAssignment>
    {
        public EmploymentStatusAssignmentConfig()
        {
            this.HasRequired<EmploymentStatus>(e => e.EmploymentStatus)
                .WithMany()
                .HasForeignKey<int>(k => k.EmploymentStatusId)
                .WillCascadeOnDelete(false);

            this.HasRequired<Employee>(e => e.Employee)
                .WithMany(e => e.EmploymentStatusAssignmentHistory)
                .HasForeignKey<int>(k => k.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }
}