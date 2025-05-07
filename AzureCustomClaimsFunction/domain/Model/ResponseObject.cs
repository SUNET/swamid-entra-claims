using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedEntraToolkit.Domain.Model
{
    public class ResponseObject
    {
        [JsonProperty("data")]
        public Data data { get; set; }
        public ResponseObject()
        {
            data = new Data();
        }
    }
}
