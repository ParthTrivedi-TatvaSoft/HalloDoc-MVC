using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Weeklytimesheetdetail
{
    public int TimeSheetDetailId { get; set; }

    public DateOnly Date { get; set; }

    public int? NumberOfShifts { get; set; }

    public int? NightShiftWeekend { get; set; }

    public int? HouseCall { get; set; }

    public int? HouseCallNightWeekend { get; set; }

    public int? PhoneConsult { get; set; }

    public int? PhoneConsultNightWeekend { get; set; }

    public int? BatchTesting { get; set; }

    public string? Item { get; set; }

    public int? Amount { get; set; }

    public string? Bill { get; set; }

    public int? TotalAmount { get; set; }

    public int? BonusAmount { get; set; }

    public int? TimeSheetId { get; set; }

    public int? OnCallHours { get; set; }

    public int? TotalHours { get; set; }

    public bool? IsWeekendHoliday { get; set; }

    public virtual Weeklytimesheet? TimeSheet { get; set; }
}
