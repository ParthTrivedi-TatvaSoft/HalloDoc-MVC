using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Rolemenu
{
    public int Rolemenuid { get; set; }

    public int Roleid { get; set; }

    public int Menuid { get; set; }

    public virtual Menu Menu { get; set; } = null!;
}
