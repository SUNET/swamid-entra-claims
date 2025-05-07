using FedEntraToolkit.Application.Interface;
using FedEntraToolkit.Application.Interface;
using FedEntraToolkit.Application.Settings;
using FedEntraToolkit.Domain.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedEntraToolkit.Application.Impl
{
    
    public class ClaimManager : IClaimManager
    {
        private readonly ILogger<ClaimManager> _logger;
        private readonly ClaimSettings _claimSettings;
        private ICustomClaimService _claimService;
        public ClaimManager(ICustomClaimService claimService, ILogger<ClaimManager> logger)
        {
            _logger = logger;
            _claimService = claimService;
        }


        public ResponseObject GetClaims(AuthRequest authRequest)
        {
            //setup attributes to fetch
            var claims = _claimService.GetAllClaims(authRequest);
            return claims;
        }
    }
}
