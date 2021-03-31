using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace CIS.HR.Models
{
    public class Entity
    {
        public int Id { get; set; }

        //public DateTime DateCreated { get; set; }
        //public DateTime DateModified { get; set; }
    }

    public abstract class EntityConfig<T> : EntityTypeConfiguration<T> where T : Entity
    {
        protected EntityConfig()
        {
            this.HasKey<int>(e => e.Id)
                .Property<int>(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
 