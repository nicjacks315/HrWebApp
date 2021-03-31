using System;
using System.Collections.Generic;
using System.Linq;

namespace CIS.HR.Models
{
    public class TemporalEntity : Entity
    {
        public DateTime DateEffective { get; set; }

        //public DateTime DateEffectiveFrom { get; set; }
        //public DateTime DateEffectiveTo { get; set; }
    }
}