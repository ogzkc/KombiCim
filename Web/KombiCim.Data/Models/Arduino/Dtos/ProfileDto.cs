using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
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

        public bool Active { get; set; }
    }
}
