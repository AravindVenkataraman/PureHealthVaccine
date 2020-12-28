using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DataEntityModel;
using Public.Library.ErrorHandeling;
using PureVaccineDAO;
using PatientMessenger;
using ICAIntegration;

namespace pureVaccineWebAPI.Controllers
{
    /// <summary>
    /// Enable cors for Limitng the Incoming Request Origin from know consumers only
    /// </summary> 
    /// 
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PureVaccineController : ApiController
    {
        /// <summary>
        /// api method for saving the vaccination form input by the user
        /// </summary>
        /// <param name="formInput">Input fields payload</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/saveVaccinationApplication")]
        public HttpResponseMessage saveVaccinationApplication([FromBody] VaccineApplicationFormInput formInput)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            vaccinationApplications application = new vaccinationApplications();
            try
            {
                application.fullName = formInput.fullName;
                application.dob = formInput.dob;
                application.gender = formInput.gender;
                application.mobileNumber = formInput.mobileNumber;
                application.eid = formInput.eid;
                application.passportNumber = formInput.passportNumber;
                application.nationality = formInput.nationality;
                application.residentEmirate = formInput.residentEmirate;
                application.emailID = formInput.email;
                application.prefLanguage = formInput.prefLanguage;
                application.uaeAddress = formInput.uaeAddress;
                application.hadCovid19 = formInput.hadCovid19;
                application.covid19AntiBodies = formInput.covid19AntiBodies;
                application.covid19Vaccinated = formInput.covid19Vaccinated;
                application.chronicDisease = formInput.chronicDisease;
                application.majorOperationSurgery = formInput.majorOperationSurgery;
                application.coronarySyndromeStroke = formInput.coronarySyndromeStroke;
                application.isPregnantOrPlanning = formInput.isPregnantOrPlanning;
                application._firstDoseLocId = formInput.firstDoseLocId;
                application.firstDoseDate = formInput.firstDoseDate;
                application.firstDoseApptSlotID = formInput.firstDoseApptSlotID;
                application._vaccConsent = formInput.vaccineConsent;

                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();
                object[] objResposne = vaccineDAO.savePatientvaccinationApplication(application);
                long _newApplicationNumber = (!Convert.IsDBNull(objResposne[0])) ? Convert.ToInt64(objResposne[0]) : 0;
                string msg = (!Convert.IsDBNull(objResposne[1])) ? objResposne[1].ToString() : "";
                string facilityName = (!Convert.IsDBNull(objResposne[2])) ? objResposne[2].ToString() : "";
                string facilityAddress = (!Convert.IsDBNull(objResposne[3])) ? objResposne[3].ToString() : "";
                string timeSlot = (!Convert.IsDBNull(objResposne[4])) ? objResposne[4].ToString() : "";

                if (_newApplicationNumber > 0)
                {
                    PureVaccineAPISuccessResponse successResponse = new PureVaccineAPISuccessResponse();
                    successResponse.isSuccess = true;
                    successResponse.applicationNo = _newApplicationNumber;
                    successResponse.message = "application saved successfully";
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, successResponse, "application/JSON");

                    string emailTemplates = vaccineDAO.getEmailSMSTemplates(1, "APP_SUCCESS");
                    if (!string.IsNullOrEmpty(emailTemplates) && !string.IsNullOrEmpty(formInput.email))
                    {
                        emailTemplates = emailTemplates.Replace("[Applicant_Name]", formInput.fullName.ToUpper())
                                                       .Replace("[Application_Id]", _newApplicationNumber.ToString())
                                                       .Replace("[Facility_Name]", facilityName)
                                                       .Replace("[Facility_Address]", facilityAddress)
                                                       .Replace("[Apointment_date]", formInput.firstDoseDate)
                                                       .Replace("[Time_Slot]", timeSlot);

                        EmailProperties properties = new EmailProperties();
                        properties.emailTo = formInput.email;
                        properties.emailToName = formInput.fullName.ToUpper();
                        properties.emailSubject = "Pure Vaccine Registration";
                        properties.emailAPIKey = "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";
                        properties.emailFrom = "vaccination@purehealth.ae";
                        properties.emailFromName = "Pure Vaccine";
                        properties.emailPlainTextContent = "";
                        properties.emailHTMLContent = emailTemplates;

                        EmailMessenger messenger = new EmailMessenger();
                        messenger.sendEMail(properties);

                        string strMobile = formInput.mobileNumber.Trim().Replace("+", "");
                        string firstDigint = strMobile.Substring(0, 1);
                        if (firstDigint == "0")
                        {
                            strMobile = "971" + strMobile.Substring(1, strMobile.Length - 1);
                        }

                        string smsTemplates = vaccineDAO.getEmailSMSTemplates(2, "APP_SMS");
                        smsTemplates = smsTemplates.Replace("[Applicant_Name]", formInput.fullName.ToUpper())
                                                       .Replace("[Application_Id]", _newApplicationNumber.ToString());
                        SMSProperties SMSproperties = new SMSProperties();
                        SMSproperties.smsAPIUserName = "purehealth";
                        SMSproperties.smsAPIUserPassword = "prh901";
                        SMSproperties.smsSenderID = "Pure Health";
                        SMSproperties.smsRecepient = strMobile;
                        SMSproperties.smsText = smsTemplates;

                        SMSMessenger smsSender = new SMSMessenger();
                        bool _sent = smsSender.sendSMS(SMSproperties);
                    }
                }
                else
                {
                    PureVaccineAPIErrorResponse errorResponse = new PureVaccineAPIErrorResponse();
                    errorResponse.isSuccess = false;
                    errorResponse.errorCode = -100;
                    errorResponse.errorDesc = msg;
                    errorResponse.message = "unable to save data";
                    responseMessage = Request.CreateResponse(HttpStatusCode.Conflict, errorResponse, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                application = null;
            }
            return responseMessage;
        }

        /// <summary>
        /// api to get List of all the vaccination location by city
        /// </summary>
        /// <param name="cityId">city id for which list of location to be needed</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/getVaccinationLocationForCity/{cityId}")]
        public HttpResponseMessage getVaccinationLocationForCity(int cityId)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();
                List<vaccinationLocations> lstVaccLocation = vaccineDAO.getVaccinationLocationsByCity(cityId);

                if (lstVaccLocation.Capacity > 0)
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, lstVaccLocation, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.NoContent, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }

        /// <summary>
        ///  api to get List of all the vaccination locations
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/getAllLocationsForVaccination")]
        public HttpResponseMessage getAllLocationsForVaccination()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();
                List<Locations> lstVaccLocation = vaccineDAO.getAllLocations();

                if (lstVaccLocation.Capacity > 0)
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, lstVaccLocation, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.NoContent, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }

        /// <summary>
        /// api to return list of all the appointment slots for a date and location along with status
        /// </summary>
        /// <param name="filters">filters payload supplied for fetching the appointment</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/getVaccinationAppointment")]
        public HttpResponseMessage getVaccinationAppointment([FromBody] AppointmentFilters filters)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();
                List<AppointmentSlots> lstAppointmentSlot = vaccineDAO.getLocationAppointmentSlotsByDate(filters);

                if (lstAppointmentSlot.Capacity > 0)
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, lstAppointmentSlot, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.NoContent, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }

        /// <summary>
        /// api to retrun list of emirates
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/getEmiratesList")]
        public HttpResponseMessage getEmiratesList()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();
                List<countryEmirates> lstCountryEmirates = vaccineDAO.getCountryEmiratesList();

                if (lstCountryEmirates.Capacity > 0)
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, lstCountryEmirates, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.NoContent, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }


        /// <summary>
        /// Valid user login details based on the user id and password supplied by the application
        /// </summary>
        /// <param name="login">user login details payload</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/validateUserLogin")]
        public HttpResponseMessage validateUserLogin([FromBody] userLogin login)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();
                validatedUser validUser = vaccineDAO.validateUserLogins(login.userId, login.password);

                if (validUser != null)
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, validUser, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }




        /// <summary>
        /// To Send OTP to the User
        /// </summary>
        /// <param name="requestOtp">OTP send request payload</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/sendOtpSMS")]
        public HttpResponseMessage sendOtpSMS([FromBody] OTPReqquest requestOtp)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();
                string smsTemplates = vaccineDAO.getEmailSMSTemplates(2, "OTP_SMS");

                if (requestOtp.emiratesId != null && requestOtp.emailId != null && requestOtp.mobileNumber != null && smsTemplates != null)
                {
                    Random generator = new Random();
                    String randomNumber = generator.Next(0, 1000000).ToString("D6");
                    smsTemplates = smsTemplates.Replace("[OTP]", randomNumber);

                    string strMobile = requestOtp.mobileNumber.Trim().Replace("+", "");
                    string firstDigint = strMobile.Substring(0, 1);
                    if (firstDigint == "0")
                    {
                        strMobile = "971" + strMobile.Substring(1, strMobile.Length - 1);
                    }


                    SMSProperties properties = new SMSProperties();
                    properties.smsAPIUserName = "purehealth";
                    properties.smsAPIUserPassword = "prh901";
                    properties.smsSenderID = "Pure Health";
                    properties.smsRecepient = strMobile;
                    properties.smsText = smsTemplates;

                    SMSMessenger messenger = new SMSMessenger();
                    bool _sent = messenger.sendSMS(properties);
                    object[] objResp = new object[2];
                    if (_sent)
                    {
                        objResp = vaccineDAO.saveOTPCommunicationDetails(requestOtp.emiratesId, strMobile, requestOtp.emailId, randomNumber);
                    }

                    OTPResponse response = new OTPResponse();
                    response.isSuccesss = true;
                    response.hasOTPSent = _sent;
                    response.trnKey = !Convert.IsDBNull(objResp[1]) ? objResp[1].ToString() : "";
                    response.firstDoseAdministered = !Convert.IsDBNull(objResp[0]) ? Convert.ToBoolean(objResp[0]) : false;
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, response, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }

        /// <summary>
        /// To Send OTP to the User
        /// </summary>
        /// <param name="requestOtpValidation">OTP validation payload</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/validateOTP")]
        public HttpResponseMessage validateOTP([FromBody] validateOTPRequest requestOtpValidation)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();

                if (requestOtpValidation.uniqueKey != null && requestOtpValidation.OTP != null)
                {
                    bool isValid = vaccineDAO.validateOTP(requestOtpValidation.uniqueKey, requestOtpValidation.OTP);

                    validateOTPResposne response = new validateOTPResposne();
                    response.isValid = isValid;
                    
                    if (isValid)
                    {
                        ICARequestHandler requestHandler = new ICARequestHandler();
                        ICAResponse personInfo = requestHandler.getPersonByEmiratesId(requestOtpValidation.eid);

                        if (!string.IsNullOrWhiteSpace(personInfo.idn))
                        {
                            response.isSuccess = true;
                            response.name = personInfo.nameEnglish;
                            response.dob = personInfo.birthDate;
                            response.gender = personInfo.gender;
                            response.age = personInfo.age;
                            response.nationality = personInfo.nationality;
                            response.preferredLanguage = personInfo.preferredLang;
                        }
                    }

                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, response, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }






        /// <summary>
        /// Get Patient Details using EID for Walkin Appoitments
        /// </summary>
        /// <param name="requestEIDValidation">EID validation payload</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/getPatientDetailsByEID")]
        public HttpResponseMessage getPatientDetailsByEID([FromBody] validateEIDRequest requestEIDValidation)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();

               
                if (!string.IsNullOrWhiteSpace(requestEIDValidation.eid) && requestEIDValidation.eid.Length == 15)
                {
                    ICARequestHandler requestHandler = new ICARequestHandler();
                    ICAResponse personInfo = requestHandler.getPersonByEmiratesId(requestEIDValidation.eid);
                    validateEIDResposne response = new validateEIDResposne();
                    if (!string.IsNullOrWhiteSpace(personInfo.idn))
                    {
                        response.isValid = true;
                        response.isSuccess = true;
                        response.name = personInfo.nameEnglish;
                        response.dob = personInfo.birthDate;
                        response.gender = personInfo.gender;
                        response.age = personInfo.age;
                        response.nationality = personInfo.nationality;
                        response.preferredLanguage = personInfo.preferredLang;
                    }
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, response, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }







        /// <summary>
        /// get vaccination application details
        /// </summary>
        /// <param name="request">application number payload to fetch the application details</param>
        /// <returns></returns>

        [HttpPost]
        [Route("~/api/getVaccinationApplicationDetails")]
        public HttpResponseMessage getVaccinationApplicationDetails([FromBody] applicationDetailsRequest request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();

                if (request.applicationNo > 0 || !string.IsNullOrWhiteSpace(request.eid))
                {
                    applicationDetailsResponse applicationResponse = vaccineDAO.getApplicationDetails(request);
                    if (applicationResponse != null)
                    {
                        applicationResponse.isSuccess = true;
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, applicationResponse, "application/JSON");
                    }
                    else
                    {
                        applicationNotFound applicationNot = new applicationNotFound();
                        applicationNot.isSuccess = false;
                        applicationNot.message = "application details not found";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, applicationNot, "application/JSON");
                    }
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }

        /// <summary>
        /// save patient nursing observation into db
        /// </summary>
        /// <param name="entry">nursing entry against the application</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/saveNursingObservationForPatient")]
        public HttpResponseMessage saveNursingObservationForPatient([FromBody] nursingEntry entry)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();

                if (entry != null)
                {
                    bool _saved = vaccineDAO.savePatientNursingObservations(entry);
                    if (_saved)
                    {
                        nursingEntryResponse entryResponse = new nursingEntryResponse();
                        entryResponse.isSuccess = true;
                        entryResponse.message = "observations saved successfully";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, entryResponse, "application/JSON");
                    }
                    else
                    {
                        nursingEntryResponse entryResponse = new nursingEntryResponse();
                        entryResponse.isSuccess = false;
                        entryResponse.message = "error while saving the entry";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, entryResponse, "application/JSON");
                    }
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }


        /// <summary>
        /// api to udpate DEO response for an application
        /// </summary>
        /// <param name="deoResponse"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/updateDEOResponseForApplication")]
        public HttpResponseMessage updateDEOResponseForApplication([FromBody] applicationUpdateRequestByDEO deoResponse)
        {
            bool _updated = false;
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();
                if (deoResponse.applicationNumber > 0)
                {
                    _updated = vaccineDAO.updateDEOResponseForApplication(deoResponse);
                }

                if (_updated)
                {
                    applicationUpdateResponseForDEO responseForDEO = new applicationUpdateResponseForDEO();
                    responseForDEO.isSuccess = _updated;
                    responseForDEO.message = "application status updated successfully";
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, responseForDEO, "application/JSON");
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }


        /// <summary>
        /// save patient adverse reaction observation into db
        /// </summary>
        /// <param name="entry">nursing entry against the application</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/saveAdverseReactionForPatient")]
        public HttpResponseMessage saveAdverseReactionForPatient([FromBody] adverseReactionEntry entry)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();

                if (entry != null)
                {
                    bool _saved = vaccineDAO.savePatientAdverseReaction(entry);
                    if (_saved)
                    {
                        adverseReactionEntryResponse entryResponse = new adverseReactionEntryResponse();
                        entryResponse.isSuccess = _saved;
                        entryResponse.message = "adverse reaction saved successfully";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, entryResponse, "application/JSON");
                    }
                    else
                    {
                        adverseReactionEntryResponse entryResponse = new adverseReactionEntryResponse();
                        entryResponse.isSuccess = _saved;
                        entryResponse.message = "error while saving the entry";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, entryResponse, "application/JSON");
                    }
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }

         
        /// <summary>
        /// get vaccination application details
        /// </summary>
        /// <param name="request">application number payload to fetch the application details</param>
        /// <returns></returns>

        [HttpPost]
        [Route("~/api/getAdverseReactionForPatient")]
        public HttpResponseMessage getAdverseReactionForPatient([FromBody] applicationDetailsRequest request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();

                if (request.applicationNo > 0 || !string.IsNullOrWhiteSpace(request.eid))
                {
                    List<adverserReactionResponse> applicationResponse = vaccineDAO.getAdverseReactionDetails(request);
                    if (applicationResponse.Count >0)
                    { 
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, applicationResponse, "application/JSON");
                    }
                    else
                    {
                        applicationNotFound applicationNot = new applicationNotFound();
                        applicationNot.isSuccess = false;
                        applicationNot.message = "application details not found";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, applicationNot, "application/JSON");
                    }
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }


        /// <summary>
        /// get vaccination application details
        /// </summary>
        /// <param name="request">application number payload to fetch the application details</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/getPatientNursingObservation")]
        public HttpResponseMessage getPatientNursingObservation([FromBody] applicationDetailsRequest request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                PureVaccineDAO.PureVaccineDAO vaccineDAO = new PureVaccineDAO.PureVaccineDAO();

                if (request.applicationNo > 0 || !string.IsNullOrWhiteSpace(request.eid))
                {
                    List < nursingObservationResponse> applicationResponse = vaccineDAO.getPatientNursingObservation(request);
                    if (applicationResponse != null)
                    {
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, applicationResponse, "application/JSON");
                    }
                    else
                    {
                        applicationNotFound applicationNot = new applicationNotFound();
                        applicationNot.isSuccess = false;
                        applicationNot.message = "application details not found";
                        responseMessage = Request.CreateResponse(HttpStatusCode.OK, applicationNot, "application/JSON");
                    }
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return responseMessage;
        }

    }
}
