using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Requestwisefile
{
    public int Requestwisefileid { get; set; }

    public int Requestid { get; set; }

    public string Filename { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public int? Physicianid { get; set; }

    public int? Adminid { get; set; }

    public short? Doctype { get; set; }

    public BitArray? Isfrontside { get; set; }

    public BitArray? Iscompensation { get; set; }

    public string? Ip { get; set; }

    public BitArray? Isfinalize { get; set; }

    public BitArray? Isdeleted { get; set; }

    public BitArray? Ispatientrecords { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Physician? Physician { get; set; }

    public virtual Request Request { get; set; } = null!;
}
