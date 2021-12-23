using Newtonsoft.Json;

namespace Kombicim.Data.Models.Mobile.Responses
{
    public class MobileBaseResponse
    {
        public bool Success { get; set; } = true;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }
    }
}
