namespace CIS.HR.Models
{
    public class ShiftTypeAssignment : TemporalEntity
    {
        public int EmployeeId { get; set; }
        public int ShiftTypeId { get; set; }
        public string Uid { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ShiftType ShiftType { get; set; }
    }

    public class ShiftTypeAssignmentConfig : EntityConfig<ShiftTypeAssignment>
    {
        public ShiftTypeAssignmentConfig()
        {
            this.HasRequired<ShiftType>(e => e.ShiftType)
                .WithMany()
                .HasForeignKey<int>(k => k.ShiftTypeId)
                .WillCascadeOnDelete(false);

            this.HasRequired<Employee>(e => e.Employee)
                .WithMany(e => e.ShiftTypeAssignmentHistory)
                .HasForeignKey<int>(k => k.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }
}