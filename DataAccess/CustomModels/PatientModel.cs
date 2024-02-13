using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.CustomModels
{
    public class PatientInfoModel
    {
        public string symptoms { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dob { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public string roomNo { get; set; }
        public string country { get; set; }


    }

    public class FamilyReqModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string relation { get; set; }
        public string symptoms { get; set; }
        public string patientFirstName { get; set; }
        public string patientLastName { get; set; }
        public DateTime patientDob { get; set; }
        public string patientEmail { get; set; }
        public string patientPhoneNo { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public int roomNo { get; set; }
    }

    public class ConciergeReqModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string hotelName { get; set; }
        public string symptoms { get; set; }
        public string patientFirstName { get; set; }
        public string patientLastName { get; set; }
        public DateTime patientDob { get; set; }
        public string patientEmail { get; set; }
        public string patientPhoneNo { get; set; }

        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public int roomNo { get; set; }


    }

    public class BusinessReqModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string businessName { get; set; }
        public string caseNo { get; set; }
        public string symptoms { get; set; }
        public string patientFirstName { get; set; }
        public string patientLastName { get; set; }
        public DateTime patientDob { get; set; }
        public string patientEmail { get; set; }
        public string patientPhoneNo { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public int roomNo { get; set; }
    }


    public class PatientDashboard
    {
        public DateTime createdDate { get; set; }
        public string currentStatus { get; set; }
        public string document { get; set; }
    }

    public class PatientDashboardInfo
    {
        public List<PatientDashboard> patientDashboardItems { get; set; }
    }



}
