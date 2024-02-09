using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Aspnetuser
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Passwordhash { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public string? Ip { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime? Modifieddate { get; set; }

    public virtual ICollection<Admin> AdminAspnetusers { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminCreatedbyNavigations { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminModifiedbyNavigations { get; set; } = new List<Admin>();

    public virtual ICollection<Aspnetuserrole> Aspnetuserroles { get; set; } = new List<Aspnetuserrole>();

    public virtual ICollection<Business> BusinessCreatedbyNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Business> BusinessModifiedbyNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Physician> PhysicianAspnetusers { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianCreatedbyNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianModifiedbyNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<Shiftdetail> Shiftdetails { get; set; } = new List<Shiftdetail>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
