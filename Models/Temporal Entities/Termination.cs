using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS.HR.Models
{
    public class Termination : TemporalEntity
    {
        public bool IsRehireEligible { get; set; }
        public TerminationType TerminationType { get; set; }
        public DateTime LastDay { get; set; }

        //public int EmployeeId { get; set; }
        //public virtual Employee Employee { get; set; }
    }
}