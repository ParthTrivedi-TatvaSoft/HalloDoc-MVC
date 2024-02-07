using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Healthprofessional
{
    public int Vendorid { get; set; }

    public string Vendorname { get; set; } = null!;

    public int? Profession { get; set; }

    public string Faxnumber { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zip { get; set; }

    public int? Regionid { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime? Modifieddate { get; set; }

    public string? Phonenumber { get; set; }

    public BitArray? Isdeleted { get; set; }

    public string? Ip { get; set; }

    public string? Email { get; set; }

    public string? Businesscontact { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual Healthprofessionaltype? ProfessionNavigation { get; set; }

    public virtual Region? Region { get; set; }
}
