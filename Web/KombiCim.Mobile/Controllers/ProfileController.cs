using KombiCim.Data.Models.Mobile.Requests;
using KombiCim.Data.Models.Mobile.Responses;
using KombiCim.Data.Repository;
using KombiCim.Data.Utilities;
using KombiCim.Filters;
using KombiCim.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace KombiCim.Controllers
{
    [Authentication]
    [ModelValidation]
    [TokenValidation]
    [Authorize(Roles = Roles.ROLE_MOBILE_APP)]
    public class ProfileController : BaseApiController
    {
        public async Task<GetProfilesResponse> Get()
        {
            return new GetProfilesResponse()
            {
                ProfileDtos = await ProfileRepository.GetDtos(ApiUser.OwnedDeviceId)
            };
        }

        public async Task<MobileBaseResponse> SetActive([FromBody]SetActiveProfileRequest request)
        {
            await ProfileRepository.SetActive(request.ProfileId, ApiUser.Id);
            return new MobileBaseResponse();
        }
    }
}