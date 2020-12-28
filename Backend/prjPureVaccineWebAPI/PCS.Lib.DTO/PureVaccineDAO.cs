using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Public.Library.ErrorHandeling;
using Public.PCS.Main;
using System.Data.SqlClient;
using DataEntityModel;

namespace PureVaccineDAO
{
    public class PureVaccineDAO
    {
        /// <summary>
        /// Save Patient Vaccination Application Into DB
        /// </summary>
        /// <param name="objInputs[]">Object Array Conatning the InPuts Coming form Vaccination Portal</param>
        /// <returns>Retrun Patient Application Number for Vaccination Application</returns>
        public object[] savePatientvaccinationApplication(vaccinationApplications application)
        {
            object[] objOutput = new object[5];
            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable sqlDT = new DataTable();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("procSaveVaccinationApplication", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@fName", application.fullName));
                    sqlCMD.Parameters.Add(new SqlParameter("@dob", application.dob));
                    sqlCMD.Parameters.Add(new SqlParameter("@gender", application.gender));
                    sqlCMD.Parameters.Add(new SqlParameter("@mobileNumber", application.mobileNumber));
                    sqlCMD.Parameters.Add(new SqlParameter("@eid", application.eid));
                    sqlCMD.Parameters.Add(new SqlParameter("@passportNumber", application.passportNumber));
                    sqlCMD.Parameters.Add(new SqlParameter("@nationality", application.nationality));
                    sqlCMD.Parameters.Add(new SqlParameter("@resiEmirate", application.residentEmirate));
                    sqlCMD.Parameters.Add(new SqlParameter("@email", application.emailID));
                    sqlCMD.Parameters.Add(new SqlParameter("@prefLan", application.prefLanguage));
                    sqlCMD.Parameters.Add(new SqlParameter("@uaeAddress", application.uaeAddress));
                    sqlCMD.Parameters.Add(new SqlParameter("@hadCovid19", application.hadCovid19));
                    sqlCMD.Parameters.Add(new SqlParameter("@covid19AntiBodies", application.covid19AntiBodies));
                    sqlCMD.Parameters.Add(new SqlParameter("@covid19Vaccinated", application.covid19Vaccinated));
                    sqlCMD.Parameters.Add(new SqlParameter("@chronicDisease", application.chronicDisease));
                    sqlCMD.Parameters.Add(new SqlParameter("@majorOperationSurg", application.majorOperationSurgery));
                    sqlCMD.Parameters.Add(new SqlParameter("@coronarySyndromeStroke", application.chronicDisease));
                    sqlCMD.Parameters.Add(new SqlParameter("@isPregnant", application.isPregnantOrPlanning));
                    sqlCMD.Parameters.Add(new SqlParameter("@firstDoseLocId", application._firstDoseLocId));
                    sqlCMD.Parameters.Add(new SqlParameter("@firstDoseApptDate", application.firstDoseDate));
                    sqlCMD.Parameters.Add(new SqlParameter("@firstDoseApptSlotID", application.firstDoseApptSlotID));
                    sqlCMD.Parameters.Add(new SqlParameter("@vaccineConsent", application._vaccConsent));
                    sqlCMD.Parameters.Add("@applicationNoOut", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@facilityNameOut", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@facilityAddressOut", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@timeSlotOut", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@errorMessageOut", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output;
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    int i = sqlCMD.ExecuteNonQuery();

                    objOutput[0] = sqlCMD.Parameters["@applicationNoOut"].Value;
                    objOutput[1] = sqlCMD.Parameters["@errorMessageOut"].Value;
                    objOutput[2] = sqlCMD.Parameters["@facilityNameOut"].Value;
                    objOutput[3] = sqlCMD.Parameters["@facilityAddressOut"].Value;
                    objOutput[4] = sqlCMD.Parameters["@timeSlotOut"].Value;
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return objOutput;
        }

        /// <summary>
        /// Get the list of all the emirates in the country
        /// </summary> 
        /// <returns>List<countryEmirates></returns>
        public List<countryEmirates> getCountryEmiratesList()
        {

            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable sqlDT = new DataTable();
            List<countryEmirates> lstEmirates = new List<countryEmirates>();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("procGetCountryEmirates", con);
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand = sqlCMD;
                    sqlDA.Fill(sqlDT);
                    while (sqlDT != null && sqlDT.Rows.Count > 0)
                    {
                        foreach (DataRow row in sqlDT.Rows)
                        {
                            countryEmirates emirates = new countryEmirates();
                            emirates.emirateID = !row.IsNull("Emirates_ID") ? Convert.ToInt32(row["Emirates_ID"]) : 0;
                            emirates.emirateName = !row.IsNull("Emirates_Name") ? row["Emirates_Name"].ToString() : "";

                            lstEmirates.Add(emirates);
                        }
                        sqlDT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return lstEmirates;
        }

        /// <summary>
        /// get the list of appointments
        /// </summary>
        /// <param name="filters">filters applied for appointment date and location id</param>
        /// <returns></returns>
        public List<AppointmentSlots> getLocationAppointmentSlotsByDate(AppointmentFilters filters)
        {

            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable sqlDT = new DataTable();
            List<AppointmentSlots> lstAppointmentSlots = new List<AppointmentSlots>();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[proGetLocationAppointment]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@locationID", filters.locationID));
                    sqlCMD.Parameters.Add(new SqlParameter("@appDate", filters.appointmentDate));

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand = sqlCMD;
                    sqlDA.Fill(sqlDT);
                    while (sqlDT != null && sqlDT.Rows.Count > 0)
                    {
                        foreach (DataRow row in sqlDT.Rows)
                        {
                            AppointmentSlots slots = new AppointmentSlots();
                            slots.slotID = !row.IsNull("SlotID") ? Convert.ToInt32(row["SlotID"]) : 0;
                            slots.slotStartTime = !row.IsNull("SlotStart") ? row["SlotStart"].ToString() : "";
                            slots.slotEndTime = !row.IsNull("SlotEnd") ? row["SlotEnd"].ToString() : "";
                            slots.isBooked = !row.IsNull("IsAvailable") ? Convert.ToInt16(row["IsAvailable"]) : 0;

                            lstAppointmentSlots.Add(slots);
                        }
                        sqlDT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return lstAppointmentSlots;
        }

        /// <summary>
        /// Validate user using User id and password supplied
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public validatedUser validateUserLogins(string userId, string pwd)
        {

            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            validatedUser userValidation = new validatedUser();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[procValidateUser]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@userID", userId));
                    sqlCMD.Parameters.Add(new SqlParameter("@pwd", pwd));

                    sqlCMD.Parameters.Add("@fName", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@isNurse", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@isDEO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@isAdmin", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@hasWalkInAccess", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@hasPostVaccinationAccess", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@lastLoginLocId", SqlDbType.Int).Direction = ParameterDirection.Output;

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    int i = sqlCMD.ExecuteNonQuery();

                    if (!Convert.IsDBNull(sqlCMD.Parameters["@fName"].Value))
                    {
                        userValidation.isValid = true;
                        userValidation.fullName = sqlCMD.Parameters["@fName"].Value.ToString();
                    }
                    if (!Convert.IsDBNull(sqlCMD.Parameters["@isNurse"].Value))
                    {
                        userValidation.isNurse = Convert.ToBoolean(sqlCMD.Parameters["@isNurse"].Value);
                    }
                    if (!Convert.IsDBNull(sqlCMD.Parameters["@isDEO"].Value))
                    {
                        userValidation.isDeo = Convert.ToBoolean(sqlCMD.Parameters["@isDEO"].Value);
                    }
                    if (!Convert.IsDBNull(sqlCMD.Parameters["@isAdmin"].Value))
                    {
                        userValidation.isAdmin = Convert.ToBoolean(sqlCMD.Parameters["@isAdmin"].Value);
                    }
                    if (!Convert.IsDBNull(sqlCMD.Parameters["@hasWalkInAccess"].Value))
                    {
                        userValidation.hasWalkinAccess = Convert.ToBoolean(sqlCMD.Parameters["@hasWalkInAccess"].Value);
                    }
                    if (!Convert.IsDBNull(sqlCMD.Parameters["@lastLoginLocId"].Value))
                    {
                        userValidation.lastLoginLocID = Convert.ToInt16(sqlCMD.Parameters["@lastLoginLocId"].Value);
                    }

                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return userValidation;
        }

        /// <summary>
        /// Save Patient Communication for email and sms
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public validatedUser savePatientCommunicationLog(patientCommunication commLog)
        {
            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            validatedUser userValidation = new validatedUser();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[procSavePatientCommuicationLog]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@appNo", commLog.applicationNo));
                    sqlCMD.Parameters.Add(new SqlParameter("@userID", commLog.userId));
                    sqlCMD.Parameters.Add(new SqlParameter("@commType", commLog.commType));
                    sqlCMD.Parameters.Add(new SqlParameter("@emailAddress", commLog.emailAddress));
                    sqlCMD.Parameters.Add(new SqlParameter("@smsPhoneNumber", commLog.smsPhoneNumber));
                    sqlCMD.Parameters.Add(new SqlParameter("@subject", commLog.msgSubject));
                    sqlCMD.Parameters.Add(new SqlParameter("@content", commLog.msgContent));

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    int i = sqlCMD.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return userValidation;
        }

        /// <summary>
        /// get the list of appointments
        /// </summary>
        /// <param name="filters">filters applied for appointment date and location id</param>
        /// <returns></returns>
        public List<vaccinationLocations> getVaccinationLocationsByCity(int _cityID)
        {

            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable sqlDT = new DataTable();
            List<vaccinationLocations> lstVaccLocation = new List<vaccinationLocations>();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[procGetVaccinationLocationByCity]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@cityID", _cityID));

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand = sqlCMD;
                    sqlDA.Fill(sqlDT);
                    while (sqlDT != null && sqlDT.Rows.Count > 0)
                    {
                        foreach (DataRow row in sqlDT.Rows)
                        {
                            vaccinationLocations vloc = new vaccinationLocations();
                            vloc.locationID = !row.IsNull("LocID") ? Convert.ToInt32(row["LocID"]) : 0;
                            vloc.locationDesc = !row.IsNull("LocDesc") ? row["LocDesc"].ToString() : "";
                            vloc.locationCity = !row.IsNull("LocCity") ? row["LocCity"].ToString() : "";
                            vloc.locationLatitude = !row.IsNull("LocLatitude") ? row["LocLatitude"].ToString() : "";
                            vloc.locationLongitude = !row.IsNull("LocLongitude") ? row["LocLongitude"].ToString() : "";

                            lstVaccLocation.Add(vloc);
                        }
                        sqlDT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return lstVaccLocation;
        }

        /// <summary>
        /// get list of all the locations
        /// </summary>
        /// <returns></returns>
        public List<Locations> getAllLocations()
        {

            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable sqlDT = new DataTable();
            List<Locations> lstVaccLocation = new List<Locations>();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[procGetAllLocations]", con);

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand = sqlCMD;
                    sqlDA.Fill(sqlDT);
                    while (sqlDT != null && sqlDT.Rows.Count > 0)
                    {
                        foreach (DataRow row in sqlDT.Rows)
                        {
                            Locations vloc = new Locations();
                            vloc.locationID = !row.IsNull("LocID") ? Convert.ToInt32(row["LocID"]) : 0;
                            vloc.locationDesc = !row.IsNull("LocDesc") ? row["LocDesc"].ToString() : "";

                            lstVaccLocation.Add(vloc);
                        }
                        sqlDT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return lstVaccLocation;
        }

        /// <summary>
        /// Get Email or SMS Templates by Keyword and Type ID 1: Email / 2 : SMS
        /// </summary>
        /// <param name="_typeID"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public string getEmailSMSTemplates(int _typeID, string keyWord)
        {
            string retVal = string.Empty;
            SqlCommand sqlCMD = new SqlCommand();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("SELECT dbo.getEmailSMSTemplate(@typeID,@keyWord)", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@typeID", _typeID));
                    sqlCMD.Parameters.Add(new SqlParameter("@keyWord", keyWord));

                    sqlCMD.CommandType = CommandType.Text;
                    retVal = (string)sqlCMD.ExecuteScalar();
                    return retVal;
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return retVal;
        }

        /// <summary>
        /// Save OTP Communication details and Retrun First dose administration status
        /// </summary>
        /// <param name="emiratesId"></param>
        /// <param name="mobileNumber"></param>
        /// <param name="emailId"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public object[] saveOTPCommunicationDetails(string emiratesId, string mobileNumber, string emailId, string otp)
        {
            SqlCommand sqlCMD = new SqlCommand();
            object[] objOutValues = new object[2];
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[saveOTPCommunication]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@eid", emiratesId));
                    sqlCMD.Parameters.Add(new SqlParameter("@mobileNumber", mobileNumber));
                    sqlCMD.Parameters.Add(new SqlParameter("@emailId", emailId));
                    sqlCMD.Parameters.Add(new SqlParameter("@otp", otp));

                    sqlCMD.Parameters.Add("@hasCompletedFirstDose", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    sqlCMD.Parameters.Add("@uniqueKey", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    int i = sqlCMD.ExecuteNonQuery();

                    if (!Convert.IsDBNull(sqlCMD.Parameters["@hasCompletedFirstDose"].Value))
                    {
                        objOutValues[0] = !Convert.IsDBNull(sqlCMD.Parameters["@hasCompletedFirstDose"].Value) ? Convert.ToBoolean(sqlCMD.Parameters["@hasCompletedFirstDose"].Value) : false;
                    }

                    if (!Convert.IsDBNull(sqlCMD.Parameters["@uniqueKey"].Value))
                    {
                        objOutValues[1] = sqlCMD.Parameters["@uniqueKey"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return objOutValues;
        }

        /// <summary>
        /// Validate OTP entered by user and return true or false
        /// </summary>
        /// <param name="uniqueKey">GUID Id for each otp transaction</param>
        /// <param name="otp">OTP Received by user</param>
        /// <returns></returns>
        public bool validateOTP(string uniqueKey, string otp)
        {
            bool isValid = false;
            SqlCommand sqlCMD = new SqlCommand();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("SELECT dbo.validateOTP(@trnKey,@otp)", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@trnKey", uniqueKey));
                    sqlCMD.Parameters.Add(new SqlParameter("@otp", otp));

                    sqlCMD.CommandType = CommandType.Text;
                    isValid = (bool)sqlCMD.ExecuteScalar();
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return isValid;
        }

        /// <summary>
        /// get application details using application number
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public applicationDetailsResponse getApplicationDetails(applicationDetailsRequest application)
        {
            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable sqlDT = new DataTable();
            applicationDetailsResponse applicationDetails = null;
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[procGetVaccineApplicationDetails]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@applicationNo", application.applicationNo));
                    sqlCMD.Parameters.Add(new SqlParameter("@eid",  application.eid ?? ""));

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand = sqlCMD;
                    sqlDA.Fill(sqlDT);
                    while (sqlDT != null && sqlDT.Rows.Count > 0)
                    {
                        foreach (DataRow row in sqlDT.Rows)
                        {
                            applicationDetails = new applicationDetailsResponse();
                            applicationDetails.applicationNo = !row.IsNull("ApplicationNo") ? Convert.ToInt64(row["ApplicationNo"]) : 0;
                            applicationDetails.fullName = !row.IsNull("FullName") ? row["FullName"].ToString() : "";
                            applicationDetails.dob = !row.IsNull("DOB") ? row["DOB"].ToString() : "";
                            applicationDetails.gender = !row.IsNull("Gender") ? row["Gender"].ToString() : "";
                            applicationDetails.mobileNumber = !row.IsNull("MobileNumber") ? row["MobileNumber"].ToString() : "";
                            applicationDetails.eid = !row.IsNull("EID") ? row["EID"].ToString() : "";
                            applicationDetails.passportNumber = !row.IsNull("PassportNumber") ? row["PassportNumber"].ToString() : "";
                            applicationDetails.hadCovid19 = !row.IsNull("HadCovid19") ? Convert.ToBoolean(row["HadCovid19"].ToString()) : false;
                            applicationDetails.covid19AntiBodies = !row.IsNull("Covid19AntiBodies") ? Convert.ToBoolean(row["Covid19AntiBodies"].ToString()) : false;
                            applicationDetails.covid19Vaccinated = !row.IsNull("Covid19Vaccinated") ? Convert.ToBoolean(row["Covid19Vaccinated"].ToString()) : false;
                            applicationDetails.chronicDisease = !row.IsNull("ChronicDisease") ? Convert.ToBoolean(row["ChronicDisease"].ToString()) : false;
                            applicationDetails.majorOperationSurg = !row.IsNull("MajorOperationSurg") ? Convert.ToBoolean(row["MajorOperationSurg"].ToString()) : false;
                            applicationDetails.coronarySyndromeStroke = !row.IsNull("CoronarySyndromeStroke") ? Convert.ToBoolean(row["CoronarySyndromeStroke"].ToString()) : false;
                            applicationDetails.isPregnant = !row.IsNull("IsPregnant") ? Convert.ToBoolean(row["IsPregnant"].ToString()) : false;
                            applicationDetails.firstDoseLocation = !row.IsNull("First_Dose_Loc") ? row["First_Dose_Loc"].ToString() : "";
                            applicationDetails.firstApptDate = !row.IsNull("First_Appt_Date") ? row["First_Appt_Date"].ToString() : "";
                            applicationDetails.firstApptStartTime = !row.IsNull("First_Appt_STime") ? row["First_Appt_STime"].ToString() : "";
                            applicationDetails.firstApptEndTime = !row.IsNull("First_Appt_ETime") ? row["First_Appt_ETime"].ToString() : "";
                            applicationDetails.secondDoseLocation = !row.IsNull("Second_Dose_Loc") ? row["Second_Dose_Loc"].ToString() : "";
                            applicationDetails.secondApptDate = !row.IsNull("Second_Appt_Date") ? row["Second_Appt_Date"].ToString() : "";
                            applicationDetails.secondApptStartTime = !row.IsNull("Second_Appt_STime") ? row["Second_Appt_STime"].ToString() : "";
                            applicationDetails.secondApptEndTime = !row.IsNull("Second_Appt_ETime") ? row["Second_Appt_ETime"].ToString() : "";
                            applicationDetails.isIDVerifiedByDEO = !row.IsNull("isIDVerifiedByDEO") ? Convert.ToBoolean(row["isIDVerifiedByDEO"].ToString()) : false;
                            applicationDetails.DEOUserID = !row.IsNull("DEOUserID") ? row["DEOUserID"].ToString() : "";
                            applicationDetails.DEOVerificationDT = !row.IsNull("DEOVerificationDT") ? row["DEOVerificationDT"].ToString() : "";
                        }
                        sqlDT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return applicationDetails;

        }

        /// <summary>
        /// save patient nursing observation
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool savePatientNursingObservations(nursingEntry entry)
        {
            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            bool _isSaved = false;
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[savePatientNursingObservations]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@applicationNumber", entry.applicationNumber));
                    sqlCMD.Parameters.Add(new SqlParameter("@Tympanic", entry.tympanic));
                    sqlCMD.Parameters.Add(new SqlParameter("@PPR", entry.PPR));
                    sqlCMD.Parameters.Add(new SqlParameter("@AHR", entry.AHR));
                    sqlCMD.Parameters.Add(new SqlParameter("@RR", entry.RR));
                    sqlCMD.Parameters.Add(new SqlParameter("@BP", entry.BP));
                    sqlCMD.Parameters.Add(new SqlParameter("@WT", entry.WT));
                    sqlCMD.Parameters.Add(new SqlParameter("@HT", entry.HT)); 
                    sqlCMD.Parameters.Add(new SqlParameter("@vaccineName", entry.vaccineName));
                    sqlCMD.Parameters.Add(new SqlParameter("@vaccineLotNumber", entry.vaccineLotNumber));
                    sqlCMD.Parameters.Add(new SqlParameter("@vitalStatus", entry.vitalStatus));
                    sqlCMD.Parameters.Add(new SqlParameter("@rejectionComment", entry.rejectionComment));
                    sqlCMD.Parameters.Add(new SqlParameter("@createdBy", entry.createdBy));
                    sqlCMD.Parameters.Add(new SqlParameter("@userLocID", entry.userLocID));

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    int i = sqlCMD.ExecuteNonQuery();
                    _isSaved = true;
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return _isSaved;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool updateDEOResponseForApplication(applicationUpdateRequestByDEO entry)
        {
            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            bool _isSaved = false;
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[procUpdateDEOResponse]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@applicationNo", entry.applicationNumber));
                    sqlCMD.Parameters.Add(new SqlParameter("@isIDVerified", entry.isIdChecked));
                    sqlCMD.Parameters.Add(new SqlParameter("@DEOUserID", entry.DEOUserId));
                    sqlCMD.Parameters.Add(new SqlParameter("@userLocID", entry.userLocID));

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    int i = sqlCMD.ExecuteNonQuery();
                    _isSaved = true;
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return _isSaved;
        }

        /// <summary>
        /// save patient adverse reaction
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool savePatientAdverseReaction(adverseReactionEntry entry)
        {
            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            bool _isSaved = false;
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[savePatientAdverseReaction]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@applicationNumber", entry.applicationNumber));
                    sqlCMD.Parameters.Add(new SqlParameter("@AnyVaccineAbnormality", entry.anyVaccineAbnormality));
                    sqlCMD.Parameters.Add(new SqlParameter("@InjectionOnsiteReaction", entry.injectionOnsiteReaction));
                    sqlCMD.Parameters.Add(new SqlParameter("@OnSiteReactionComments", entry.onSiteReactionComments));
                    sqlCMD.Parameters.Add(new SqlParameter("@Redness", entry.redness));
                    sqlCMD.Parameters.Add(new SqlParameter("@Swelling", entry.swelling));
                    sqlCMD.Parameters.Add(new SqlParameter("@Induration", entry.induration));
                    sqlCMD.Parameters.Add(new SqlParameter("@rash", entry.rash));
                    sqlCMD.Parameters.Add(new SqlParameter("@pruritus", entry.pruritus));
                    sqlCMD.Parameters.Add(new SqlParameter("@pain", entry.pain));
                    sqlCMD.Parameters.Add(new SqlParameter("@otherComments", entry.otherComments));
                    sqlCMD.Parameters.Add(new SqlParameter("@otherSymptoms", entry.otherSymptoms));
                    sqlCMD.Parameters.Add(new SqlParameter("@createdBy", entry.createdBy)); 

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    int i = sqlCMD.ExecuteNonQuery();
                    _isSaved = true;
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return _isSaved;
        }

        /// <summary>
        /// get application details using application number
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public List<adverserReactionResponse> getAdverseReactionDetails(applicationDetailsRequest application)
        {
            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable sqlDT = new DataTable();
            List<adverserReactionResponse> lstAdversereaction = new List<adverserReactionResponse>();
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[procGetPatientAdverseReactions]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@applicationNo", application.applicationNo));
                    sqlCMD.Parameters.Add(new SqlParameter("@eid", application.eid ?? ""));

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand = sqlCMD;
                    sqlDA.Fill(sqlDT);
                    while (sqlDT != null && sqlDT.Rows.Count > 0)
                    {
                        foreach (DataRow row in sqlDT.Rows)
                        {
                            adverserReactionResponse adverseReationDetails = new adverserReactionResponse();
                            adverseReationDetails.applicationNumber = !row.IsNull("ApplicationNo") ? Convert.ToInt64(row["ApplicationNo"]) : 0;
                            adverseReationDetails.dno = !row.IsNull("DNO") ? Convert.ToInt16(row["DNO"]) : 0;
                            adverseReationDetails.anyVaccineAbnormality = !row.IsNull("AnyVaccineAbnormality") ? Convert.ToBoolean(row["AnyVaccineAbnormality"].ToString()) : false;
                            adverseReationDetails.injectionOnsiteReaction = !row.IsNull("InjectionOnsiteReaction") ? Convert.ToBoolean(row["InjectionOnsiteReaction"].ToString()) : false;
                            adverseReationDetails.onSiteReactionComments = !row.IsNull("OnSiteReactionComments") ? row["OnSiteReactionComments"].ToString() : "";
                            adverseReationDetails.redness = !row.IsNull("Redness") ? Convert.ToBoolean(row["Redness"].ToString()) : false;
                            adverseReationDetails.swelling = !row.IsNull("Swelling") ? Convert.ToBoolean(row["Swelling"].ToString()) : false;
                            adverseReationDetails.induration = !row.IsNull("Induration") ? Convert.ToBoolean(row["Induration"].ToString()) : false;
                            adverseReationDetails.rash = !row.IsNull("Rash") ? Convert.ToBoolean(row["Rash"].ToString()) : false;
                            adverseReationDetails.pruritus = !row.IsNull("Pruritus") ? Convert.ToBoolean(row["Pruritus"].ToString()) : false;
                            adverseReationDetails.pain = !row.IsNull("Pain") ? Convert.ToBoolean(row["Pain"].ToString()) : false;
                            adverseReationDetails.otherComments = !row.IsNull("OtherComments") ? row["OtherComments"].ToString() : "";
                            adverseReationDetails.otherSymptoms = !row.IsNull("OtherSymptoms") ? row["OtherSymptoms"].ToString() : "";
                            adverseReationDetails.createdBy = !row.IsNull("CreatedBy") ? row["CreatedBy"].ToString() : "";
                            adverseReationDetails.createdDate = !row.IsNull("CreatedDT") ? row["CreatedDT"].ToString() : "";
                            adverseReationDetails.isSuccess = true;

                            lstAdversereaction.Add(adverseReationDetails);
                        }
                        sqlDT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return lstAdversereaction;

        }


        /// <summary>
        /// get application details using application number
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public List<nursingObservationResponse> getPatientNursingObservation(applicationDetailsRequest application)
        {
            SqlCommand sqlCMD = new SqlCommand();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            DataTable sqlDT = new DataTable();
            List<nursingObservationResponse> lstNursingObservations = new List<nursingObservationResponse>();
            
            try
            {
                using (SqlConnection con = clsSQLConnection.getSQLConnection())
                {
                    sqlCMD = new SqlCommand("[procGetPatientNursingObservation]", con);
                    sqlCMD.Parameters.Add(new SqlParameter("@applicationNo", application.applicationNo));
                    sqlCMD.Parameters.Add(new SqlParameter("@eid", application.eid ?? ""));

                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlDA.SelectCommand = sqlCMD;
                    sqlDA.Fill(sqlDT);
                    while (sqlDT != null && sqlDT.Rows.Count > 0)
                    {
                        foreach (DataRow row in sqlDT.Rows)
                        {
                            nursingObservationResponse nursingObservationDetails  = new nursingObservationResponse();
                            nursingObservationDetails.applicationNumber = !row.IsNull("ApplicationNo") ? Convert.ToInt64(row["ApplicationNo"]) : 0;
                            nursingObservationDetails.dno = !row.IsNull("DNO") ? Convert.ToInt16(row["DNO"]) : 0;
                            nursingObservationDetails.tympanic = !row.IsNull("Tympanic") ? row["Tympanic"].ToString() : "";
                            nursingObservationDetails.PPR = !row.IsNull("PPR") ? row["PPR"].ToString() : "";
                            nursingObservationDetails.AHR = !row.IsNull("AHR") ? row["AHR"].ToString() : "";
                            nursingObservationDetails.RR = !row.IsNull("RR") ? row["RR"].ToString() : "";
                            nursingObservationDetails.BP = !row.IsNull("BP") ? row["BP"].ToString() : "";
                            nursingObservationDetails.WT = !row.IsNull("WT") ? row["WT"].ToString() : "";
                            nursingObservationDetails.HT = !row.IsNull("HT") ? row["HT"].ToString() : "";
                            nursingObservationDetails.vaccineName = !row.IsNull("VaccineName") ? row["VaccineName"].ToString() : "";
                            nursingObservationDetails.vaccineLotNumber = !row.IsNull("VaccineLotNumber") ? row["VaccineLotNumber"].ToString() : "";
                            nursingObservationDetails.vitalStatus = !row.IsNull("VitalStatus") ? Convert.ToBoolean(row["VitalStatus"].ToString()) : false;
                            nursingObservationDetails.rejectionComment = !row.IsNull("RejectionComment") ? row["RejectionComment"].ToString() : "";
                            nursingObservationDetails.createdBy = !row.IsNull("CreatedBy") ? row["CreatedBy"].ToString() : "";
                            nursingObservationDetails.createdDate = !row.IsNull("CreatedDT") ? row["CreatedDT"].ToString() : "";
                            nursingObservationDetails.isSuccess = true;

                            lstNursingObservations.Add(nursingObservationDetails);
                        }
                        sqlDT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            finally
            {
                sqlCMD.Dispose();
            }
            return lstNursingObservations; 
        }
    }

}
