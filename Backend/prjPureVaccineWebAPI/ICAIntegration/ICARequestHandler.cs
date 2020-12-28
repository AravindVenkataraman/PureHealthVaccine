using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ae.gov.id.crypto;
using System.IO;
using System.Net;
using System.Xml;

namespace ICAIntegration
{
    public class ICARequestHandler
    {
        public ICAResponse getPersonByEmiratesId(string emiratesId)
        {
            string timestamp = DateTime.Now.ToString("yyMMddHHmmss");
            String timeStamp2 = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss.fff") + "+04:00";

            string RandomDigits(int length)
            {
                var random = new Random();
                string s = string.Empty;
                for (int i = 0; i < length; i++)
                    s = String.Concat(s, random.Next(10).ToString());
                return s;
            }

            string randomnm = RandomDigits(10);
            string transactionalRef = "042" + randomnm + timestamp;
            string body = @"<prof1:Body>
                                <prof1:transactionRefNo>" + transactionalRef + @"</prof1:transactionRefNo>
                                <prof1:idn>" + emiratesId + @"</prof1:idn>
                                <prof1:timestamp>" + timeStamp2 + @"</prof1:timestamp>
                            </prof1:Body>";

            string username = "ESB.PURE_HEALTH";

            string key = "FBA9B32617F81A9778C118B5F0A116FD8CE3860A17A767865383415A7B594180";

            string password = "QWo09*()@";

            string hash = MessageSigner.SignMessage(body, key);
            string encrypt_username = Encrypter.EncryptData(username, key);
            string encrypt_password = Encrypter.EncryptData(password, key);

            string xmlMessage = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:prof=\"http://id.gov.ae/ws/enquiry/profile\" xmlns:com=\"http://id.gov.ae/schema/commontypes\" xmlns:prof1=\"http://id.gov.ae/schema/enquiry/profile\">\r\n" +
                            "<soapenv:Header/>\r\n" +
                            "<soapenv:Body>" +
                               "<prof:getPersonProfileInfo>" +
                                  "<com:Header>" +
                                     @"<com:serviceName>GET_PERSONPROFILE_INFO</com:serviceName>
                                        <com:sourceChannel>PURE_HEALTH</com:sourceChannel>
                                        <com:serviceVersion>1</com:serviceVersion>
                                        <com:serviceLanguage>EN</com:serviceLanguage>
                                        <com:userName>" + encrypt_username + @"</com:userName>
                                        <com:password>" + encrypt_password + @"</com:password>
                                        <com:hash>" + hash + @"</com:hash>
                                    </com:Header>" +
                                                body
                                  +
                                @"</prof:getPersonProfileInfo>
                            </soapenv:Body>
                            </soapenv:Envelope>";

            string url = System.Configuration.ConfigurationManager.AppSettings["ICAAPIURL"];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            byte[] requestInFormOfBytes = System.Text.Encoding.ASCII.GetBytes(xmlMessage);
            request.Method = "POST";
            request.ContentType = "text/xml;charset=utf-8";
            request.ContentLength = requestInFormOfBytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(requestInFormOfBytes, 0, requestInFormOfBytes.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
            string receivedResponse = respStream.ReadToEnd();
            respStream.Close();
            response.Close();
            ICAResponse icaResposne = new ICAResponse();

            while (!string.IsNullOrEmpty(receivedResponse))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(receivedResponse);

                XmlNamespaceManager xmlnsManager = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);
                xmlnsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                xmlnsManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xmlnsManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
                xmlnsManager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
                xmlnsManager.AddNamespace("soapenc", "http://schemas.xmlsoap.org/soap/encoding/");
                xmlnsManager.AddNamespace("NS1", "http://id.gov.ae/ws/enquiry/profile");
                xmlnsManager.AddNamespace("NS2", "http://id.gov.ae/schema/commontypes");
                xmlnsManager.AddNamespace("NS3", "http://id.gov.ae/schema/enquiry/profile");




                //responseDescription for error code handling

                XmlNode responseDesc = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:responseDescription", xmlnsManager);
                if (responseDesc.InnerText.ToUpper() != "SUCCESS")
                {
                    icaResposne.responseDescription = responseDesc.InnerText;
                    receivedResponse = null;
                    continue;
                }

                //unified Number
                XmlNode unifiedNumber = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:unifiedNo", xmlnsManager);
                icaResposne.unifiedNo = unifiedNumber.InnerText;

                //Emirates ID Number
                XmlNode nationalId = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:nationalId", xmlnsManager);
                icaResposne.idn = nationalId.InnerText;

                //Name in English
                XmlNode nameEnglish = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:nameEnglish", xmlnsManager);
                icaResposne.nameEnglish = nameEnglish.InnerText;

                //Name in Arabic
                XmlNode nameArabic = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:nameArabic", xmlnsManager);
                icaResposne.nameArabic = nameArabic.InnerText;

                //religion
                XmlNode religion = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:religion/@english_desc", xmlnsManager);
                icaResposne.religion = nameArabic.InnerText;

                //maritalStatus
                XmlNode maritalStatus = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:maritalStatus/@english_desc", xmlnsManager);
                icaResposne.maritalStatus = maritalStatus.InnerText;

                //Date of Birth
                XmlNode dob = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:birthDate", xmlnsManager);
                icaResposne.birthDate = dob.InnerText;

                //Gender
                XmlNode gender = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:gender/@english_desc", xmlnsManager);
                icaResposne.gender = gender.InnerText;

                //mobileNo
                XmlNode mobileNO = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:mobileNo", xmlnsManager);
                icaResposne.mobileNo = mobileNO.InnerText;

                //passportNationality as nationality
                XmlNode nationality = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:activePassport/NS3:passportNationality/@english_desc", xmlnsManager);
                icaResposne.nationality = nationality.InnerText;

                //passportNo
                XmlNode passportNumber = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:activePassport/NS3:passportNo", xmlnsManager);
                icaResposne.passportNo = passportNumber.InnerText;

                //Passport Issue Date
                XmlNode issueDate = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:activePassport/NS3:issueDate", xmlnsManager);
                icaResposne.issueDate = issueDate.InnerText;

                //passport expiry
                XmlNode expiryDate = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:activePassport/NS3:expiryDate", xmlnsManager);
                icaResposne.expiryDate = expiryDate.InnerText;

                //preferred Lang
                XmlNode preferredLang = xmlDoc.SelectSingleNode("/soap:Envelope/soapenv:Body/NS1:getPersonProfileInfoResponse/NS3:Body/NS3:profileInfo/NS3:preferredLang", xmlnsManager);
                icaResposne.preferredLang = preferredLang.InnerText;

                if (!string.IsNullOrEmpty(icaResposne.birthDate))
                {
                    DateTime dtDOB = DateTime.ParseExact(icaResposne.birthDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                    icaResposne.age = calculateAge(dtDOB);
                }

                receivedResponse = null;

                return icaResposne;
            }

            return icaResposne;
        }

        public string calculateAge(DateTime dob)
        {
            DateTime today = DateTime.Today;

            int months = today.Month - dob.Month;
            int years = today.Year - dob.Year;

            if (today.Day < dob.Day)
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            int days = (today - dob.AddMonths((years * 12) + months)).Days;

            return string.Format("{0}Y/{1}M/{2}D", years,months,days);
        }
    }
}
