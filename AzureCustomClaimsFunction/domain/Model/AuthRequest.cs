using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedEntraToolkit.Domain.Model
{
    public class AuthRequest
    {
        public string Uid { get; set; }
        public string AppId { get; set; }
        public string AppName { get; set; }
    }
}
