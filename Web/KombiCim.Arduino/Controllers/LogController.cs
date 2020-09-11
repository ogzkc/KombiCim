using KombiCim.Data.Models;
using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Repository;
using KombiCim.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace KombiCim.Arduino.Controllers
{
    [Authentication]
    [ModelValidation]
    public class LogController : ApiController
    {
        public async Task<BaseDeviceResponse> PostCombiLog([FromBody]PostCombiLogRequest request)
        {
            await CombiLogRepository.Post(request.DeviceId, request.State);
            return new BaseDeviceResponse();
        }
    }
}