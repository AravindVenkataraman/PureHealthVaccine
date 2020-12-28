using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAIntegration
{
    public class ICAResponse
    {
        public string unifiedNo { get; set; }
        public string idn { get; set; }
        public string nameArabic { get; set; }
        public string nameEnglish { get; set; }
        public string nationality { get; set; }
        public string birthDate { get; set; }
        public string gender { get; set; }
        public string religion { get; set; }
        public string maritalStatus { get; set; }
        public string passportNo { get; set; }
        public string issueDate { get; set; }
        public string expiryDate { get; set; }
        public string mobileNo { get; set; }
        public string nationalId { get; set; }
        public string nationalIdCardNo { get; set; }
        public string preferredLang { get; set; } 
        public string age { get; set; }
        public string responseDescription { get; set; }
    }
}
