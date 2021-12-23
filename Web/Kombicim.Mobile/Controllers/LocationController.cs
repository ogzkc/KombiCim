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
    public class LocationController : MobileApiController<LocationController>
    {
        private readonly LocationRepository locationRepository;
        private readonly ProfileRepository profileRepository;

        public LocationController(IServiceProvider serviceProvider, LocationRepository locationRepository, ProfileRepository profileRepository) : base(serviceProvider)
        {
            this.locationRepository = locationRepository;
            this.profileRepository = profileRepository;
        }

        [HttpGet]
        public async Task<GetLocationsResponse> Get(int profileId)
        {
            return new GetLocationsResponse()
            {
                LocationDtos = await locationRepository.GetLocationDtos(ApiUser.DeviceId, profileId)
            };
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<GetLocationsResponse> GetThermometers()
        {
            return new GetLocationsResponse()
            {
                LocationDtos = await locationRepository.GetThermometers(ApiUser.DeviceId)
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