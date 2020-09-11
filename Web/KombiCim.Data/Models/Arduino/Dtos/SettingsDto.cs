using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
{
    public class SettingsDto
    {
        public string ProfileName { get; set; }
        public string Mode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? MinTemperature { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<MinTemperatureDto> MinTemperatureDtos { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ProfileDto> ProfileDtos { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ProfilePreferenceDto> ProfilePreferenceDtos { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? State { get; set; }

        public string Guid { get; set; }
    }
}
