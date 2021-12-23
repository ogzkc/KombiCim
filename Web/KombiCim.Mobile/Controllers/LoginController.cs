using Kombicim.APIShared.Attributes;
using Kombicim.Data.Models;
using Kombicim.Data.Models.Mobile.Requests;
using Kombicim.Data.Models.Mobile.Responses;
using Kombicim.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kombicim.Mobile.Controllers
{
    [Authentication]
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Roles = Roles.ROLE_MOBILE_APP)]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository userRepository;

        public LoginController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<LoginPostResponse> Post([FromBody] LoginPostRequest request)
        {
            string token = null;
            try
            {
                token = await userRepository.GetToken(request.Content);
            }
            catch (Exception ex)
            {
                // can be logged (may be exception from decoding base64)
            }
            return new LoginPostResponse()
            {
                Success = token != null,
                ErrorCode = token != null ? null : ErrorCodes.INVALID_USER_INFO,
                Token = token
            };
        }
    }
}