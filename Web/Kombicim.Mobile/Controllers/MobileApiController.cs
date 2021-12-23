using Kombicim.APIShared.Middlewares;
using Kombicim.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kombicim.Mobile.Controllers
{
    [Route("[controller]")]
    public class MobileApiController<T> : ControllerBase
    {
        public UserEntity ApiUser { get; set; }
        public ILogger<T> Logger { get; set; }

        public MobileApiController(IServiceProvider serviceProvider)
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext.Items.ContainsKey(BasicAuthMiddleware.API_MOBILE_USER) && httpContext.Items[BasicAuthMiddleware.API_MOBILE_USER] is UserEntity userEntity)
            {
                ApiUser = userEntity;
            }
            else
                throw new Exception($"HttpContext does not contains Mobile api user information.");

            Logger = serviceProvider.GetService<ILogger<T>>();
        }
    }
}
