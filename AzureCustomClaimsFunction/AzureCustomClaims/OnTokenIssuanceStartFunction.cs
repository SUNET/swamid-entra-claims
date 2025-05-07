using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;
using FedEntraToolkit.Domain.Model;
using FedEntraToolkit.Application.Interface;

namespace FedEntraToolkit
{
    public class OnTokenIssuanceStartFunction
    {
        private readonly ILogger<OnTokenIssuanceStartFunction> _logger;
        private readonly IClaimManager _manager;

        public OnTokenIssuanceStartFunction(IClaimManager manager, ILogger<OnTokenIssuanceStartFunction> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [Function("OnTokenIssuanceStartFunction")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            //var envVar = Environment.GetEnvironmentVariable("EduPersonAffiliation");
            var authRequest = new AuthRequest();
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string correlationId = data?.data.authenticationContext.correlationId;
            authRequest.Uid = data?.data.authenticationContext.user.id;
            authRequest.AppId = data?.data.authenticationContext.clientServicePrincipal.appId;
            authRequest.AppName = data?.data.authenticationContext.clientServicePrincipal.appDisplayName;

            _logger.LogDebug("REQUEST");

            _logger.LogDebug(requestBody);

            //Check if setup is ok
            //if(_manager != null)
            //{
            //    _logger.LogInformation("Manager loaded");
            //}
            
            //Responseobject to be converted to json
            ResponseObject r = null;
            try
            {
                r = _manager.GetClaims(authRequest);
                
                //r = new ResponseObject();
                //.data.actions[0].claims.CorrelationId = correlationId;
                //r.data.actions[0].claims.ApiVersion = "1.0.0";
                //r.data.actions[0].claims.DateOfBirth = "01/01/2000";
                //r.data.actions[0].claims.EduPersonScopedAffiliation.Add("member@aticdmoutlook.onmicrosoft.com");
                //r.data.actions[0].claims.EduPersonScopedAffiliation.Add("employee@aticdmoutlook.onmicrosoft.com");
                //r.data.actions[0].claims.EduPersonScopedAffiliation.Add("staff@aticdmoutlook.onmicrosoft.com");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
            
            var c = new ContentResult();
            c.ContentType = "application/json";
            c.Content = JsonConvert.SerializeObject(r);
            _logger.LogInformation("Response: " + c.Content);
            _logger.LogInformation("END");
            return c;

        }
        
    }
    
   
   
   
}
