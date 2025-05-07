using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedEntraToolkit.Domain.Model
{
    public class Claims
    {
        public Claims()
        {
            EduPersonAffiliation = new List<string>();
            EduPersonScopedAffiliation = new List<string>();
            SchacPersonalUniqueCode = new List<string>();
            EduPersonAssurance = new List<string>();
        }

        public string EduPersonPrincipalName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? GivenName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Sn { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? DisplayName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Mail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> MailLocalAddress { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? NorEduPersonNIN { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? PersonalIdentityNumber{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? SchacDateOfBirth { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? SchacHomeOrganization { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? SchacHomeOrganizationType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SchacPersonalUniqueCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> EduPersonAssurance { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> EduPersonAffiliation { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> EduPersonScopedAffiliation { get; set; }
        

    }
}
