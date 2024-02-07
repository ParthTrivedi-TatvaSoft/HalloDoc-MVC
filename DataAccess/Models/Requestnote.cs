using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Requestnote
{
    public int Requestnotesid { get; set; }

    public int Requestid { get; set; }

    public string? Strmonth { get; set; }

    public int? Intyear { get; set; }

    public int? Intdate { get; set; }

    public string? Physiciannotes { get; set; }

    public string? Adminnotes { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public string? Ip { get; set; }

    public string? Administrativenotes { get; set; }

    public virtual Request Request { get; set; } = null!;
}
