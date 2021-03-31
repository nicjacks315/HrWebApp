namespace CIS.HR.Models
{
    public class DepartmentAssignment : TemporalEntity
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public string Uid { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Department Department { get; set; }
    }

    public class DepartmentAssignmentConfig : EntityConfig<DepartmentAssignment>
    {
        public DepartmentAssignmentConfig()
        {
            this.HasRequired<Department>(e => e.Department)
                .WithMany()
                .HasForeignKey<int>(k => k.DepartmentId)
                .WillCascadeOnDelete(false);

            this.HasRequired<Employee>(e => e.Employee)
                .WithMany(e => e.DepartmentAssignmentHistory)
                .HasForeignKey<int>(k => k.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }
}