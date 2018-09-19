using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiJwt.Models
{
    public class MinimumMonthsEmployedHandler : AuthorizationHandler<MinimumMonthsEmployedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumMonthsEmployedRequirement requirement)
        {

            var employmentCommenced = context.User.FindFirst(
                claim => claim.Type == CustomClaims.EmploymentCommenced);

            var employmentStarted = Convert.ToDateTime(employmentCommenced);
            var today = DateTime.Now;

           // var monthsPassed = employmentStarted - today;

            if (0 >= requirement.MinimumMonthsEmployed)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
