using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntityModel
{
    public class VaccineApplicationFormInput
    {
        public string fullName { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string mobileNumber { get; set; }
        public string eid { get; set; }
        public string passportNumber { get; set; }
        public string nationality { get; set; }
        public string residentEmirate { get; set; }
        public string email { get; set; }
        public string prefLanguage { get; set; }
        public string uaeAddress { get; set; }
        public bool hadCovid19 { get; set; }
        public bool covid19AntiBodies { get; set; }
        public bool covid19Vaccinated { get; set; }
        public bool chronicDisease { get; set; }
        public bool majorOperationSurgery { get; set; }
        public bool coronarySyndromeStroke { get; set; }
        public bool isPregnantOrPlanning { get; set; }
        public int firstDoseLocId { get; set; }
        public string firstDoseDate { get; set; }
        public long firstDoseApptSlotID { get; set; }
        public bool vaccineConsent { get; set; }
    }

    public class AppointmentFilters
    {
        public int locationID { get; set; }

        public string appointmentDate { get; set; }
    }
}
