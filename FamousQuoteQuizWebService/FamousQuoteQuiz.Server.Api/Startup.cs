using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FamousQuoteQuiz.Server.Api.Startup))]

namespace FamousQuoteQuiz.Server.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
