using Azure.Identity;
using FedEntraToolkit.Application.Impl;
using FedEntraToolkit.Application.Interface;
using FedEntraToolkit.Application.Settings;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using Microsoft.Graph.Contacts.Item.MemberOf.GraphAdministrativeUnit;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Linq;
var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
//builder.Services
//  .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//  .AddMicrosoftIdentityWebApp(builder.Configuration, "AzureAd")
//      .EnableTokenAcquisitionToCallDownstreamApi()
//          .AddMicrosoftGraph(builder.Configuration.GetSection("GraphV1"))
//      .AddInMemoryTokenCaches();
builder.Services.AddMvc();
// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
//builder.Services.
//.AddApplicationInsightsTelemetryWorkerService()
//.ConfigureFunctionsApplicationInsights();
//LoadSettings



var claimSettings = new ClaimSettings()
{
    //Prefix_ESI = Environment.GetEnvironmentVariable("Prefix_ESI"),
    Scope = Environment.GetEnvironmentVariable("Scope") ?? string.Empty,
    O = Environment.GetEnvironmentVariable("Claim_O") ?? string.Empty,
    Co = Environment.GetEnvironmentVariable("Claim_Co")?? string.Empty,
    GivenName = Environment.GetEnvironmentVariable("Claim_GivenName")?? string.Empty,
    Sn = Environment.GetEnvironmentVariable("Claim_Sn")?? string.Empty,
    DisplayName = Environment.GetEnvironmentVariable("Claim_DisplayName")?? string.Empty,
    Mail = Environment.GetEnvironmentVariable("Claim_Mail")?? string.Empty,
    MailLocalAddress = Environment.GetEnvironmentVariable("Claim_MailLocalAddress")?? string.Empty,
    EduPersonOrcid = Environment.GetEnvironmentVariable("Claim_EduPersonOrcid")?? string.Empty,
    NorEduPersonNIN = Environment.GetEnvironmentVariable("Claim_NorEduPersonNIN")?? string.Empty,
    NorEduOrgAcronym = Environment.GetEnvironmentVariable("Claim_NorEduOrgAcronym")?? string.Empty,
    PersonalIdentityNumber = Environment.GetEnvironmentVariable("Claim_PersonalIdentityNumber")?? string.Empty,
    SchacDateOfBirth = Environment.GetEnvironmentVariable("Claim_ShacDateOfBirth")?? string.Empty,
    SchacHomeOrganization = Environment.GetEnvironmentVariable("Claim_SchacHomeOrganization")?? string.Empty,
    SchacHomeOrganizationType = Environment.GetEnvironmentVariable("Claim_SchacHomeOrganizationType")?? string.Empty,
    SchacPersonalUniqueCode = (Environment.GetEnvironmentVariable("Claim_SchacPersonalUniqueCode") ?? string.Empty)
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(s => s.Split(new[] { '=' }))
        .ToDictionary(x => x[0], y => y[1]),
    EduPersonAssurance = Environment.GetEnvironmentVariable("Claim_EduPersonAssurance")?? string.Empty,
    EppnBase = Environment.GetEnvironmentVariable("Claim_EppnBase")?? string.Empty,
    EduPersonAffiliation = (Environment.GetEnvironmentVariable("Claim_EduPersonAffiliation") ?? string.Empty)
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(s => s.Split(new[] { '=' }))
        .ToDictionary(x => x[0], y => y[1]),
    Assurance_LOW = (Environment.GetEnvironmentVariable("Assurance_LOW") ?? string.Empty).Split(',').ToList(),
    Assurance_MEDIUM = (Environment.GetEnvironmentVariable("Assurance_MEDIUM") ?? string.Empty).Split(',').ToList(),
    Assurance_HIGH = (Environment.GetEnvironmentVariable("Assurance_HIGH") ?? string.Empty).Split(',').ToList()
};

foreach(DictionaryEntry di in Environment.GetEnvironmentVariables()){
    if(di.Key != null) { 
        if (di.Key.ToString().StartsWith("Prefix_"))
        {
            claimSettings.Prefixes.Add(di.Key.ToString().Split("_").Last(), di.Value?.ToString() ?? string.Empty);
        }
    }
}

// GraphServiceClient
GraphServiceClient graphServiceClient = null;
var scopes = new[] { "https://graph.microsoft.com/.default" };
// using Azure.Identity;
var options = new ClientSecretCredentialOptions
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
};

var useCredentialsEnv = Environment.GetEnvironmentVariable("UseCredentials");
if (!string.IsNullOrEmpty(useCredentialsEnv) && useCredentialsEnv.ToLower() == "true")
{
    var tokenCredential = new ClientSecretCredential(
                Environment.GetEnvironmentVariable("tenantId"),
                Environment.GetEnvironmentVariable("clientId"),
                Environment.GetEnvironmentVariable("clientSecret"),
            options);
    graphServiceClient= new GraphServiceClient(tokenCredential, scopes);
}
else
{
    var cred = new ChainedTokenCredential(new ManagedIdentityCredential());
    graphServiceClient = new GraphServiceClient(cred, scopes);
}

builder.Services.AddSingleton<GraphServiceClient>(graphServiceClient);
builder.Services.AddSingleton<ClaimSettings>(claimSettings);
builder.Services.AddSingleton<ICustomClaimService, DefaultClaimService>();
builder.Services.AddTransient<IClaimManager, ClaimManager>();
var app = builder.Build();
//app.Logger
app.Run();

