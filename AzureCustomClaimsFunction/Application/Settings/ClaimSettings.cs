using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedEntraToolkit.Application.Settings
{
    public class ClaimSettings
    {
        public ClaimSettings() 
        {
            Prefixes = new Dictionary<string, string>();
            Assurance_LOW = new List<string>();
            Assurance_MEDIUM = new List<string>();
            Assurance_HIGH = new List<string>();
        }

        public string Scope { get; set; }
        public string EppnBase { get; set; }
        public string O { get; set; }
        public string Co { get; set; }
        public string C { get; set; }
        public string NorEduOrgAcronym { get; set; }
        public string GivenName { get; set; }
        public string Sn { get; set; }
        public string DisplayName { get; set; }
        public string Mail { get; set; }
        public string MailLocalAddress { get; set; }
        public string EduPersonOrcid { get; set; }
        public string NorEduPersonNIN { get; set; }
        public string PersonalIdentityNumber { get; set; }
        public string SchacDateOfBirth { get; set; }
        public Dictionary<string, string> SchacPersonalUniqueCode { get; set; }
        public string SchacHomeOrganization { get; set; }
        public string SchacHomeOrganizationType { get; set; }
        public string EduPersonAssurance { get; set; }
        public Dictionary<string, string> EduPersonAffiliation { get; set; }
        public List<string> Assurance_LOW { get; set; }
        public List<string> Assurance_MEDIUM { get; set; }
        public List<string> Assurance_HIGH { get; set; }

        public Dictionary<string, string> Prefixes {get;set;}


        public string [] GetUserProperties()
        {
            var props = new List<string>();
            if (!string.IsNullOrEmpty(EppnBase)) props.Add(EppnBase.ToLower());
            if (!string.IsNullOrEmpty(GivenName)) props.Add(GivenName.ToLower());
            if (!string.IsNullOrEmpty(Sn)) props.Add(Sn.ToLower());
            if (!string.IsNullOrEmpty(DisplayName)) props.Add(DisplayName.ToLower());
            if (!string.IsNullOrEmpty(Mail)) props.Add(Mail.ToLower());
            if (!string.IsNullOrEmpty(MailLocalAddress)) props.Add(MailLocalAddress.ToLower()) ;
            if (!string.IsNullOrEmpty(EduPersonOrcid)) props.Add(EduPersonOrcid.ToLower());
            if (!string.IsNullOrEmpty(NorEduPersonNIN)) props.Add(NorEduPersonNIN.ToLower());
            if (!string.IsNullOrEmpty(PersonalIdentityNumber)) props.Add(PersonalIdentityNumber.ToLower());
            if (!string.IsNullOrEmpty(SchacDateOfBirth)) props.Add(SchacDateOfBirth.ToLower());
            if (!string.IsNullOrEmpty(SchacHomeOrganization)) props.Add(SchacHomeOrganization.ToLower());
            if (!string.IsNullOrEmpty(SchacHomeOrganizationType)) props.Add(SchacHomeOrganizationType.ToLower());
            if (!string.IsNullOrEmpty(EduPersonAssurance)) props.Add(EduPersonAssurance.ToLower());
            //if (!string.IsNullOrEmpty()) props.Add();
            //if (!string.IsNullOrEmpty()) props.Add();
            if (SchacPersonalUniqueCode.Any())
            {
                foreach(var entry in SchacPersonalUniqueCode)
                {
                    props.Add(entry.Value);
                }
            }
            //if (!string.IsNullOrEmpty(SchacPersonalUniqueCode)) props.AddRange(SchacPersonalUniqueCode.Split(','));

            return props.Distinct().ToArray();
        }

        
    }
}
