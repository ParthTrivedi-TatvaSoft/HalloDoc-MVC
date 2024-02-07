using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Requestconcierge
{
    public int Id { get; set; }

    public int Requestid { get; set; }

    public int Conciergeid { get; set; }

    public string? Ip { get; set; }

    public virtual Concierge Concierge { get; set; } = null!;

    public virtual Request Request { get; set; } = null!;
}
