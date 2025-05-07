using FedEntraToolkit.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace FedEntraToolkit.Test
{
    public  class ClaimServiceTest :BaseTest
    {
        private ITestOutputHelper _helper;
        public ClaimServiceTest(ITestOutputHelper helper) :base()
        {
                _helper = helper;
        }

        [Fact]
        public void GetAllClaimsTest()
        {
            var authRequest = new AuthRequest()
            {
                AppId = "5a109f38-590a-4622-8269-322893a9eeaa",
                Uid = "5b73f6cf-3f3f-4b2b-b8e2-c63cd47af915"
            };
            var response = this._claimService.GetAllClaims(authRequest);
            Assert.Equal(4, response.data.actions[0].claims.EduPersonAffiliation.Count);
            Assert.NotNull(response);
            var claims = response.data.actions[0].claims;
            _helper.WriteLine("Sn: " + Print(claims.Sn));
            foreach(var affiliation in claims.EduPersonAffiliation)
            {
                _helper.WriteLine("EduPersonAffiliation: " + affiliation);
            }
            foreach (var s in claims.SchacPersonalUniqueCode)
            {
                _helper.WriteLine("SchacPersonalUniqueCode: " + s);
            }
            foreach(var a in claims.EduPersonAssurance)
            {
                _helper.WriteLine("EduPersonAssurance: " + a);
            }

        }

        private string Print(string input)
        {
            if (input == null) { return "null"; }
            return input;
        }
    }
}
