using Newtonsoft.Json;

namespace Kombicim.Data.Models.Arduino.Dtos
{
    public class MinTemperatureDto
    {
        public double Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? DayOfWeek { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Hour { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Minute { get; set; }
    }
}
