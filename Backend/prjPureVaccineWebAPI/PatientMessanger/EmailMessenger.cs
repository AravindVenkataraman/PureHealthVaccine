using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Public.Library.ErrorHandeling;


namespace PatientMessenger
{
    public class EmailMessenger
    {
        public async Task<bool> sendEMail(EmailProperties emailProperties)
        {
            bool isSent = false;
            try
            {
                var client = new SendGridClient(emailProperties.emailAPIKey);
                var from = new EmailAddress(emailProperties.emailFrom, emailProperties.emailFromName);
                var subject = emailProperties.emailSubject;
                var to = new EmailAddress(emailProperties.emailTo, emailProperties.emailToName);
                var plainTextContent = emailProperties.emailPlainTextContent;
                var htmlContent = emailProperties.emailHTMLContent;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return isSent = true;
            }
            catch(Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return isSent;
        }
    }

    public class EmailProperties
    {
        public string emailAPIKey { get; set; } 
        public string emailFrom { get; set; }
        public string emailFromName { get; set; }
        public string emailSubject { get; set; }
        public string emailTo { get; set; }
        public string emailToName { get; set; } 
        public string emailPlainTextContent { get; set; }
        public string emailHTMLContent { get; set; }
    }

    public class SMSMessenger
    {
        public bool sendSMS(SMSProperties properties)
        {
            bool isSent = false;
            using (var web = new System.Net.WebClient()) 
            {
                try
                {
                    string userName = properties.smsAPIUserName;
                    string userPassword = properties.smsAPIUserPassword;
                    string msgRecepient = properties.smsRecepient;
                    string msgText = properties.smsText;
                    string senderID = properties.smsSenderID;
                    string url = "https://www.smartsmsgateway.com/api/api_http.php?" +
                    "username=" + userName +
                    "&password=" + userPassword +
                    "&senderid=" + senderID +
                    "&text=" + msgText +
                    "&type=" + "text" +
                    "&datetime=" + DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") +
                    "&to=" + msgRecepient;  
                    string result = web.DownloadString((url));
                    if (result.Contains("OK"))
                    {
                       return isSent = true;
                    }
                    else
                    {
                        clsEvntvwrLogging.fnMsgWritter("Error while sending the SMS");
                    }
                } 
                catch (Exception ex)
                {
                    clsEvntvwrLogging.fnLogWritter(ex);
                }
                return isSent;
            }
        }
    }

    public class SMSProperties
    { 
        public string smsAPIUserName { get; set; }
        public string smsAPIUserPassword { get; set; }
        public string smsRecepient { get; set; }
        public string smsText { get; set; }
        public string smsSenderID { get; set; }
        
    }
   
}
