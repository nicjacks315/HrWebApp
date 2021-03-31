using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CIS.HR.Models
{
    public class Position : Entity
    {
        public string Code { get; set; }
        //public virtual ICollection<PositionDescription> PositionDescriptionHistory { get; set; }
        public virtual TemporalCollection<PositionDescription> PositionDescriptionHistory { get; set; }

        //cctor
        public Position()
        {
            //PositionDescriptionHistory = new HashSet<PositionDescription>();
            PositionDescriptionHistory = new TemporalCollection<PositionDescription>();
        }
    }
}
