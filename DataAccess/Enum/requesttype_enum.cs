using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Enums
{
    public enum RequestTypeEnum
    {
        Business = 1,
        Patient = 2,
        Family = 3,
        Concierge = 4
    }

    public enum ResponseStatus
    {
        Failed,
        Success
    }

    public enum AccountType
    {
        Admin = 1,
        Physician = 2,
        Patient = 3
    }

    public enum Status
    {
        Active = 1,
        Pending = 2,
        Inactive = 3
    }
}