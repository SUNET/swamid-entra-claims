// This bicep file is used to deploy a Resource Group, a System Topic with Subscriptions and a Function App to an Azure subscription.
// The resources that are deployed by the modules would be defined in the respective bicep files (topicAndSubscriptionModule.bicep and functionAppModule.bicep).
// It is assumed that the following resources are already created in Azure:
// the common Service Bus, the Topic 'message-inbox', the common runtime Storage Account for function apps and a common App Service Plan.
// Use the deployFunctionAppParametersTemplate.json to create a parameters.json file and set all values. Make sure you are logged in to the correct subscription,
// then run the command 'az deployment sub create --template-file {relative path}/deployFunctionApp.bicep --parameters @{relative path}/parameters.json -l SwedenCentral' to deploy the resources.
// You can also shorten it to 'az deployment sub create --parameters @{relative path}/parameters.bicepparam -l SwedenCentral' if the bicepparam file points to this bicep file in its using statement.


targetScope = 'subscription'

// Hardcoded names and tags
param location string = 'swedencentral'
param hardTagsObject object = {
    hardTags: {
      Owner: 'Sunet'
      UpdatedUtc: utcNow('yyyy-MM-dd HH:mm:ss')
    }
  }
// Names and tags from parameter file
param parameterTags object
param allTags object = union(hardTagsObject, parameterTags)
param resourceGroupName string
param resourceGroupLocation string
param functionAppName string
param functionAppConfig object

//functionAppConfig.appSettings
resource resourceGroup 'Microsoft.Resources/resourceGroups@2023-07-01' = {
    name: resourceGroupName
    location: resourceGroupLocation  
    tags: union(allTags.hardTags, allTags.commonTags, {
      Description: allTags.descriptionTags.ResourceGroupDescription
    })
    //dependsOn: [
    //  topicAndSubscription
    //]
  }

  module functionApp 'Modules/functionAppModule.bicep' = {
    name: 'functionAppModule'
    scope: resourceGroup
    params: {
      allTags: allTags
      functionAppName: functionAppName
      functionAppConfig: functionAppConfig
      location: location
      //hostingPlanName: hostingPlanName
      //functionAppName: functionAppName
      //serviceBusName: serviceBusName
      //systemTopicName: systemTopicName
      //#systemSubscriptionName: systemSubscriptionName
      //listenAccessKeyName: listenAccessKeyName
      //commonResourceGroupName: commonResourceGroupName
    }
  }
