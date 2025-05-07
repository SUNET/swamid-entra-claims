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
    Scope = Environment.GetEnvironmentVariable("Scope"),
    O = Environment.GetEnvironmentVariable("Claim_O"),
    Co = Environment.GetEnvironmentVariable("Claim_Co"),
    GivenName = Environment.GetEnvironmentVariable("Claim_GivenName"),
    Sn = Environment.GetEnvironmentVariable("Claim_Sn"),
    DisplayName = Environment.GetEnvironmentVariable("Claim_DisplayName"),
    Mail = Environment.GetEnvironmentVariable("Claim_Mail"),
    MailLocalAddress = Environment.GetEnvironmentVariable("Claim_MailLocalAddress"),
    EduPersonOrcid = Environment.GetEnvironmentVariable("Claim_EduPersonOrcid"),
    NorEduPersonNIN = Environment.GetEnvironmentVariable("Claim_NorEduPersonNIN"),
    NorEduOrgAcronym = Environment.GetEnvironmentVariable("Claim_NorEduOrgAcronym"),
    PersonalIdentityNumber = Environment.GetEnvironmentVariable("Claim_PersonalIdentityNumber"),
    SchacDateOfBirth = Environment.GetEnvironmentVariable("Claim_ShacDateOfBirth"),
    SchacHomeOrganization = Environment.GetEnvironmentVariable("Claim_SchacHomeOrganization"),
    SchacHomeOrganizationType = Environment.GetEnvironmentVariable("Claim_SchacHomeOrganizationType"),
    SchacPersonalUniqueCode = Environment.GetEnvironmentVariable("Claim_SchacPersonalUniqueCode").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { '=' })).ToDictionary(x => x[0], y => y[1]),
    EduPersonAssurance = Environment.GetEnvironmentVariable("Claim_EduPersonAssurance"),
    EppnBase = Environment.GetEnvironmentVariable("Claim_EppnBase"),
    EduPersonAffiliation = Environment.GetEnvironmentVariable("Claim_EduPersonAffiliation").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { '=' })).ToDictionary(x => x[0], y => y[1]),
    Assurance_LOW = Environment.GetEnvironmentVariable("Assurance_LOW").Split(',').ToList(),
    Assurance_MEDIUM = Environment.GetEnvironmentVariable("Assurance_MEDIUM").Split(',').ToList(),
    Assurance_HIGH = Environment.GetEnvironmentVariable("Assurance_HIGH").Split(',').ToList()
};

foreach(DictionaryEntry di in Environment.GetEnvironmentVariables()){
    if (di.Key.ToString().StartsWith("Prefix_"))
    {
        claimSettings.Prefixes.Add(di.Key.ToString().Split("_").Last(), di.Value.ToString());
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

if (Environment.GetEnvironmentVariable("UseCredentials").ToLower()=="true")
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

