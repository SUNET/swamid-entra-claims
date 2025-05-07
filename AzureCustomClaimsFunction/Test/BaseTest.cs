using FedEntraToolkit.Application.Impl;
using FedEntraToolkit.Application.Interface;
using FedEntraToolkit.Application.Settings;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace FedEntraToolkit.Test
{
    public class BaseTest
    {
        public ICustomClaimService _claimService;
        public  IConfiguration _configuration;
        public GraphServiceClient _client;

        public BaseTest()
        {
            GetIConfigurationRoot();
            _client = GetGraphClient();
            //ILogger logger = new ILogger<BaseTest>();
            _claimService = new DefaultClaimService(_client, GetClaimSettings(),null);
            _claimService.Settings(GetClaimSettings());
        }

        public void GetIConfigurationRoot()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
            .Build();
        }
        private GraphServiceClient GetGraphClient()
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Values from app registration
            var clientId = _configuration.GetValue<string>("Values:ClientId"); 
            var tenantId = _configuration.GetValue<string>("Values:TenantId");
            var clientSecret = _configuration.GetValue<string>("Values:ClientSecret");

            // Claim Settings
            

            // using Azure.Identity;
            var options = new ClientSecretCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,

            };
            var tokenCredential = new ClientSecretCredential(
                tenantId,
                clientId,
                clientSecret,
            options);
            //var clientCertCredential = new ClientCertificateCredential(
            //    tenantId, clientId, clientCertificate, options);

            return new GraphServiceClient(tokenCredential, scopes);
        }

        public ClaimSettings GetClaimSettings()
        {
            var claimSettings = new ClaimSettings();
            //claimSettings.Prefix_ESI = _configuration.GetValue<string>("Values:Prefix_ESI");
            claimSettings.Scope = _configuration.GetValue<string>("Values:Scope");
            claimSettings.O = _configuration.GetValue<string>("Values:Claim_O");
            claimSettings.Co = _configuration.GetValue<string>("Values:Claim_Co");
            claimSettings.GivenName = _configuration.GetValue<string>("Values:Claim_GivenName");
            claimSettings.Sn = _configuration.GetValue<string>("Values:Claim_Sn");
            claimSettings.DisplayName = _configuration.GetValue<string>("Values:Claim_DisplayName");
            claimSettings.Mail = _configuration.GetValue<string>("Values:Claim_Mail");
            claimSettings.MailLocalAddress = _configuration.GetValue<string>("Values:Claim_MailLocalAddress");
            claimSettings.EduPersonOrcid = _configuration.GetValue<string>("Values:Claim_EduPersonOrcid");
            claimSettings.NorEduPersonNIN = _configuration.GetValue<string>("Values:Claim_NorEduPersonNIN");
            claimSettings.NorEduOrgAcronym = _configuration.GetValue<string>("Values:Claim_NorEduOrgAcronym");
            claimSettings.PersonalIdentityNumber = _configuration.GetValue<string>("Values:Claim_PersonalIdentityNumber");
            claimSettings.SchacDateOfBirth = _configuration.GetValue<string>("Values:Claim_ShacDateOfBirth");
            claimSettings.SchacHomeOrganization = _configuration.GetValue<string>("Values:Claim_SchacHomeOrganization");
            claimSettings.SchacHomeOrganizationType = _configuration.GetValue<string>("Values:Claim_SchacHomeOrganizationType");
            //claimSettings.SchacPersonalUniqueCode = _configuration.GetValue<string>("Values:Claim_SchacPersonalUniqueCode");
            claimSettings.EduPersonAssurance = _configuration.GetValue<string>("Values:Claim_EduPersonAssurance");
            claimSettings.EppnBase= _configuration.GetValue<string>("Values:Claim_EppnBase");
            claimSettings.EduPersonAffiliation = _configuration.GetValue<string>("Values:Claim_EduPersonAffiliation").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { '=' })).ToDictionary(x => x[0], y => y[1]);
            
            //claimSettings.O = _configuration.GetValue<string>("Values:ClientId");
            //claimSettings.O = _configuration.GetValue<string>("Values:ClientId");
            return claimSettings;
        }

        //public static MyAppConfig GetMyAppConfiguration()
        //{
        //    var configuration = new MyAppConfig();
        //    GetIConfigurationRoot().Bind("MyApp", configuration);
        //    return configuration;
        //}
    }
}
