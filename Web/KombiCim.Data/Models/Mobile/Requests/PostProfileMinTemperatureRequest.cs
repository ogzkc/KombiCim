using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Mobile.Requests
{
    public class PostProfileMinTemperatureRequest : MobileBaseRequest
    {
        [Required]
        public double Temperature { get; set; }

        [Required]
        public int ProfileId { get; set; }

        public int? DayOfWeek { get; set; }
        public int? Hour { get; set; }
        public int? Minutes { get; set; }
    }
}
