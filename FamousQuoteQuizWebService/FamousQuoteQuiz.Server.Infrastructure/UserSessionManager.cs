using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net.Http;
using System.Web;

using FamousQuoteQuiz.Data;
using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Server.Infrastructure
{
    public class UserSessionManager
    {
        private const int SessionLifeTimeInMinutes = 120;

        private IQuoteQuizContext dbContext;

        public UserSessionManager()
            : this(new QuoteQuizContext())
        {
        }

        public UserSessionManager(IQuoteQuizContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private HttpRequestMessage CurrentRequest
        {
            get
            {
                return (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
            }
        }

        /// <summary>
        /// Extends the validity period of the current user's session in the database.
        /// This will configure the user's bearer authorization token to expire after
        /// certain period of time (e.g. 30 minutes, see UserSessionTimeout in Web.config)
        /// </summary>
        public void CreateUserSession(string username, string authToken)
        {
            var userId = this.dbContext.Users.First(u => u.UserName == username).Id;
            var userSession = new UserSession()
            {
                OwnerUserId = userId,
                AuthToken = authToken
            };
            // Extend the lifetime of the current user's session: current moment + fixed timeout
            userSession.ExpirationDateTime = DateTime.Now + TimeSpan.FromMinutes(30);

            this.dbContext.UserSessions.Add(userSession);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Makes the current user session invalid (deletes the session token from the user sessions).
        /// The goal is to revoke any further access with the same authorization bearer token.
        /// Typically this method is called at "logout".
        /// </summary>
        public void InvalidateUserSession()
        {
            string authToken = GetCurrentBearerAuthrorizationToken();
            var currentUserId = GetCurrentUserId();
            var userSession = this.dbContext.UserSessions.FirstOrDefault(session =>
                session.AuthToken == authToken && session.OwnerUserId == currentUserId);
            if (userSession != null)
            {
                this.dbContext.UserSessions.Remove(userSession);
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Re-validates the user session. Usually called at each authorization request.
        /// If the session is not expired, extends it lifetime and returns true.
        /// If the session is expired or does not exist, return false.
        /// </summary>
        /// <returns>True if the session is valid</returns>
        public bool ReValidateSession()
        {
            string authToken = this.GetCurrentBearerAuthrorizationToken();
            var currentUserId = this.GetCurrentUserId();
            var userSession = this.dbContext.UserSessions.FirstOrDefault(session =>
                session.AuthToken == authToken && session.OwnerUserId == currentUserId);

            if (userSession == null)
            {
                // User does not have a session with this token --> invalid session
                return false;
            }

            if (userSession.ExpirationDateTime < DateTime.Now)
            {
                // User's session is expired --> invalid session
                return false;
            }

            // Extend the lifetime of the current user's session: current moment + fixed timeout
            userSession.ExpirationDateTime = DateTime.Now + TimeSpan.FromMinutes(SessionLifeTimeInMinutes);
            this.dbContext.SaveChanges();

            return true;
        }

        public void DeleteExpiredSessions()
        {
            var expiredUserSessions = this.dbContext.UserSessions.Where(
                session => session.ExpirationDateTime < DateTime.Now);
            foreach (var userSession in expiredUserSessions)
            {
                this.dbContext.UserSessions.Remove(userSession);
            }

            this.dbContext.SaveChanges();
        }

        /// <returns>The current bearer authorization token from the HTTP headers</returns>
        private string GetCurrentBearerAuthrorizationToken()
        {
            string authToken = null;
            if (this.CurrentRequest.Headers.Authorization != null)
            {
                if (this.CurrentRequest.Headers.Authorization.Scheme == "Bearer")
                {
                    authToken = this.CurrentRequest.Headers.Authorization.Parameter;
                }
            }

            return authToken;
        }

        private string GetCurrentUserId()
        {
            if (HttpContext.Current.User == null)
            {
                return null;
            }

            string userId = HttpContext.Current.User.Identity.GetUserId();

            return userId;
        }
    }
}