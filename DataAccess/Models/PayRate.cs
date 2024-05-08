using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Payrate
{
    public int PayRateId { get; set; }

    public int PhysicianId { get; set; }

    public int? NightShiftWeekend { get; set; }

    public int? Shift { get; set; }

    public int? HouseCallNightWeekend { get; set; }

    public int? PhoneConsult { get; set; }

    public int? PhoneConsultNightWeekend { get; set; }

    public int? BatchTesting { get; set; }

    public int? HouseCall { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Physician Physician { get; set; } = null!;

    public virtual ICollection<Weeklytimesheet> Weeklytimesheets { get; set; } = new List<Weeklytimesheet>();
}
