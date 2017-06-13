using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Script.Serialization;
using System.Net;
using System.Data.Entity;

using FamousQuoteQuiz.Server.Infrastructure.Attributes;
using FamousQuoteQuiz.Server.Infrastructure;
using FamousQuoteQuiz.Data;
using FamousQuoteQuiz.Server.Api.BindingModels.Account;
using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Server.Api.Controllers
{
    [SessionAuthorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager userManager;
        private IQuoteQuizContext dbContext;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat,
            IQuoteQuizContext dbContext)
        {
            this.UserManager = userManager;
            this.AccessTokenFormat = accessTokenFormat;

            this.dbContext = dbContext;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        private IAuthenticationManager Authentication
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        // POST api/Account/Logout
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            // This does not actually perform logout! The OWIN OAuth implementation
            // does not support "revoke OAuth token" (logout) by design.
            this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);

            // Delete the user's session from the database (revoke its bearer token)
            var userSessionManager = new UserSessionManager();
            userSessionManager.InvalidateUserSession();

            return this.Ok(
                new
                {
                    message = "Logout successful."
                }
            );
        }

        // POST api/Account/ChangePassword
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Content(HttpStatusCode.Conflict, this.ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        // POST api/Account/Register
        [SessionAuthorize(Roles = "Administrator")]
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (model == null)
            {
                return this.Content(HttpStatusCode.Conflict, "Model cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return this.Content(HttpStatusCode.Conflict, this.ModelState);
            }

            var duplicate = await this.dbContext.Users.
                FirstOrDefaultAsync(x => x.UserName == model.Username);
            if (duplicate != null)
            {
                return this.BadRequest(string.Format("User {0} has already been added.", model.Username));
            }

            var user = new ApplicationUser
            {
                UserName = model.Username
            };

            IdentityResult createResult = await this.UserManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                return this.GetErrorResult(createResult);
            }

            if (model.IsAdmin)
            {
                IdentityResult addRoleResult = await this.UserManager.AddToRolesAsync(user.Id, "Administrator");
                if (!addRoleResult.Succeeded)
                {
                    return this.GetErrorResult(createResult);
                }
            }

            return this.Ok();
        }

        // POST api/Account/Login
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginUserBindingModel model)
        {
            if (model == null)
            {
                return this.Content(HttpStatusCode.Conflict, "Model cannot be null.");
            }

            // Invoke the "token" OWIN service to perform the login (POST /api/token)
            // Use Microsoft.Owin.Testing.TestServer to perform in-memory HTTP POST request
            //var testServer = TestServer.Create<Startup>();

            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };
            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            HttpClient httpClient = new HttpClient();
            var request = HttpContext.Current.Request;
            var tokenServiceResponse = await httpClient.
                PostAsync(string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority) +
                    Startup.OAuthOptions.TokenEndpointPath.ToString(), requestParamsFormUrlEncoded);
            //var tokenServiceResponse = await testServer.HttpClient.PostAsync(
            //    Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

            if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
            {
                // Sucessful login --> create user session in the database
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var jsSerializer = new JavaScriptSerializer();
                var responseData =
                    jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
                var authToken = responseData["access_token"];
                var username = responseData["username"];
                var userSessionManager = new UserSessionManager();
                userSessionManager.CreateUserSession(username, authToken);

                // Cleanup: delete expired sessions from the database
                userSessionManager.DeleteExpiredSessions();
            }

            return this.ResponseMessage(tokenServiceResponse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.Content(HttpStatusCode.Conflict, this.ModelState);
            }

            return null;
        }
    }
}
