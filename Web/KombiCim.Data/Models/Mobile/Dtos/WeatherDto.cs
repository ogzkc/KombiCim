using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Mobile.Dtos
{
    public class WeatherDto
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime Date { get; set; }
    }
}
