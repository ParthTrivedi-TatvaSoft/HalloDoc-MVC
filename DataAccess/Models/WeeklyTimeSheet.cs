using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Weeklytimesheet
{
    public int TimeSheetId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int ProviderId { get; set; }

    public int? PayRateId { get; set; }

    public int? AdminId { get; set; }

    public bool? IsFinalized { get; set; }

    public string? AdminNote { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Payrate? PayRate { get; set; }

    public virtual Physician Provider { get; set; } = null!;

    public virtual ICollection<Weeklytimesheetdetail> Weeklytimesheetdetails { get; set; } = new List<Weeklytimesheetdetail>();
}
