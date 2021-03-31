namespace CIS.HR.Models
{
    public class Benefits : Entity
    {
        public string Code { get; set; }
        public virtual TemporalCollection<BenefitsDescription> BenefitsDescriptionHistory { get; set; }
    }
}