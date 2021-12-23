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
    [Route("[controller]")]
    //[Authorize(Roles = Roles.ROLE_MOBILE_APP)]
    public class WeatherController : MobileApiController<WeatherController>
    {
        private readonly WeatherRepository weatherRepository;

        public WeatherController(IServiceProvider serviceProvider, WeatherRepository weatherRepository) : base(serviceProvider) 
        {
            this.weatherRepository = weatherRepository;
        }

        [HttpGet]
        public async Task<WeatherGetResponse> Get(int lastHours = 6)
        {
            return new WeatherGetResponse()
            {
                WeatherDataList = await weatherRepository.GetAll(ApiUser.Id, ApiUser.DeviceId, lastHours: lastHours)
            };
        }
    }
}
