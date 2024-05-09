using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{

    public class InvoicingViewModel
    {
        public List<DateViewModel>? dates { get; set; }

        public DateOnly startDate { get; set; }

        public DateOnly endDate { get; set; }

        public int differentDays { get; set; }

        public List<Timesheet> timesheets { get; set; } = new List<Timesheet>();

        public List<PhysicianMain>? Physicians { get; set; }

        public List<PhysicianViewModel>? PhysiciansList { get; set; }

        public int PhysicianId { get; set; }

        public int TimeSheetId { get; set; }

        public bool IsFinalized { get; set; }

        public Pager? pager { get; set; }

        public int SkipCount { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public bool isAdminSide { get; set; }

        public string? Status { get; set; }

        public string? AdminNotes { get; set; }

        public int? BonusAmount { get; set; }

        public int shiftPayrate { get; set; }

        public int weekendPayrate { get; set; }

        public int HouseCallPayrate { get; set; }

        public int phoneConsultPayrate { get; set; }

        public int shiftTotal { get; set; }

        public int weekendTotal { get; set; }

        public int HouseCallTotal { get; set; }

        public int phoneconsultTotal { get; set; }

        public int GrandTotal { get; set; }

    }
    public class Timesheet
    {
        public int TotalHours { get; set; } = 0;

        public bool Weekend { get; set; } = false;

        public int NumberOfHouseCall { get; set; } = 0;

        public int NumberOfPhoneConsults { get; set; } = 0;

        public DateOnly Date { get; set; }

        public int OnCallhours { get; set; } = 0;

        public string? Medicine { get; set; }

        public int Amount { get; set; }

        public IFormFile? Bill { get; set; }

        public string? Items { get; set; }

        public int NumberofShift { get; set; } = 0;

        public int NightShiftWeekend { get; set; } = 0;

        public int HousecallNightsWeekend { get; set; } = 0;

        public int phoneConsultNightsWeekend { get; set; } = 0;

        public int BatchTesting { get; set; } = 0;

        public string? BillName { get; set; }

        public int WeeklyTimesheetDeatilsId { get; set; }

    }

    public class DateViewModel
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }

    public class PhysicianMain
    {
        public string PhyName { get; set; }
        public int PhyId { get; set; }
    }

    public class WeeklyTimeSheet
    {
        [Key]
        public int TimeSheetId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? CreatedDate { get; set; }

        public int ProviderId { get; set; }

        public int? PayRateId { get; set; }

        public int? AdminId { get; set; }

        public bool? IsFinalized { get; set; }

        [Column(TypeName = "character varying")]
        public string? AdminNote { get; set; }

        public int? Bonusamount { get; set; }

        [ForeignKey("AdminId")]
        [InverseProperty("WeeklyTimeSheets")]
        public virtual Admin? Admin { get; set; }

        //[ForeignKey("PayRateId")]
        //[InverseProperty("WeeklyTimeSheets")]
        //public virtual PayRate? PayRate { get; set; }

        [ForeignKey("ProviderId")]
        [InverseProperty("WeeklyTimeSheets")]
        public virtual Physician Provider { get; set; } = null!;

        [InverseProperty("TimeSheet")]
        public virtual ICollection<WeeklyTimeSheetDetail> WeeklyTimeSheetDetails { get; set; } = new List<WeeklyTimeSheetDetail>();
    }

    public class WeeklyTimeSheetDetail
    {
        [Key]
        public int TimeSheetDetailId { get; set; }

        public DateOnly Date { get; set; }

        public int? NumberOfShifts { get; set; }

        public int? NightShiftWeekend { get; set; }

        public int? HouseCall { get; set; }

        public int? HouseCallNightWeekend { get; set; }

        public int? PhoneConsult { get; set; }

        public int? PhoneConsultNightWeekend { get; set; }

        public int? BatchTesting { get; set; }

        [StringLength(100)]
        public string? Item { get; set; }

        public int? Amount { get; set; }

        [StringLength(100)]
        public string? Bill { get; set; }

        public int? TotalAmount { get; set; }

        public int? BonusAmount { get; set; }

        public int? TimeSheetId { get; set; }

        public int? OnCallHours { get; set; }

        public int? TotalHours { get; set; }

        public bool? IsWeekendHoliday { get; set; }

        [ForeignKey("TimeSheetId")]
        [InverseProperty("WeeklyTimeSheetDetails")]
        public virtual WeeklyTimeSheet? TimeSheet { get; set; }
    }

    public class Pager
    {
        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }

    }

    public class PhysicianViewModel
    {
        public int PhysicianId { get; set; }

        public string? PhysicianName { get; set; }

        public string? role { get; set; }

        public string? OnCallStatus { get; set; }

        public int region { get; set; }

        public bool IsNotificatonStopped { get; set; }

        public string? Email { get; set; }
    }

}