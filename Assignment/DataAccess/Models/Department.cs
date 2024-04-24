using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Department
{
    public int Departmentid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
