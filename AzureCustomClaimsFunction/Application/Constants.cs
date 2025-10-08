using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedEntraToolkit.Application
{
    public static class Constants
    {
        public const string AFFILIATIONSTAFF = "staff";
        public const string AFFILIATIONFACULTY = "faculty";
        public const string AFFILIATIONEMPLOYEE = "employee";
        public const string AFFILIATIONMEMBER = "member";
        public const string AFFILIATIONSTUDENT = "student";
        public const string AFFILIATIONAFFILIATE = "affiliate";
        public const string AFFILIATIONALUM = "alum";
        public const string AFFILIATIONLIBRARYWALKIN = "library-walk-in";

        public const string ASSURANCE_LOW = "http://www.swamid.se/policy/assurance/al1,https://refeds.org/assurance,https://refeds.org/assurance/ID/unique,https://refeds.org/assurance/ID/eppn-unique-no-reassign,https://refeds.org/assurance/IAP/low,https://refeds.org/assurance/ATP/ePA-1m";
        public const string ASSURANCE_MEDIUM = "http://www.swamid.se/policy/assurance/al2,https://refeds.org/assurance/profile/cappuccino,https://refeds.org/assurance/IAP/medium,https://refeds.org/assurance/IAP/local-enterprise";
        public const string ASSURANCE_HIGH = "http://www.swamid.se/policy/assurance/al3,https://refeds.org/assurance/profile/espresso,https://refeds.org/assurance/IAP/high";
    }
}
