using System;

namespace CIS.HR.Models
{
    public class DatabaseInfo
    {
        public DateTime LastSync { get; set; }
        public bool IsReadOnly { get; set; }
    }
}