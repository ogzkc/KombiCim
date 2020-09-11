using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
{
    public class BaseResponse
    {
        public BaseResponse()
        {

        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ValidationErrors { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }
    }
}
