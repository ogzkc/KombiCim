using Kombicim.Data.Repository;
using Kombicim.APIShared.Attributes;
using Kombicim.Data.Models.Arduino.Responses;
using Microsoft.AspNetCore.Mvc;
using Kombicim.Data.Models.Arduino.Requests;

namespace Kombicim.Arduino.Controllers
{
    [Authentication]
    [ApiController]
    [Route("[controller]/[action]")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceRepository deviceRepository;

        public DeviceController(DeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        [HttpPost]
        public async Task<PostDeviceResponse> Post([FromBody] PostDeviceRequest request)
        {
            await deviceRepository.Post(request.DeviceId, request.TypeName);
            return new PostDeviceResponse();
        }
    }
}
