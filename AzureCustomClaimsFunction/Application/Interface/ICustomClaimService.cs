using FedEntraToolkit.Application.Settings;
using FedEntraToolkit.Domain.Model;
using Microsoft.Graph.Models;
namespace FedEntraToolkit.Application.Interface
{
    public interface ICustomClaimService
    {
        
        //User GetGraphUser(string userId);
        //void GetEduPersonAffiliation(string uid);
        ResponseObject GetAllClaims(AuthRequest authRequest);
        void Settings(ClaimSettings claimSettings);
    }
}
