using KombiCim.Data.Models;
using KombiCim.Data.Repository;
using KombiCim.Data.Utilities;
using KombiCim.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace KombiCim.Filters
{
    public class TokenValidationAttribute : ActionFilterAttribute
    {
        private const string API_TOKEN = "ApiToken";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Headers.Contains(API_TOKEN))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }
            else
            {
                var tokens = actionContext.Request.Headers.GetValues(API_TOKEN);
                if (tokens.Count() != 1)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return;
                }

                var token = tokens.FirstOrDefault();
                if (actionContext.ControllerContext.Controller is BaseApiController apiController)
                {
                    apiController.ApiUser = UserRepository.Get(token);
                    if (apiController.ApiUser == null)
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}