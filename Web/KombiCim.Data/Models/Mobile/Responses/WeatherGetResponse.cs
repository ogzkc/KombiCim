﻿using KombiCim.Data.Models.Mobile.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Mobile.Responses
{
    public class WeatherGetResponse : MobileBaseResponse
    {
        public List<WeatherData> WeatherDataList { get; set; }
    }
}
