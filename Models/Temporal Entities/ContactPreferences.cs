using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS.HR.Models
{
    public class ContactPreferences : TemporalEntity
    {
        public int EmployeeId { get; set; }
        public string Extension { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }

        virtual public Employee Employee { get; set; }
    }

    public class ContactPreferencesConfig : EntityConfig<ContactPreferences>
    {
        public ContactPreferencesConfig()
        {
            this.HasRequired<Employee>(e => e.Employee)
                .WithMany(e => e.ContactPreferencesHistory)
                .HasForeignKey<int>(k => k.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }
}