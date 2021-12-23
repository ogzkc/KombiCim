using Kombicim.Data.Repository;
using Kombicim.APIShared.Attributes;
using Microsoft.AspNetCore.Mvc;
using Kombicim.Data.Models.Arduino.Requests;
using Kombicim.Data.Models.Arduino.Responses;

namespace Kombicim.Arduino.Controllers
{
    [Authentication]
    [ApiController]
    [Route("[controller]/[action]")]
    public class LogController : ControllerBase
    {
        private readonly CombiLogRepository combiLogRepository;

        public LogController(CombiLogRepository combiLogRepository)
        {
            this.combiLogRepository = combiLogRepository;
        }

        [HttpPost]
        public async Task<BaseDeviceResponse> PostCombiLog([FromBody] PostCombiLogRequest request)
        {
            await combiLogRepository.Post(request.DeviceId, request.State);
            return new BaseDeviceResponse();
        }
    }
}