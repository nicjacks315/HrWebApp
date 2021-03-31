using CIS.HR.Models;

namespace CIS.HR.DAL
{
    public class Service
    {
        protected Context _context;

        public Service(Context context = null)
        {
            _context = context ?? new Context();
        }
    }
}
