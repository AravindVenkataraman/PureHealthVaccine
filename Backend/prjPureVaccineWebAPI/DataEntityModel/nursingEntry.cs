using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntityModel
{
    /// <summary>
    /// Nursing data input
    /// </summary>
    public class nursingEntry
    {
        public long applicationNumber { get; set; }
        public string tympanic { get; set; }
        public string PPR { get; set; }
        public string AHR { get; set; }
        public string RR { get; set; }
        public string BP { get; set; }
        public string WT { get; set; }
        public string HT { get; set; } 
        public string vaccineName { get; set; }
        public string vaccineLotNumber { get; set; }
        public bool vitalStatus { get; set; }
        public string rejectionComment { get; set; }
        public string createdBy { get; set; }
        public int userLocID { get; set; }
    }

    /// <summary>
    /// Response to return for Nursing Data Save Request
    /// </summary>
    public class nursingEntryResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }



    /// <summary>
    /// Adverse Reaction data input
    /// </summary>
    public class adverseReactionEntry
    {
        public long applicationNumber { get; set; }
        public bool anyVaccineAbnormality { get; set; }
        public bool injectionOnsiteReaction { get; set; }
        public string onSiteReactionComments { get; set; }
        public bool redness { get; set; }
        public bool swelling { get; set; }
        public bool induration { get; set; }
        public bool rash { get; set; }
        public bool pruritus { get; set; }
        public bool pain { get; set; }
        public string otherComments { get; set; }
        public string otherSymptoms { get; set; }
        public string createdBy { get; set; }
       
    }


    /// <summary>
    /// Response to return for Nursing Data Save Request
    /// </summary>
    public class adverseReactionEntryResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }


    /// <summary>
    /// Application update sent by DEO
    /// </summary>
    public class applicationUpdateRequestByDEO
    {
        public long applicationNumber { get; set; }
        public bool isIdChecked { get; set; }
        public string DEOUserId { get; set; }
        public int userLocID { get; set; }
    }

    /// <summary>
    /// Application Update Response for DEO Request
    /// </summary>
    public class applicationUpdateResponseForDEO
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }

    public class adverserReactionResponse
    {
        public bool isSuccess { get; set; }
        public long applicationNumber { get; set; }
        public int dno { get; set; }
        public bool anyVaccineAbnormality { get; set; }
        public bool injectionOnsiteReaction { get; set; }
        public string onSiteReactionComments { get; set; }
        public bool redness { get; set; }
        public bool swelling { get; set; }
        public bool induration { get; set; }
        public bool rash { get; set; }
        public bool pruritus { get; set; }
        public bool pain { get; set; }
        public string otherComments { get; set; }
        public string otherSymptoms { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }

    }

    public class nursingObservationResponse
    {
        public bool isSuccess { get; set; }
        public long applicationNumber { get; set; }
        public int dno { get; set; }
        public string tympanic { get; set; }
        public string PPR { get; set; }
        public string AHR { get; set; }
        public string RR { get; set; }
        public string BP { get; set; }
        public string WT { get; set; }
        public string HT { get; set; }
        public string vaccineName { get; set; }
        public string vaccineLotNumber { get; set; }
        public bool vitalStatus { get; set; }
        public string rejectionComment { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }

    }
}
