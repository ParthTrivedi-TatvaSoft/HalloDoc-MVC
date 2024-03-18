using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Business
{
    public int Businessid { get; set; }

    public string Name { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? Regionid { get; set; }

    public string? Zipcode { get; set; }

    public string? Phonenumber { get; set; }

    public string? Faxnumber { get; set; }

    public BitArray? Isregistered { get; set; }

    public string? Createdby { get; set; }

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public short? Status { get; set; }

    public BitArray? Isdeleted { get; set; }

    public string? Ip { get; set; }

    public virtual Aspnetuser? CreatedbyNavigation { get; set; }

    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    public virtual Region? Region { get; set; }

    public virtual ICollection<Requestbusiness> Requestbusinesses { get; set; } = new List<Requestbusiness>();
}
