using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Aspnetuserrole
{
    public int Aspnetuserroleid { get; set; }

    public string Userid { get; set; } = null!;

    public int Roleid { get; set; }

    public virtual Aspnetrole Role { get; set; } = null!;

    public virtual Aspnetuser User { get; set; } = null!;
}
