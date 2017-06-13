﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

using FamousQuoteQuiz.Data;

namespace TravelAgentAidKit.Server.Infrastructure
{
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, 
            IOwinContext context)
        {
            var appRoleManager = 
                new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<QuoteQuizContext>()));

            return appRoleManager;
        }
    }
}