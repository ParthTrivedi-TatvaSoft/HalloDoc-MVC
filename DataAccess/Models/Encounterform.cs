using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Encounterform
{
    public int Id { get; set; }

    public int Requestid { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string? Location { get; set; }

    public string? Strmonth { get; set; }

    public int? Intyear { get; set; }

    public int? Intdate { get; set; }

    public DateTime? Servicedate { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public string? Illnesshistory { get; set; }

    public string? Medicalhistory { get; set; }

    public string? Medications { get; set; }

    public string? Allergies { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Heartrate { get; set; }

    public decimal? Respirationrate { get; set; }

    public int? Bloodpressuresystolic { get; set; }

    public int? Bloodpressurediastolic { get; set; }

    public decimal? Oxygenlevel { get; set; }

    public string? Pain { get; set; }

    public string? Heent { get; set; }

    public string? Cardiovascular { get; set; }

    public string? Chest { get; set; }

    public string? Abdomen { get; set; }

    public string? Extremities { get; set; }

    public string? Skin { get; set; }

    public string? Neuro { get; set; }

    public string? Other { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatmentplan { get; set; }

    public string? Medicationsdispensed { get; set; }

    public string? Procedures { get; set; }

    public string? Followup { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime? Modifieddate { get; set; }

    public bool? Isfinalized { get; set; }

    public DateTime? Finalizeddate { get; set; }

    public virtual Request Request { get; set; } = null!;
}
