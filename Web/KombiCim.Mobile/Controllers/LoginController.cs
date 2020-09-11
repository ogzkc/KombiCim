using KombiCim.Data.Models;
using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models.Mobile.Requests;
using KombiCim.Data.Models.Mobile.Responses;
using KombiCim.Data.Repository;
using KombiCim.Data.Utilities;
using KombiCim.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace KombiCim.Controllers
{
    [ModelValidationAttribute]
    [Authentication]
    [Authorize(Roles = Roles.ROLE_MOBILE_APP)]
    public class LoginController : ApiController
    {
        public async Task<LoginPostResponse> Post([FromBody]LoginPostRequest request)
        {
            using (var db = new KombiCimEntities())
            {
                var token = await UserRepository.GetToken(request.Content, db);
                return new LoginPostResponse()
                {
                    Success = token != null,
                    ErrorCode = token != null ? null : ErrorCodes.INVALID_USER_INFO,
                    Token = token
                };
            }
        }
    }
}