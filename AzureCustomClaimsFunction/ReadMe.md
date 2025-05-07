### Custom claims provider for entra

Install application as an Azure function

Create an app registration with listed permissions
- Directory.ReadAll
- Group.ReadAll
- GroupMember.ReadAll
- User.Read
- User.ReadAll
  
 Configure a 'Custom authentication extension' under Enterprise applications. Set up with the appregistration and the attributes in the supported attributes list below

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

Grupper som programmet tilldelats

Visningsnamn för endast moln-grupper

filtergrupper 
    visningsnamn
    innehåller
    affiliation.
Anpassa namn
    urn:oid:1.3.6.1.4.1.5923.1.1.1.9
    ^(?'name'\w+).(?'value'\w+)
    {value}@aticdmoutlook.onmicrosoft.com
  


