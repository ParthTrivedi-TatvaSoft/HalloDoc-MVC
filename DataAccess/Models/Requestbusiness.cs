using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Requestbusiness
{
    public int Requestbusinessid { get; set; }

    public int Requestid { get; set; }

    public int Businessid { get; set; }

    public string? Ip { get; set; }

    public virtual Business Business { get; set; } = null!;

    public virtual Request Request { get; set; } = null!;
}
