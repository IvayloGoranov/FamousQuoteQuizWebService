using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace FamousQuoteQuiz.Server.Infrastructure.Attributes
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
            {
                return;
            }

            var userSessionManager = new UserSessionManager();
            if (userSessionManager.ReValidateSession())
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized, "Session token expried or not valid.");
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.
                            GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}