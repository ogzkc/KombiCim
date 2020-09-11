using KombiCim.Data.Models;
using KombiCim.Data.Models.Mobile.Responses;
using KombiCim.Data.Repository;
using KombiCim.Data.Utilities;
using KombiCim.Filters;
using KombiCim.Utilities;
using System.Threading.Tasks;
using System.Web.Http;

namespace KombiCim.Controllers
{
    [Authentication]
    [ModelValidation]
    [TokenValidation]
    [Authorize(Roles = Roles.ROLE_MOBILE_APP)]
    public class CombiLogController : BaseApiController
    {
        public async Task<GetCombiLogResponse> Get(int lastHours = 12)
        {

            return new GetCombiLogResponse()
            {
                CombiLogs = await CombiLogRepository.GetDtos(ApiUser.OwnedDeviceId)
            };
        }
    }
}