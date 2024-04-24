

using DataAccess.CustomModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IEMService
    {
        List<Records> EmsRecords();
        bool DeleteRecord(int employeeid);

        Records AddEmployees(Records model);

        Records EditEmp(int employeeid);
    }
}
