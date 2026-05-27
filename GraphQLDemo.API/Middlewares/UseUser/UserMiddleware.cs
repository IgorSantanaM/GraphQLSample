using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.Schema.Queries;
using HotChocolate.Resolvers;
using System.Security.Claims;

namespace GraphQLDemo.API.Middlewares.UseUser
{
    public class UserMiddleware
    {
        private readonly FieldDelegate _next;

        public UserMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(IMiddlewareContext context)
        {

            if(context.ContextData.TryGetValue("ClaimsPrincipal", out object rawClaimsPrincipal) && rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal)
            {
                User user = new()
                {
                    Id = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID),
                    Email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL),
                    Username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME),
                    EmailVerified = bool.TryParse(claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED), out bool emailVerified) && emailVerified
                };

                context.ContextData.Add("User", user);
            }
            await _next(context);
        }
    }
}
