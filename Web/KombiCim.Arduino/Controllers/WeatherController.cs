using Kombicim.Data.Repository;
using Kombicim.APIShared.Attributes;
using Kombicim.Data.Models.Arduino.Responses;
using Microsoft.AspNetCore.Mvc;
using Kombicim.Data.Models.Arduino.Requests;

namespace Kombicim.Arduino.Controllers
{
    [Authentication]
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherRepository weatherRepository;

        public WeatherController(WeatherRepository weatherRepository)
        {
            this.weatherRepository = weatherRepository;
        }

        [HttpPost]
        public async Task<WeatherPostResponse> Post([FromBody] WeatherPostRequest request)
        {
            await weatherRepository.Post(request.DeviceId, request.Temperature, request.Humidity);
            return new WeatherPostResponse();
        }
    }
}
