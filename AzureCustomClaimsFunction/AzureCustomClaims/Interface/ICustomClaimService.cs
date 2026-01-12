using FedEntraToolkit.Settings;
using FedEntraToolkit.Model;
using Microsoft.Graph.Models;
namespace FedEntraToolkit.Interface
{
    public interface ICustomClaimService
    {
        
        //User GetGraphUser(string userId);
        //void GetEduPersonAffiliation(string uid);
        ResponseObject GetAllClaims(AuthRequest authRequest);
        void Settings(ClaimSettings claimSettings);
    }
}
