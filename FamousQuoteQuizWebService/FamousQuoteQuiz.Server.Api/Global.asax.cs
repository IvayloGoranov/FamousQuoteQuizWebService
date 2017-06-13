using System.Data.Entity;
using System.Web.Http;

using Newtonsoft.Json.Serialization;

using FamousQuoteQuiz.Data;
using FamousQuoteQuiz.Data.Migrations;

namespace FamousQuoteQuiz.Server.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<QuoteQuizContext, DbMigrationsConfig>());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacConfig.RegisterAutofac();
            GlobalConfiguration.Configuration.Formatters
                .JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
