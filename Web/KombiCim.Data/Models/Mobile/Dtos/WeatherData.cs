using KombiCim.Data.Models.Arduino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Mobile.Dtos
{
    public class WeatherData
    {
        public List<WeatherDto> WeatherList { get; set; }
        public LocationDto Location { get; set; }
    }
}
