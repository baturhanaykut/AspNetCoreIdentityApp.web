using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.web.Requirements
{
    public class ExechangeExpireRequriment : IAuthorizationRequirement
    {

    }

    public class ExechangeExpireRequrimentHandeler : AuthorizationHandler<ExechangeExpireRequriment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExechangeExpireRequriment requirement)
        {

            if (!context.User.HasClaim(x => x.Type == "ExchangeExpireDate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            Claim exchangeExpireDate = context.User.FindFirst("ExchangeExpireDate")!;

            if (DateTime.Now > Convert.ToDateTime(exchangeExpireDate.Value))
            {
                context.Fail();
                return Task.CompletedTask;
            }


            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
