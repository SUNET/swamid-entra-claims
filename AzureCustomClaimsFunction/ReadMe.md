### Custom claims provider for entra

Install application as an Azure function

Checkout code (from )
Run vscode, open folder AzureCustomClaimsFunction/AzureCustomClaims

If there's an existing function app to deploy to:
1. In the cmd text field, type Azure Functions: Deploy to function app
2. Choose Subscription
3. Choose Function App

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
	- Type the name of the function (EntraCustomTest3)
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

Create an app registration with listed permissions (EntraCustomClaimsClient)
- Directory.ReadAll
- Group.ReadAll
- GroupMember.ReadAll
- User.Read
- User.ReadAll
  
 Configure a 'Custom authentication extension' under Enterprise applications. 
 configure with the api-url from the function app (get function web address)
 
 Set up with the appregistration and the attributes in the supported attributes list below

Supported attributes:
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

Every attribute in the settings, thats not 'null' gets loaded and is available for issuance rules

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

  


