using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Healthprofessionaltype
{
    public int Healthprofessionalid { get; set; }

    public string Professionname { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public BitArray? Isactive { get; set; }

    public BitArray? Isdeleted { get; set; }

    public virtual ICollection<Healthprofessional> Healthprofessionals { get; set; } = new List<Healthprofessional>();
}
