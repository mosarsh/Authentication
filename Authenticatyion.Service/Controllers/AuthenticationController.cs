using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Data;
using System.Data.OleDb;
using Authentication.Service.Helpers;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Facebook;
using System.Security.Claims;
using Authentication.Data;
using Authentication.Model;
using Authentication.Constant;

namespace Authentication.Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationController : ApiController
    {
        Repository _repo = new Repository();

        /// <summary>
        /// Local user registeration route
        /// </summary>
        /// <param name="model">accepts resiger model as a param</param>
        /// <returns>Http response message</returns>
        public HttpResponseMessage Register(RegisterModel model) 
        {
            try
            {
                _repo.RegisterLocalUser(model);

                return Request.CreateResponse(HttpStatusCode.OK, Message.UserRegistrationSucceess);
            }
            catch (Exception error) 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.GetType().Name);
            }
        }

        /// <summary>
        /// local user login route
        /// </summary>
        /// <param name="model">accepts login model as a param</param>
        /// <returns>http response message</returns>
        public HttpResponseMessage Login(LoginModel model) 
        {
            try 
            {
                var result = _repo.GenerateAccessToken(model.UserName, model.Password, false);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception error) 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.GetType().Name);
            }
        }

        /// <summary>
        /// external user login route
        /// http://{hostname}/v1/Authentication/ExternalLogin?provider={provider_name}&response_type=token&client_id={client_id}& redirect_uri=http://{hostname}/Home
        /// </summary>
        /// <param name="provider">external provicer name</param>
        /// <param name="error">error param</param>
        /// <returns>http action result</returns>
        public IHttpActionResult ExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                //return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri.Validate(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                //Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                //return new ChallengeResult(provider, this);
            }

            var user = _repo.FindExternalUser(externalLogin.LoginProvider, externalLogin.ProviderKey);

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken.Verify(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            var user = _repo.FindExternalUser(model.Provider, verifiedAccessToken.user_id);

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            //user = new IdentityUser() { UserName = model.UserName };

            _repo.RegisterExternalUser(user);
            
            //var info = new ExternalLoginInfo()
            //{
            //    DefaultUserName = model.UserName,
            //    Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            //};

            //result = await _repo.AddLoginAsync(user.Id, info.Login);
            //if (!result.Succeeded)
            //{
            //    return GetErrorResult(result);
            //}

            //generate access token response
            var accessTokenResponse = _repo.GenerateAccessToken(model.UserName, null, true);

            //return Ok(accessTokenResponse);
            return Ok();
        }
    }
}
