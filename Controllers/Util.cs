using System;
using System.Linq;
using CIS.HR.Models;

namespace CIS.HR.Controllers
{
    static public class Util
    {
        static public string GetSortParameter(string paramField, string sortField, string sortOrder)
        {
            return sortField == paramField ? sortOrder == "ascending" ? "descending" : "ascending" : "ascending";
        }

        static public DateTime? GetLastSync( Context db )
        {
            return db.Database.SqlQuery<DateTime>("SELECT LastSync FROM dbo.DatabaseInfo").ToList().FirstOrDefault();
        }

        static public bool GetReadOnlyState( Context db )
        {
            return db.Database.SqlQuery<bool>("SELECT IsReadOnly FROM dbo.DatabaseInfo").ToList().FirstOrDefault();
        }

        static public string GetUserFullName(string domain, string userName)
        {
            System.Security.Principal.WindowsIdentity wi = System.Security.Principal.WindowsIdentity.GetCurrent();
            string[] a = System.Web.HttpContext.Current.User.Identity.Name.Split('\\');

            System.DirectoryServices.DirectoryEntry ADEntry = new System.DirectoryServices.DirectoryEntry("WinNT://" + a[0] + "/" + a[1]);
            string Name = ADEntry.Properties["FullName"].Value.ToString();

            return Name;
        }
    }
}