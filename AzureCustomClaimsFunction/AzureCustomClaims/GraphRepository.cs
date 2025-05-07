using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedEntraToolkit
{
    public class GraphRepository
    {
        private GraphServiceClient _userClient;
        public GraphRepository() { }

        //var scopes = new[] { "User.Read" };

        // Multi-tenant apps can use "common",
        // single-tenant apps must use the tenant ID from the Azure portal
        //var tenantId = "common";

        // Value from app registration
        //var clientId = "YOUR_CLIENT_ID";

        

        // https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
        //var deviceCodeCredential = new DeviceCodeCredential(options);

        // var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);
        private void InitializeGraphForUserAuth()
        {
            //_settings = settings;

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Values from app registration
            var clientId = "";
            var tenantId = "";
            var clientSecret = "";
            //var clientCertificate = getCertificate(_settings.CertificateThumbprint);

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

            _userClient = new GraphServiceClient(tokenCredential, scopes);
        }
    }
    
}
