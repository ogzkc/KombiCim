using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
{
    public class ProfilePreferenceDto
    {
        public int ProfileId { get; set; }
        public int? DayOfWeek { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
    }
}
