using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Data
{
    public class QuoteQuizContext : IdentityDbContext<ApplicationUser>, IQuoteQuizContext
    {
        public QuoteQuizContext()
            : base("name=QuoteQuizContext")
        {
        }

        public IDbSet<Author> Authors { get; set; }

        public IDbSet<Quote> Quotes { get; set; }

        public IDbSet<QuizMode> QuizModes { get; set; }

        public IDbSet<UserSession> UserSessions { get; set; }

        public static QuoteQuizContext Create()
        {
            return new QuoteQuizContext();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}