using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.web.Requirements
{
    public class ViolenceRequriment : IAuthorizationRequirement
    {
        public int ThreshOldAge { get; set; }
    }

    public class ViolenceRequrimentHandler : AuthorizationHandler<ViolenceRequriment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequriment requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "birthdate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            Claim birthDateClaim = context.User.FindFirst("birthdate")!;

            var today = DateTime.Now;

            var birthDate = Convert.ToDateTime(birthDateClaim.Value);

            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age)) age--;

            if (requirement.ThreshOldAge > age)
            {
                context.Fail();
                return Task.CompletedTask;
            }


            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
