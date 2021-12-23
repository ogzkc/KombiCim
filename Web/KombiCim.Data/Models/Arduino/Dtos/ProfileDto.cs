using Newtonsoft.Json;

namespace Kombicim.Data.Models.Arduino.Dtos
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public string TypeName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? State { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? MinTempValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LocationDto SelectedThermometer { get; set; }

        public bool Active { get; set; }
    }
}
