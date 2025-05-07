param allTags object
param functionAppName string
param functionAppConfig object
param location string



resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    siteconfig: functionAppConfig.appSettings
  }
}