using Kombicim.Data.Models.Arduino;
using Kombicim.Data.Repository;
using Kombicim.APIShared.Attributes;
using Kombicim.Data.Models.Arduino.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kombicim.Arduino.Controllers
{
    [Authentication]
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly SettingRepository settingRepository;

        public SettingsController(SettingRepository settingRepository)
        {
            this.settingRepository = settingRepository;
        }

        [HttpGet]
        public async Task<SettingsResponse> Get(string deviceId)
        {
            return new SettingsResponse()
            {
                Settings = await settingRepository.GetActiveDto(deviceId)
            };
        }

        [Route("[action]")]
        [HttpGet]
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

        [Route("[action]")]
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
