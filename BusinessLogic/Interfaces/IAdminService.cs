
using DataAccess.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAdminService
    {
        List<AdminDashTableModel> GetRequestsByStatus();

        ViewCaseViewModel ViewCase(int reqClientId);
    }
}