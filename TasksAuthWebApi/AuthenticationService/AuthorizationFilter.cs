using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TaskServiceAPI.Models;

namespace TasksAuthWebApi.AuthenticationService
{
    public class AuthorizationFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!IsUserAuthorized(actionContext))
            {
                ShowAuthenticationError(actionContext);
                return;
            }
            base.OnAuthorization(actionContext);
        }

        private static void ShowAuthenticationError(HttpActionContext filterContext)
        {
            var responseDTO = new ResponseDTO { Code = 401, Message = "Unable to access, Please login" };
            filterContext.Response =
            filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseDTO);
        }

        public bool IsUserAuthorized(HttpActionContext actionContext)
        {
            var authHeader = FetchFromHeader(actionContext);

            if (authHeader != null)
            {
                var auth = new AuthenticationModule();
                JwtSecurityToken UserPayLoadToken = auth.GenerateUserClaimFromJWT(authHeader);

                if (UserPayLoadToken != null)
                {
                    var identity = auth.PopulateUserIdentity(UserPayLoadToken);
                    string[] roles = { "All" };
                    var genericPrincipal = new GenericPrincipal(identity, roles);
                    Thread.CurrentPrincipal = genericPrincipal;
                    var authenticationIdentity = Thread.CurrentPrincipal.Identity as JWTAuthenticationIdentity;
                    if (authenticationIdentity != null && !String.IsNullOrEmpty(authenticationIdentity.Username))
                    {
                        authenticationIdentity.Username = identity.Username;
                        authenticationIdentity.UserID = identity.UserID;
                    }
                    return true;
                }
            }
            return false;
        }

        private string FetchFromHeader(HttpActionContext actionContext)
        {
            string requestToken = null;

            var authRequest = actionContext.Request.Headers.Authorization;
            if (authRequest != null)
            {
                requestToken = authRequest.Parameter;
            }
            return requestToken;
        }
    }
}