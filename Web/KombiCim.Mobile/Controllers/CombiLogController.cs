using Kombicim.APIShared.Attributes;
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
    public class CombiLogController : MobileApiController<CombiLogController>
    {
        private readonly CombiLogRepository combiLogRepository;

        public CombiLogController(IServiceProvider serviceProvider, CombiLogRepository combiLogRepository) : base(serviceProvider)
        {
            this.combiLogRepository = combiLogRepository;
        }

        [HttpGet]
        public async Task<GetCombiLogResponse> Get(int lastHours = 12)
        {
            Logger.LogWarning("geldi!!");
            return new GetCombiLogResponse()
            {
                CombiLogs = await combiLogRepository.GetDtos(ApiUser.DeviceId, lastHours)
            };
        }
    }
}