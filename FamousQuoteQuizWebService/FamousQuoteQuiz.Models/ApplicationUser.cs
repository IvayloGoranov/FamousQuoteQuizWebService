using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Models
{
    public class ApplicationUser : IdentityUser
    {
        private ICollection<UserSession> userSessions;

        public ApplicationUser()
        {
            this.userSessions = new HashSet<UserSession>();
        }

        public virtual ICollection<UserSession> UserSessions
        {
            get
            {
                return this.userSessions;
            }

            set
            {
                this.userSessions = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
