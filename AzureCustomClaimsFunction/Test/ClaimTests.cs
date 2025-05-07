using FedEntraToolkit.Application;
using FedEntraToolkit.Test;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;

namespace Test
{
    public class ClaimTests : BaseTest
    {
        private const string uid = "5b73f6cf-3f3f-4b2b-b8e2-c63cd47af915";
       
        public ClaimTests():base() { }

        [Fact]
        public void GetEntityIdTest()
        {
            var appId = "5a109f38-590a-4622-8269-322893a9eeaa";
            var foo = this._client.ApplicationsWithAppId(appId).GetAsync((config) =>
            {
                config.QueryParameters.Select = new[] { "appId", "identifierUris" };
            }).GetAwaiter().GetResult();
            var entityId = foo.IdentifierUris.FirstOrDefault();

            var fo = "";

            
        }

        [Fact]
        public void GetUserTest()
        {
            var user =  this._client.Users[uid].GetAsync((config) =>
            {
                // Only request specific properties
                config.QueryParameters.Select = this.GetClaimSettings().GetUserProperties();

                //Example
                //config.QueryParameters.Select = new[] {
                //    "UserPrincipalName",
                //    "GivenName",
                //    "SurName",
                //    "DisplayName",
                //    "OnPremisesImmutableId",
                //    "UserPrincipalName",
                //    "OnPremisesLastSyncDateTime",
                //    "UsageLocation",
                //    "Id",
                //    "ProxyAddresses",
                //};

            }).GetAwaiter().GetResult();

            Assert.NotNull(user.UserPrincipalName);
            var pp = user.GetType().GetProperties();
            var dd = pp.Where(p => p.Name.ToLower() == "userprincipalname").First().GetValue(user, null);
            
        }
        [Fact]
        public void GetMemberOfTest()
        {
            

            var groups = this._client.Users[uid].TransitiveMemberOf.GetAsync().GetAwaiter().GetResult();
            var settings = GetClaimSettings();
            List<string> affliliations = new List<string>();
            foreach (var item in settings.EduPersonAffiliation)
            {
                var g = groups.Value.Select(d=>d.Id == item.Value).FirstOrDefault();
                if(g!=null)
                {
                    affliliations.Add(item.Key.ToLower());
                }
            }

            //Fix missing
            if((affliliations.Contains(Constants.AFFILIATIONFACULTY ))|| (affliliations.Contains(Constants.AFFILIATIONSTAFF)))  {
                if(!affliliations.Contains(Constants.AFFILIATIONEMPLOYEE)) {
                    affliliations.Add(Constants.AFFILIATIONEMPLOYEE);
                }
                if (!affliliations.Contains(Constants.AFFILIATIONMEMBER))
                {
                    affliliations.Add(Constants.AFFILIATIONMEMBER);
                }
            }
            Assert.NotNull (groups);
        }

        

    }
}