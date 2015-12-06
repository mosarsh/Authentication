using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Authentication.Service.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false)]
    public class GenericAuthenticationFilter : AuthorizationFilterAttribute
    {
        public GenericAuthenticationFilter()
        { 
        }

        private readonly bool _isActive = true;

        public GenericAuthenticationFilter(bool isActive)
        {
            _isActive = isActive;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!_isActive)
            {
                return;
            }

            var identity = FetchAuthHeader(actionContext);
            if (identity == null)
            {
                ChallengeAuthRequest(actionContext);
                return;
            }

            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;
            if (!OnAuthorizeUser(identity.Token, actionContext))
            {
                ChallengeAuthRequest(actionContext);
                return;
            }

            base.OnAuthorization(actionContext);
        }

        protected virtual bool OnAuthorizeUser(string token, HttpActionContext actionContext)
        {
            if (string.IsNullOrEmpty(token))
                return false;
            return true;
        }

        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext actionContext)
        {
            string authHeaderValue = null;
            var authRequest = actionContext.Request.Headers.Authorization;
            if (authRequest != null && !String.IsNullOrEmpty(authRequest.Scheme) && authRequest.Scheme == "Basic")
            {
                authHeaderValue = authRequest.Parameter;
            }
                
            if (string.IsNullOrEmpty(authHeaderValue))
            {
                return null;
            }
                
            return authHeaderValue.Length == 0 ? null : new BasicAuthenticationIdentity(authHeaderValue);
        }

        private static void ChallengeAuthRequest(HttpActionContext actionContext)
        {
            var dnsHost = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

            //Open in case you want to popup the form for credentials
            //actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", dnsHost));
        }
    }
}