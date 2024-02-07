using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Physicianregion
{
    public int Physicianregionid { get; set; }

    public int Physicianid { get; set; }

    public int Regionid { get; set; }

    public virtual Physician Physician { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;
}
