# Custom claims provider for entra

## Install application as an Azure function

Checkout code (from github)

Run vscode, open folder AzureCustomClaimsFunction/AzureCustomClaims

Make sure the following is installed in vscode
- Azure Tools

Make sure the following is installed on computer
- Azure Functions Core (https://github.com/Azure/azure-functions-core-tools)

### Existing function
If there's an existing function app to deploy to:
1. In the cmd text field, type Azure Functions: Deploy to function app
2. Choose Subscription
3. Choose Function App

### New Function

No existing function app to deploy to. step 1-2 above

4. Choose '> Create new function app'
5. Type the name for the new function
6. Choose location
7. Choose Runtime stack -> .Net 8 Isolated
8. Select resource authentication type -> Managed Identity

Create function app in Azure Portal
1. Choose create functionapp, choose type of hosting (flex is pre selected)
2. Settings
	- Select or create resourcegroup
	- Type the name of the function (EntraCustomClaimsFunction)
	- Choose Region (North Europe)
	- Clr-stack -> .NET
	- Version -> 8 LTS
	- Instance size , 2048 MB
	- Redundance, optional
3. Storage, reuse or create new
4. Azure OpenAI, skip and next
5. Network
	- Activate public access, on
	- Activate virtual network, off (on if you're using a vn)
6. Monitoring, Application insights. New or existing, select region
7. Distribution, ignore
8. leave default
9. Tags, default
10. Review and create

## Access Rights to GraphAPI
The function can be run with managed identity or credentials from a app registration

Create an app registration with listed permissions (EntraCustomClaimsClient)
- Directory.ReadAll
- Group.Read.All
- GroupMember.Read.All
- User.Read
- User.Read.All
  
Otherwise, set up the managed identity with the same rights

## Configure Custom authentication extension
Go to Enterprise application/custom authentication extensions and choose new...

You should see the event TokenIssuanceStart, click next.  
Give it a name (EntraCustomClaimsExtension), configure with the api-url from the function app (get function web address from EntraCustomClaimsClient) and next.

Create a new app registration (api) between the extension and functionapp, easiest to set up a new one in the guide  
Name (EntraClaimProviderFunctionAPI), this gets the permission https://graph.microsoft.com/CustomAuthenticationExtension.Receive.Payload  
if using an existing, check that it has the same permissions

Configure the attributes that later will be available when setting up a relying party  
Choose from the list of supported attributs below:

### Supported attributes:
- O
- Co
- GivenName
- Sn
- DisplayName
- Mail
- MailLocalAddress
- PersonalIdentityNumber
- EduPersonOrcId
- EduPersonAssurance
- EduPersonPrincipalName
- EduPersonAffiliation
- EduPersonScopedAffiliation
- NorEduPersonNIN
- NorEduOrgAcronym
- SchacDateOfBirth
- SchacHomeOrganization
- SchacHomeOrganizationType
- SchacPersonalUniqueCode
  
## Configure environment variables for the Function app
After install you will see the stuff that gets added automatically (the ones above 'scope')  
Just add the rest. Every attribute in the settings, thats not 'null' gets 'loaded' and is later available for issuance rules.

 - UseCredentials. Rights for GraphAPI set true for app registration, enter ClientId and ClientSecret, false for Managed Identity  
 - Claim_EduPersonAffiliation, all types from the spec see example for staff and student mapped to a group id  
 - Assurance_HIGH/MEDIUM/LOW. Set expected value for the attribute thats expected for the different levels (fetched from 'Claim_EduPersonAssurance'),  
   multiple values allowed like below
 - Scope. Domain name, used to construct EduPersonPrincipalName and EduPersonScopedAffiliation

```json
´´´
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "AuthenticationEvents__TenantId": "3d26d12e-567d-4d71-aaa3-058735dda11e",
    "AuthenticationEvents__AudienceAppId": "api://5a109f38-590a-4622-8269-322893a9eeaa",
    "AuthenticationEvents__CustomCallerAppId": "5a109f38-590a-4622-8269-322893a9eeaa",
    "Scope": "aticdmoutlook.onmicrosoft.com",
    "Prefix_ESI": "urn:schac:personalUniqueCode:int:esi:ladok.se:externtstudentuid-",
    "Assurance_HIGH":"3,AL3,http://www.swamid.se/policy/assurance/al3",
    "Assurance_MEDIUM":"2,AL2,http://www.swamid.se/policy/assurance/al2",
    "Assurance_LOW":"1,AL1,http://www.swamid.se/policy/assurance/al1",
    "Claim_O": null,
    "Claim_EppnBase": "UserPrincipalName",
    "Claim_SubjectID": null,
    "Claim_PairwiseID": null,
    "Claim_GivenName": "givenname",
    "Claim_Sn": "surname",
    "Claim_DisplayName": "displayname",
    "Claim_Mail": "mail",
    "Claim_MailLocalAddress": null,
    "Claim_EduPersonOrcid": null,
    "Claim_NorEduPersonNIN": null,
    "Claim_NorEduOrgAcronym": null,
    "Claim_PersonalIdentityNumber": null,
    "Claim_ShacDateOfBirth": null,
    "Claim_SchacHomeOrganization": "aticdmoutlook.onmicrosoft.com",
    "Claim_SchacHomeOrganizationType": null,
    "Claim_SchacPersonalUniqueCode": null,
    "Claim_EduPersonAssurance": null,
    "Claim_EduPersonAffiliation": "staff=c3cf02a5-1309-44d6-8170-4df37ef81169,student=0185c2d6-f0a1-400e-a3cd-3a2f9bb25948",
    "UseCredentials": "true",
    "TenantId": "<TenantId>",
    "ClientId": "<ClientId>",
    "ClientSecret": "<ClientSecret>"
  }
}
´´´
```

## Summary
After completing these steps you will have:  
 - EntraCustomClaimsFunction  Function app with code from GitHub  
 - EntraCustomClaimsClient, app registration for accessing GraphAPI 
 - EntraCustomClaimsExtension, Custom authentication extension using the function  
 - EntraClaimProviderFunctionAPI, app registration that enables the handling login events through the extension to the function
  


