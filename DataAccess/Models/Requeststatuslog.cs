using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Requeststatuslog
{
    public int Requeststatuslogid { get; set; }

    public int Requestid { get; set; }

    public short Status { get; set; }

    public int? Physicianid { get; set; }

    public int? Adminid { get; set; }

    public int? Transtophysicianid { get; set; }

    public string? Notes { get; set; }

    public DateTime Createddate { get; set; }

    public string? Ip { get; set; }

    public BitArray? Transtoadmin { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Physician? Physician { get; set; }

    public virtual Request Request { get; set; } = null!;

    public virtual ICollection<Requestclosed> Requestcloseds { get; set; } = new List<Requestclosed>();

    public virtual Physician? Transtophysician { get; set; }
}
