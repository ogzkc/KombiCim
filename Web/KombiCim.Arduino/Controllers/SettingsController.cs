using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Repository;
using KombiCim.Data.Utilities;
using KombiCim.Filters;
using KombiCim.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using KombiCim.Data.Models.Arduino.Responses;

namespace KombiCim.Arduino.Controllers
{
    [Authentication]
    [ModelValidation]
    public class SettingsController : ApiController
    {
        public async Task<SettingsResponse> Get(string deviceId)
        {
            using (var db = new KombiCimEntities())
            {
                return new SettingsResponse()
                {
                    Settings = await SettingRepository.GetActiveDto(deviceId, db)
                };
            }
        }

        public DateTimeCustom GetTime()
        {
            var now = DateTime.Now;
            return new DateTimeCustom()
            {
                Year = now.Year,
                Month = now.Month,
                Day = now.Day,
                Hour = now.Hour,
                Minute = now.Minute,
                Second = now.Second
            };
        }

        public async Task<GuidResponse> GetGuid(string deviceId)
        {
            return new GuidResponse()
            {
                Guid = await SettingRepository.GetGuid(deviceId)
            };
        }
    }
}
