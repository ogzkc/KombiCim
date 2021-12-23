using Kombicim.Data.Models;
using Kombicim.Data.Repository;
using Kombicim.Data.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;

namespace Kombicim.APIShared.Middlewares
{
    public class BasicAuthMiddleware
    {
        public const string API_MOBILE_USER = "ApiMobileUser";

        private const string API_TOKEN = "ApiToken";
        private const string AUTH = "Authorization";
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApiUserRepository apiUserRepository, UserRepository userRepository)
        {
            if (!context.Request.Headers.ContainsKey(AUTH))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                try
                {
                    var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers[AUTH]);
                    var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                    var username = credentials[0];
                    var password = credentials[1];

                    var apiUser = await apiUserRepository.GetAsync(username, StringHelper.Hash(password));
                    if (apiUser != null)
                    {
                        var roleName = ApiAuthType.GetName(apiUser.AuthTypeId);

                        var identity = new GenericIdentity("kc");
                        var principal = new GenericPrincipal(identity, new string[] { roleName });
                        Thread.CurrentPrincipal = principal;

                        if (context.User != null)
                            context.User = principal;

                        if (roleName == ApiAuthType.MOBILE_APP_NAME)
                        {
                            if (context.Request.Headers.TryGetValue(API_TOKEN, out StringValues stringValues) && stringValues.Count == 1)
                            {
                                var user = await userRepository.Get(stringValues.Single());
                                context.Items.Add(API_MOBILE_USER, user);
                            }
                            else if (!context.Request.Path.Value.EndsWith("Login")) // TODO: make constant
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            }
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }
                }
                catch
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
            }

            if (context.Response.StatusCode != StatusCodes.Status401Unauthorized)
                await _next(context);
        }
    }
}
