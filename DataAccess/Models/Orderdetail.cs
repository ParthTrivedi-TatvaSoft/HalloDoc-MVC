using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Orderdetail
{
    public int Id { get; set; }

    public int? Vendorid { get; set; }

    public int? Requestid { get; set; }

    public string? Faxnumber { get; set; }

    public string? Email { get; set; }

    public string? Businesscontact { get; set; }

    public string? Prescription { get; set; }

    public int? Noofrefill { get; set; }

    public DateTime? Createddate { get; set; }

    public string? Createdby { get; set; }

    public virtual Request? Request { get; set; }

    public virtual Healthprofessional? Vendor { get; set; }
}
