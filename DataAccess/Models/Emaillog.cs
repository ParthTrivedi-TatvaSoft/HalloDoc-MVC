using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Emaillog
{
    public int Emaillogid { get; set; }

    public string Emailtemplate { get; set; } = null!;

    public string Subjectname { get; set; } = null!;

    public string Emailid { get; set; } = null!;

    public string? Confirmationnumber { get; set; }

    public string? Filepath { get; set; }

    public int? Roleid { get; set; }

    public int? Requestid { get; set; }

    public int? Adminid { get; set; }

    public int? Physicianid { get; set; }

    public DateTime Createdate { get; set; }

    public DateTime? Sentdate { get; set; }

    public BitArray? Isemailsent { get; set; }

    public int? Senttries { get; set; }

    public int? Action { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Physician? Physician { get; set; }

    public virtual Request? Request { get; set; }
}
