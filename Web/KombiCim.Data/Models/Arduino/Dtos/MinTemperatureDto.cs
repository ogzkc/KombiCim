using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
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
