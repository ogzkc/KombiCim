using Kombicim.APIShared.Attributes;
using Kombicim.Data.Models.Arduino.Responses;
using Kombicim.Data.Models.Mobile.Requests;
using Kombicim.Data.Models.Mobile.Responses;
using Kombicim.Data.Repository;
using Kombicim.Data.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kombicim.Mobile.Controllers
{
    [Authentication]
    [ApiController]
    [Route("[controller]/[action]")]
    //[Authorize(Roles = Roles.ROLE_MOBILE_APP)]
    public class SettingsController : MobileApiController<SettingsController>
    {
        private readonly SettingRepository settingRepository;
        private readonly MinTemperatureRepository minTemperatureRepository;

        public SettingsController(IServiceProvider serviceProvider, SettingRepository settingRepository, MinTemperatureRepository minTemperatureRepository) : base(serviceProvider)
        {
            this.settingRepository = settingRepository;
            this.minTemperatureRepository = minTemperatureRepository;
        }

        [HttpGet]
        public async Task<SettingsResponse> Get(string deviceId)
        {
            return new SettingsResponse()
            {
                Settings = await settingRepository.GetActiveDto(deviceId)
            };
        }

        [HttpPost]
        public async Task<PostMinTemperatureResponse> SetMinTemperature([FromBody] PostMinTemperatureRequest request)
        {
            var success = await minTemperatureRepository.Set(request.ProfileId, request.LocationId, request.Temperature);
            return new PostMinTemperatureResponse()
            {
                Success = success
            };
        }

        [HttpPost]
        public async Task<PostMinTemperatureResponse> SetProfileMinTemperature([FromBody] PostProfileMinTemperatureRequest request)
        {
            var success = await minTemperatureRepository.SetProfile(request.ProfileId, request.Temperature);
            return new PostMinTemperatureResponse()
            {
                Success = success
            };
        }

        [HttpPost]
        public async Task<MobileBaseResponse> SetProfileThermometer([FromBody] SetProfileThermometerRequest request)
        {
            var success = await minTemperatureRepository.SetLocationByProfileId(request.ProfileId, request.LocationId);
            return new MobileBaseResponse()
            {
                Success = success
            };
        }

        //koray
        [HttpPost]
        public async Task<MobileBaseResponse> SetState([FromBody] SetStateRequest request)
        {
            await settingRepository.SetState(ApiUser.DeviceId, request.State);
            return new MobileBaseResponse();
        }

        [HttpGet]
        public async Task<GuidResponse> GetGuid(string deviceId)
        {
            return new GuidResponse()
            {
                Guid = await settingRepository.GetGuid(deviceId)
            };
        }
    }
}
