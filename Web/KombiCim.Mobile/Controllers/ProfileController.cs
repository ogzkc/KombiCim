using Kombicim.APIShared.Attributes;
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
    public class ProfileController : MobileApiController<ProfileController>
    {
        private readonly ProfileRepository profileRepository;

        public ProfileController(IServiceProvider serviceProvider, ProfileRepository profileRepository) : base(serviceProvider)
        {
            this.profileRepository = profileRepository;
        }

        [Route("[action]")]
        [HttpGet]
        public GetProfileTypesResponse GetProfileTypes()
        {
            return new GetProfileTypesResponse()
            {
                ProfileTypeDtos = profileRepository.GetSupportedProfileTypeDtos()
            };
        }

        [HttpGet]
        public async Task<GetProfilesResponse> Get()
        {
            return new GetProfilesResponse()
            {
                ProfileDtos = await profileRepository.GetDtos(ApiUser.DeviceId)
            };
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<CreateProfileResponse> Create([FromBody] CreateProfileRequest request)
        {
            var profile = await profileRepository.Create(request.Name, request.ProfileTypeId, ApiUser.Id);

            return new CreateProfileResponse()
            {
                Success = profile != null && profile.Id != 0
            };
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<MobileBaseResponse> SetActive([FromBody] SetActiveProfileRequest request)
        {
            await profileRepository.SetActive(request.ProfileId, ApiUser.Id);
            return new MobileBaseResponse();
        }
    }
}