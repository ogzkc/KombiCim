using Newtonsoft.Json;

namespace Kombicim.Data.Models.Arduino.Responses
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
