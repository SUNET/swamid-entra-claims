Connect-AzAccount -Tenant 3d26d12e-567d-4d71-aaa3-058735dda11e

Get-AzFunctionApp -SubscriptionId bd891b58-83d1-428c-9dcd-1cc2e405dec9


#Get-AzFunctionApp -SubscriptionId bd891b58-83d1-428c-9dcd-1cc2e405dec9 -Name EntraClaimStub -ResourceGroupName ADFSToolkit4Entra
#Get-AzFunctionAppSetting -SubscriptionId bd891b58-83d1-428c-9dcd-1cc2e405dec9 -Name EntraClaimStub -ResourceGroupName ADFSToolkit4Entra
Get-AzFunctionApp -SubscriptionId bd891b58-83d1-428c-9dcd-1cc2e405dec9 -Name EntraCustomClaimFunction -ResourceGroupName ADFSToolkit4Entra
Get-AzFunctionAppSetting -SubscriptionId bd891b58-83d1-428c-9dcd-1cc2e405dec9 -Name EntraCustomClaimFunction -ResourceGroupName ADFSToolkit4Entra

#//fyll på efter 'x' = 'y' med parametrar
$config =@{
    'APPLICATIONINSIGHTS_CONNECTION_STRING' = 'InstrumentationKey=7c0349cb-20cd-4939-89c9-c9de53ffe66f;IngestionEndpoint=https://northeurope-2.in.applicationinsights.azure.com/;LiveEndpoint=https://northeurope.livediagnostics.monitor.azure.com/;ApplicationId=4df33b23-7c85-4d6b-a698-ca9e69793e02'
    'AzureWebJobsStorage' = 'DefaultEndpointsProtocol=https;AccountName=adfstoolkit4entra9294;AccountKey=U9X/ZRVwMvJWeDaxkL1ylYKIWSdEd6co8lW6csiuuOctKozTRg6r2rKJjCP+nVF3BKSziaFRDjFq+AStKLonUQ==;EndpointSuffix=core.windows.net';
    'FUNCTIONS_EXTENSION_VERSION' = '~4';
    'FUNCTIONS_INPROC_NET8_ENABLED' = '1';
    'FUNCTIONS_WORKER_RUNTIME' = 'dotnet';
    'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING' = 'DefaultEndpointsProtocol=https;AccountName=adfstoolkit4entra9294;AccountKey=U9X/ZRVwMvJWeDaxkL1ylYKIWSdEd6co8lW6csiuuOctKozTRg6r2rKJjCP+nVF3BKSziaFRDjFq+AStKLonUQ==;EndpointSuffix=core.windows.net';
    'WEBSITE_CONTENTSHARE' = 'entraclaimstubac14';
    'x' = 'y';
}

Update-AzFunctionAppSetting -Name 'EntraClaimStub' -ResourceGroupName 'ADFSToolkit4Entra'  -SubscriptionId 'bd891b58-83d1-428c-9dcd-1cc2e405dec9' -AppSetting $config -Force

#$settings = Get-Content "C:\Users\toylon98\Source\Repos\entracustomclaims\AzureCustomClaimsFunction\Test\appsettings.json"


#az login 
#az deployment sub create --template-file C:\Users\toylon98\Source\Repos\entracustomclaims\AzureCustomClaimsFunction\Modules\deployResourceGroup.bicep --parameters C:\Users\toylon98\Source\Repos\entracustomclaims\AzureCustomClaimsFunction\Modules\deployResourceGroup.json -l northeurope