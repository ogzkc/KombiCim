using System.ComponentModel.DataAnnotations;

namespace Kombicim.Data.Models.Arduino.Requests
{
    public class WeatherPostRequest : BaseDeviceRequest
    {
        [Required]
        public double Temperature { get; set; }
        [Required]
        public double Humidity { get; set; }
    }
}