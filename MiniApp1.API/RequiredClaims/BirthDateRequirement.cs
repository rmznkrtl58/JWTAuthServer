using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace MiniApp1.API.RequiredClaims
{
    public class BirthDateRequirement:IAuthorizationRequirement
    {
        //Policyimde belirtmiş olduğum yaşa göre yetkilendirme yapacağım
        public int Age { get; set; }
        public BirthDateRequirement(int age)
        {
            Age = age;
        }
    }
    public class BirthDateRequirementHandler : AuthorizationHandler<BirthDateRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BirthDateRequirement requirement)
        {
            var birthDate = context.User.FindFirst(x => x.Type == "BirthDate").Value;
            if(birthDate is null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var today = DateTime.Now;
            var ageCalculation = today.Year - Convert.ToDateTime(birthDate).Year;
            if (requirement.Age < ageCalculation)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
