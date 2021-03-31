namespace CIS.HR.Models
{
    public class Department : Entity
    {
        public string LegacyName { get; set; }
        public string DepartmentName { get; set; }
        public string Abbreviation { get; set; }
    }
}