using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Adminregion
{
    public int Adminregionid { get; set; }

    public int Adminid { get; set; }

    public int Regionid { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;
}
