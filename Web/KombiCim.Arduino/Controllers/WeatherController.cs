using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Repository;
using KombiCim.Data.Utilities;
using KombiCim.Filters;
using KombiCim.Utilities;
using KombiCim.Data.Models.Arduino.Responses;

namespace KombiCim.Arduino.Controllers
{
    [Authentication]
    [ModelValidation]
    public class WeatherController : ApiController
    {
        public async Task<WeatherPostResponse> Post([FromBody]WeatherPostRequest request)
        {
            using (var db = new KombiCimEntities())
            {
                await WeatherRepository.Post(request.DeviceId, request.Temperature, request.Humidity, db);
                return new WeatherPostResponse();
            }
        }
    }
}
