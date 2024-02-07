using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Aspnetuserrole
{
    public string Userid { get; set; } = null!;

    public int Roleid { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual Aspnetuser User { get; set; } = null!;
}
