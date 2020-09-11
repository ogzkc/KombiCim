
using KombiCim.Data.Models;
using KombiCim.Data.Models.Arduino.Responses;
using KombiCim.Data.Models.Mobile.Requests;
using KombiCim.Data.Models.Mobile.Responses;
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

namespace KombiCim.Controllers
{
    [Authentication]
    [ModelValidation]
    [TokenValidation]
    [Authorize(Roles = Roles.ROLE_MOBILE_APP)]
    public class SettingsController : BaseApiController
    {
        public async Task<SettingsResponse> Get(string deviceId)
        {
            return new SettingsResponse()
            {
                Settings = await SettingRepository.GetActiveDto(deviceId)
            };
        }


        public async Task<PostMinTemperatureResponse> SetMinTemperature([FromBody]PostMinTemperatureRequest request)
        {
            var success = await MinTemperatureRepository.Set(request.ProfileId, request.LocationId, request.Temperature, request.DayOfWeek, request.Hour, request.Minutes);
            return new PostMinTemperatureResponse()
            {
                Success = success
            };
        }


        //koray
        public async Task<PostMinTemperatureResponse> SetProfileMinTemperature([FromBody]PostProfileMinTemperatureRequest request)
        {
            var success = await MinTemperatureRepository.SetProfile(request.ProfileId, request.Temperature, request.DayOfWeek, request.Hour, request.Minutes);
            return new PostMinTemperatureResponse()
            {
                Success = success
            };
        }

        //koray
        public async Task<MobileBaseResponse> SetState([FromBody]SetStateRequest request)
        {
            await SettingRepository.SetState(ApiUser.OwnedDeviceId, request.State);
            return new MobileBaseResponse();
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
