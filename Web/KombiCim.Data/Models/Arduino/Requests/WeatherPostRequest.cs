using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KombiCim.Data.Models.Arduino
{
    public class WeatherPostRequest : BaseDeviceRequest
    {
        [Required]
        public double Temperature { get; set; }
        [Required]
        public double Humidity { get; set; }
    }
}