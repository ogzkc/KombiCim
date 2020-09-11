using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Mobile.Requests
{
    public class WeatherGetRequest : MobileBaseRequest
    {
        public int LastHours { get; set; } = 6;
    }
}
