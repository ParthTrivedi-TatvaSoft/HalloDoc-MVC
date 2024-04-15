using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Aspnetrole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Aspnetuserrole> Aspnetuserroles { get; set; } = new List<Aspnetuserrole>();
}
