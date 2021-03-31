using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;


namespace CIS.HR.Models
{
    public class Context : DbContext
    {
        #region cctors
        static Context()
        {
            _readOnly = true;
            Database.SetInitializer( new MigrateDatabaseToLatestVersion<CIS.HR.Models.Context, CIS.HR.Migrations.Configuration>() );
            //Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context>());
        }

        public Context()
            //: base("Server=SQL-02;Initial Catalog=TemporalTest;Integrated Security=True;") //<!----- connection string is defined web.config
            //: base("Server=SQL-02;Database=TemporalTest;Integrated Security=True;")
        {
        }
        #endregion

        #region methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new EmployeeConfig());
            modelBuilder.Configurations.Add(new SupervisorAssignmentConfig());
            modelBuilder.Configurations.Add(new CoordinatorAssignmentConfig());
            modelBuilder.Configurations.Add(new EmploymentStatusAssignmentConfig());
            modelBuilder.Configurations.Add(new PositionAssignmentConfig());
            modelBuilder.Configurations.Add(new ShiftTypeAssignmentConfig());
            modelBuilder.Configurations.Add(new DepartmentAssignmentConfig());
            modelBuilder.Configurations.Add(new AddressConfig());
            modelBuilder.Configurations.Add(new ContactPreferencesConfig());
            modelBuilder.Configurations.Add(new PositionDescriptionConfig());
        }

        static public bool IsReadOnly()
        {
            return _readOnly;
        }
        #endregion

        #region domain object repositories
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<PositionAssignment> PositionAssignments { get; set; }
        public DbSet<PositionDescription> PositionDescriptions { get; set; }
        public DbSet<PositionClassification> PositionClassifications { get; set; }

        public DbSet<ContactPreferences> ContactPreferences { get; set; }
        public DbSet<EmploymentStatusAssignment> EmploymentStatusAssignments { get; set; }
        public DbSet<SupervisorAssignment> SupervisorAssignments { get; set; }
        public DbSet<CoordinatorAssignment> CoordinatorAssignments { get; set; }
        public DbSet<DepartmentAssignment> DepartmentAssignments { get; set; }
        public DbSet<ShiftTypeAssignment> ShiftTypeAssignments { get; set; }
        #endregion

        #region fields
        private static bool _readOnly;
        #endregion
    }
}