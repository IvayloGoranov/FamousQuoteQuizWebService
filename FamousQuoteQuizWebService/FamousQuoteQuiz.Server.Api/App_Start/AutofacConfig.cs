using System.Web.Http;
using System.Reflection;

using Autofac;
using Autofac.Integration.WebApi;

using FamousQuoteQuiz.Services;
using FamousQuoteQuiz.Data.Repositories;
using FamousQuoteQuiz.Data;

namespace FamousQuoteQuiz.Server.Api
{
    public static class AutofacConfig
    {
        public static void RegisterAutofac()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // Register services
            RegisterServices(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            var servicesAssembly = Assembly.GetAssembly(typeof(IModesService));
            builder.RegisterAssemblyTypes(servicesAssembly).AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerRequest();

            builder.Register(x => new QuoteQuizContext())
                .As<IQuoteQuizContext>()
                .InstancePerRequest();
        }
    }
}