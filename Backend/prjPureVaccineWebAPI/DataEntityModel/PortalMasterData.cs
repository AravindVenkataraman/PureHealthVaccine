using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntityModel
{
    /// <summary>
    /// Properties for Emirates in side the country
    /// </summary>
    public class countryEmirates
    {
        public int emirateID { get; set; }
        public string emirateName { get; set; }
    }

    /// <summary>
    /// Properties for appointment slots
    /// </summary>
    public class AppointmentSlots
    {
        public int slotID { get; set; }
        public string slotStartTime { get; set; }
        public string slotEndTime { get; set; }
        public int isBooked { get; set; }
    }

    /// <summary>
    /// Properties fpr User Details post validation
    /// </summary>
    public class validatedUser
    {
        public bool isValid { get; set; }
        public string fullName { get; set; }
        public bool isNurse { get; set; }
        public bool isDeo { get; set; }
        public bool isAdmin { get; set; }
        public bool hasWalkinAccess { get; set; }
        public bool hasPostVaccinationAccess { get; set; }
        public int lastLoginLocID { get; set; }
    }

    public class userLogin
    {
        public string userId { get; set; }
        public string password { get; set; }
    }

    /// <summary>
    /// Properties for vaccination Location
    /// </summary>
    public class vaccinationLocations
    {
        public int locationID { get; set; }
        public string locationDesc { get; set; }
        public string locationCity { get; set; }
        public string locationLatitude { get; set; }
        public string locationLongitude { get; set; }
    }

    /// <summary>
    /// Properties for list of locations
    /// </summary>
    public class Locations
    {
        public int locationID { get; set; }
        public string locationDesc { get; set; } 
    }
    /// <summary>
    /// Properties for Patient Communication i.e. Email or SMS
    /// </summary>
    public class patientCommunication
    {
        public long applicationNo { get; set; }
        public string userId { get; set; }
        public string commType { get; set; }
        public string emailAddress { get; set; }
        public string smsPhoneNumber { get; set; }
        public string msgSubject { get; set; }
        public string msgContent { get; set; } 
    }

    /// <summary>
    /// Properties for Success API Response on Data Save
    /// </summary>
    public class PureVaccineAPISuccessResponse
    {
        public bool isSuccess { get; set; }
        public long applicationNo { get; set; }
        public string message { get; set; }

    }

    /// <summary>
    /// Properties for UnSuccess API Response on Data Save
    /// </summary>
    public class PureVaccineAPIErrorResponse
    {
        public bool isSuccess { get; set; }
        public int errorCode { get; set; }
        public string errorDesc { get; set; }
        public string message { get; set; }

    }

    /// <summary>
    /// To Authenticate User and Validate OTP
    /// </summary>
    public class OTPReqquest
    { 
        public string emiratesId { get; set; }
        public string mobileNumber { get; set; }
        public string emailId { get; set; }

    }

    /// <summary>
    /// Reply OPT Response
    /// </summary>
    public class OTPResponse
    {
        public bool isSuccesss { get; set; }
        public bool hasOTPSent { get; set; }
        public string trnKey { get; set; }
        public bool firstDoseAdministered { get; set; }

    }

    /// <summary>
    /// Validate OTP Input
    /// </summary>
    public class validateOTPRequest
    {
        public string uniqueKey { get; set; } 
        public string OTP { get; set; } 
        public string eid { get; set; }

    }

    /// <summary>
    /// Validate EID Input
    /// </summary>
    public class validateEIDRequest
    {
        
        public string eid { get; set; }

    }

    /// <summary>
    /// Validate OTP Input
    /// </summary>
    public class validateOTPResposne
    {
        public bool isSuccess { get; set; }
        public bool isValid { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string nationality { get; set; }
        public string preferredLanguage { get; set; }
    }


    /// <summary>
    /// Validate EID Input
    /// </summary>
    public class validateEIDResposne
    {
        public bool isSuccess { get; set; }
        public bool isValid { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string nationality { get; set; }
        public string preferredLanguage { get; set; }
    }


    /// <summary>
    /// Demographics from Emirates ID Response
    /// </summary>
    public class patientDemographics
    {
        public string name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string nationality { get; set; }
        public string preferredLanguage { get; set; }

    }
}
