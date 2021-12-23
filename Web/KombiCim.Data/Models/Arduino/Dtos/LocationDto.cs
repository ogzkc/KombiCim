using Newtonsoft.Json;

namespace Kombicim.Data.Models.Arduino.Dtos
{
    public class LocationDto
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceTypeName { get; set; }
        public string Name { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<MinTemperatureDto> MinTemperatures { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? MinTempValue { get; set; }
    }
}
