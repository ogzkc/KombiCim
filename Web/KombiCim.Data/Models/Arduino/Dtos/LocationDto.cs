using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
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
