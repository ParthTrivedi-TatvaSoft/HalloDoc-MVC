using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Requestclient
{
    public int Requestclientid { get; set; }

    public int Requestid { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string? Phonenumber { get; set; }

    public string? Location { get; set; }

    public string? Address { get; set; }

    public int? Regionid { get; set; }

    public string? Notimobile { get; set; }

    public string? Notiemail { get; set; }

    public string? Notes { get; set; }

    public string? Email { get; set; }

    public string? Strmonth { get; set; }

    public int? Intyear { get; set; }

    public int? Intdate { get; set; }

    public BitArray? Ismobile { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zipcode { get; set; }

    public short? Communicationtype { get; set; }

    public short? Remindreservationcount { get; set; }

    public short? Remindhousecallcount { get; set; }

    public short? Issetfollowupsent { get; set; }

    public string? Ip { get; set; }

    public short? Isreservationremindersent { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public virtual Region? Region { get; set; }

    public virtual Request Request { get; set; } = null!;
}
