using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Employee
{
    public int Employeeid { get; set; }

    public int Departmentid { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string? Age { get; set; }

    public string? Email { get; set; }

    public string? Education { get; set; }

    public string? Company { get; set; }

    public int? Experience { get; set; }

    public string? Package { get; set; }

    public virtual Department Department { get; set; } = null!;
}
