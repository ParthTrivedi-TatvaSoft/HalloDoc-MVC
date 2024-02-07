using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Shiftdetailregion
{
    public int Shiftdetailregionid { get; set; }

    public int Shiftdetailid { get; set; }

    public int Regionid { get; set; }

    public BitArray? Isdeleted { get; set; }

    public virtual Region Region { get; set; } = null!;

    public virtual Shiftdetail Shiftdetail { get; set; } = null!;
}
