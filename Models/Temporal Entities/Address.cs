using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS.HR.Models
{
    public class Address : TemporalEntity
    {
        public int EmployeeId { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        virtual public Employee Employee { get; set; }
    }

    public class AddressConfig : EntityConfig<Address>
    {
        public AddressConfig()
        {
            this.HasRequired<Employee>(e => e.Employee)
                .WithMany(e => e.AddressHistory)
                .HasForeignKey<int>(k => k.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }
}