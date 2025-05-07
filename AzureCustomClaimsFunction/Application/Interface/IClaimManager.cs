using FedEntraToolkit.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedEntraToolkit.Application.Interface
{
    public interface IClaimManager
    {
        ResponseObject GetClaims(AuthRequest authRequest);
    }
}
