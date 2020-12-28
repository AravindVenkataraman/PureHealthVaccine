using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntityModel
{
    /// <summary>
    /// Properties for vaccination application to save the application in db
    /// </summary>
    public class vaccinationApplications
    {
        public long _applicationNo { get; set; }
        public string fullName { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string mobileNumber { get; set; }
        public string eid { get; set; }
        public string passportNumber { get; set; }
        public string nationality { get; set; }
        public string residentEmirate { get; set; }
        public string emailID { get; set; }
        public string prefLanguage { get; set; }
        public string uaeAddress { get; set; }
        public bool hadCovid19 { get; set; }
        public bool covid19AntiBodies { get; set; }
        public bool covid19Vaccinated { get; set; }
        public bool chronicDisease { get; set; }
        public bool majorOperationSurgery { get; set; }
        public bool coronarySyndromeStroke { get; set; }
        public bool isPregnantOrPlanning { get; set; }
        public int _firstDoseLocId { get; set; }
        public string firstDoseDate { get; set; }
        public long firstDoseApptSlotID { get; set; }
        public int _secondDoseLocId { get; set; }
        public string secondDoseDate { get; set; }
        public long secondDoseApptSlotID { get; set; }
        public bool _vaccConsent { get; set; }
        public string updatedDT { get; set; }
        public int _appStatus { get; set; }
        public string otpText { get; set; }
        public bool _firstDoseCompleted { get; set; }
        public string fFirstDoseCompletedDT { get; set; }
        public bool _secondDoseCompleted { get; set; }
        public string secondDoseCompletedDT { get; set; }
       
    }

    /// <summary>
    /// request parameters for searching an applictaion in DB
    /// </summary>
    public class applicationDetailsRequest
    {
        public long applicationNo { get; set; }
        public string eid { get; set; }
    }

    /// <summary>
    /// Response for no application found 
    /// </summary>
    public class applicationNotFound
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } 
    }
    /// <summary>
    /// vaccination application details by emirates id or application number
    /// </summary>
    public class applicationDetailsResponse
    {
        public bool isSuccess { get; set; }
        public long applicationNo { get; set; }
        public string fullName { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string mobileNumber { get; set; }
        public string eid { get; set; }
        public string passportNumber { get; set; }
        public bool hadCovid19 { get; set; }
        public bool covid19AntiBodies { get; set; }
        public bool covid19Vaccinated { get; set; }
        public bool chronicDisease { get; set; }
        public bool majorOperationSurg { get; set; }
        public bool coronarySyndromeStroke { get; set; }
        public bool isPregnant { get; set; }
        public string firstDoseLocation { get; set; }
        public string firstApptDate { get; set; }
        public string firstApptStartTime { get; set; }
        public string firstApptEndTime { get; set; }
        public string secondDoseLocation { get; set; }
        public string secondApptDate { get; set; }
        public string secondApptStartTime { get; set; }
        public string secondApptEndTime { get; set; }
        public bool isIDVerifiedByDEO { get; set; }
        public string DEOUserID { get; set; }
        public string DEOVerificationDT { get; set; }
    }

}
