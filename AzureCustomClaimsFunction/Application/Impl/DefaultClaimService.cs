using FedEntraToolkit.Application.Interface;
using FedEntraToolkit.Application.Settings;
using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph.Models;
using FedEntraToolkit.Domain.Model;
using System.Security.Cryptography;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Reflection.PortableExecutable;
namespace FedEntraToolkit.Application.Impl
{
    public class DefaultClaimService : ICustomClaimService
    {
        private readonly ILogger<DefaultClaimService> _logger;
        private GraphServiceClient _client;
        private ClaimSettings _settings;
        
        public DefaultClaimService(GraphServiceClient client, ClaimSettings settings, ILogger<DefaultClaimService> logger)
        {
            _client = client;
            _settings = settings;
            _logger = logger;
        }

        public ResponseObject GetAllClaims(AuthRequest authRequest)
        {
            //_logger.LogInformation("Configured use credentials " + Environment.GetEnvironmentVariable("UseCredentials"));
            //_logger.LogInformation("Configured tenant " + Environment.GetEnvironmentVariable("tenantId"));
            //_logger.LogInformation("Configured clientId " + Environment.GetEnvironmentVariable("clientId"));
            //_logger.LogInformation("Configured clientSecret " + Environment.GetEnvironmentVariable("clientSecret"));
            ResponseObject response = new ResponseObject();
            var claims = new Claims();
            var mgUser = GetMgUser(authRequest.Uid);
            var properties = mgUser.GetType().GetProperties();
            //user claims
            claims.EduPersonPrincipalName = GetEduPersonPrincipalName((string)GetAttributeValue(properties, mgUser,
               _settings.EppnBase));
            claims.GivenName = (string)GetAttributeValue(properties, mgUser,_settings.GivenName);
            claims.Sn = (string) GetAttributeValue(properties, mgUser,_settings.Sn);
            claims.DisplayName= (string)GetAttributeValue(properties, mgUser, _settings.DisplayName);
            claims.Mail= (string)GetAttributeValue(properties,mgUser,_settings.Mail);
            GetMailLocalAddress(properties, mgUser,_settings.MailLocalAddress,ref claims);
            claims.NorEduPersonNIN=(string)GetAttributeValue(properties, mgUser, _settings.NorEduPersonNIN);
            claims.PersonalIdentityNumber = (string)GetAttributeValue(properties, mgUser, _settings.PersonalIdentityNumber);


            //# Schac
            claims.SchacDateOfBirth = (string)GetAttributeValue(properties, mgUser, _settings.SchacDateOfBirth);
            claims.SchacHomeOrganization= (string)GetStaticAttributeValue(_settings.SchacHomeOrganization);
            claims.SchacHomeOrganizationType= (string)GetAttributeValue(properties, mgUser, _settings.SchacHomeOrganizationType);
            //Set ScacPersonalUniqueCode
            GetSchacPersonalUniqueCode(properties, mgUser, _settings.SchacPersonalUniqueCode, ref claims);

            //# Set EduPersonAssurance
            GetEduPersonAssurance(properties, mgUser, _settings.EduPersonAssurance,ref claims);

            //# Set Affiliation
            GetUserAffiliation(authRequest.Uid,_settings.EduPersonAffiliation,ref claims);

            response.data.actions[0].claims = claims;
            return response;
        }
        private string GetStaticAttributeValue(string attribute)
        {
            if(attribute == null)
                return null; 
            return attribute;
        }
        private object GetAttributeValue(PropertyInfo[] properties,User user, string attribute) {
            if (string.IsNullOrEmpty(attribute))
                return null;
            return properties.Where(p => p.Name.Equals(attribute, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().GetValue(user, null);
        }

        private string GetEduPersonPrincipalName(string userPrincipalName)
        {
            if (string.IsNullOrEmpty(userPrincipalName))
                return null;
            var regEx = new Regex("^(.+?)@", RegexOptions.IgnoreCase);
            Match m = regEx.Match(userPrincipalName);
            return string.Concat(m.Groups[1].Value,"@",_settings.Scope);

        }

        //TODO not implemented yet
        private void GetMailLocalAddress (PropertyInfo[] properties, User user, string attribute, ref Claims claims)
        {
            if (!string.IsNullOrEmpty(attribute))
            {
                var adresses = (List<string>)GetAttributeValue(properties, user, attribute);
            }
        }

        private void GetSchacPersonalUniqueCode(PropertyInfo[] properties, User user, Dictionary<string, string> attributes, ref Claims claims)
        {
            if (attributes != null) {
                if (attributes.Any())
                {
                    foreach(var item in attributes)
                    {
                        //lookup prefix
                        var prefix = _settings.Prefixes[item.Key];
                        claims.SchacPersonalUniqueCode.Add(string.Concat(prefix?.ToString(), (string)GetAttributeValue(properties, user, item.Value)));
                    }
                }
            }
        }

        void ICustomClaimService.Settings(ClaimSettings settings)
        {
            _settings = settings;
        }

        private User GetMgUser(string uid)
        {
            var user = this._client.Users[uid].GetAsync((config) =>
            {
                // Only request specific properties, check whats configured
                config.QueryParameters.Select = _settings.GetUserProperties();
            }).GetAwaiter().GetResult();
            return user;
        }

        private void GetEduPersonAssurance(PropertyInfo[] properties, User user, string attribute,ref Claims claims) 
        { 
            var assurance =(string) GetAttributeValue(properties,user,attribute);
            if (assurance != null)
                //_logger.LogInformation("Found assurance: " + assurance);
            {
                if (_settings.Assurance_LOW.Contains(assurance))
                {
                    claims.EduPersonAssurance.AddRange(GetValidAssuranceUris(Constants.ASSURANCE_LOW.Split(',').ToList()));
                }
                else if (_settings.Assurance_MEDIUM.Contains(assurance))
                {
                    claims.EduPersonAssurance.AddRange(GetValidAssuranceUris(Constants.ASSURANCE_LOW.Split(',').ToList()));
                    claims.EduPersonAssurance.AddRange(GetValidAssuranceUris(Constants.ASSURANCE_MEDIUM.Split(',').ToList()));
                }
                else if (_settings.Assurance_HIGH.Contains(assurance))
                {
                    claims.EduPersonAssurance.AddRange(GetValidAssuranceUris(Constants.ASSURANCE_LOW.Split(',').ToList()));
                    claims.EduPersonAssurance.AddRange(GetValidAssuranceUris(Constants.ASSURANCE_MEDIUM.Split(',').ToList()));
                    claims.EduPersonAssurance.AddRange(GetValidAssuranceUris(Constants.ASSURANCE_HIGH.Split(',').ToList()));
                }
                // cleanup duplicates, if any
                claims.EduPersonAssurance = claims.EduPersonAssurance.Distinct().ToList();
            }
        }

        private List<string> GetValidAssuranceUris(List<string> assurances)
        {
            var assuranceList = new List<string>();
            foreach(var uri in assurances)
            {
                if (ValidUri(uri)) {  assuranceList.Add(uri); }
            }
            return assuranceList;
        }

        private bool ValidUri(string uri)
        {
            return Uri.IsWellFormedUriString(uri,UriKind.Absolute);
        }

        private void GetUserAffiliation (string uid, Dictionary<string, string> affiliationSettings,ref Claims claims)
        {
            var groups = this._client.Users[uid].TransitiveMemberOf.GetAsync().GetAwaiter().GetResult();
            List<string> affiliations = new List<string>();
            
            foreach (var item in affiliationSettings)
            {
                var g = (from gg in groups.Value where( gg.Id == item.Value ) select gg).FirstOrDefault();
                if (g != null)
                {
                    affiliations.Add(item.Key.ToLower());
                }

                //Fix missing affiliations
                if ((affiliations.Contains(Constants.AFFILIATIONFACULTY)) || (affiliations.Contains(Constants.AFFILIATIONSTAFF)))
                {
                    if (!affiliations.Contains(Constants.AFFILIATIONEMPLOYEE))
                    {
                        affiliations.Add(Constants.AFFILIATIONEMPLOYEE);
                    }
                    if (!affiliations.Contains(Constants.AFFILIATIONMEMBER))
                    {
                        affiliations.Add(Constants.AFFILIATIONMEMBER);
                    }
                }

                if (affiliations.Contains(Constants.AFFILIATIONEMPLOYEE)){
                    if (!affiliations.Contains(Constants.AFFILIATIONMEMBER))
                    {
                        affiliations.Add(Constants.AFFILIATIONMEMBER);
                    }
                }

                if (affiliations.Contains(Constants.AFFILIATIONSTUDENT))
                {
                    if (!affiliations.Contains(Constants.AFFILIATIONMEMBER))
                    {
                        affiliations.Add(Constants.AFFILIATIONMEMBER);
                    }
                }

                if (affiliations.Contains(Constants.AFFILIATIONALUM))
                {
                    if (!affiliations.Contains(Constants.AFFILIATIONALUM))
                    {
                        affiliations.Add(Constants.AFFILIATIONALUM);
                    }
                }

                if (affiliations.Contains(Constants.AFFILIATIONAFFILIATE))
                {
                    if (!affiliations.Contains(Constants.AFFILIATIONAFFILIATE))
                    {
                        affiliations.Add(Constants.AFFILIATIONAFFILIATE);
                    }
                }

                if (affiliations.Contains(Constants.AFFILIATIONLIBRARYWALKIN))
                {
                    if (!affiliations.Contains(Constants.AFFILIATIONLIBRARYWALKIN))
                    {
                        affiliations.Add(Constants.AFFILIATIONLIBRARYWALKIN);
                    }
                }
            }

            if (affiliations.Any())
            {
                foreach (var affiliation in affiliations)
                {
                    claims.EduPersonAffiliation.Add(affiliation);
                    claims.EduPersonScopedAffiliation.Add(string.Concat(affiliation, "@",_settings.Scope)); 
                }
            }
            else
            {
                claims.EduPersonAffiliation = null;
                claims.EduPersonScopedAffiliation = null;
            }
        }

        private string GetEntityId(string appId)
        {
            var application= this._client.ApplicationsWithAppId(appId).GetAsync((config) =>
            {
                config.QueryParameters.Select = new[] { "appId", "identifierUris" };
                //config.Headers.Add("ConsistencyLevel", "eventual");
            }).GetAwaiter().GetResult();
            return application.IdentifierUris.FirstOrDefault();
        }
    }
}
