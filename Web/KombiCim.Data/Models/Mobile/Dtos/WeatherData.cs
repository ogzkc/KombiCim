using Kombicim.Data.Models.Arduino.Dtos;

namespace Kombicim.Data.Models.Mobile.Dtos
{
    public class WeatherData
    {
        public List<WeatherDto> WeatherList { get; set; }
        public LocationDto Location { get; set; }
    }
}
