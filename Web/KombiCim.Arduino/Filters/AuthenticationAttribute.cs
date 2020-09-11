using KombiCim.Data.Models;
using KombiCim.Data.Repository;
using KombiCim.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace KombiCim.Filters
{
    public class AuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                return;
            }
            else
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                var username = usernamePasswordArray[0];
                var password = usernamePasswordArray[1];

                using (var db = new KombiCimEntities())
                {
                    var apiUser = ApiUserRepository.Get(username, StringHelper.Hash(password), db);
                    if (apiUser != null)
                    {
                        var roleName = apiUser.ApiAuthType.Name;

                        var identity = new GenericIdentity("kc");
                        var principal = new GenericPrincipal(identity, new string[] { roleName });
                        Thread.CurrentPrincipal = principal;

                    if (HttpContext.Current != null)
                            HttpContext.Current.User = principal;
                    }
                    else
                        actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}