

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.DTOs;
using System.Linq;

namespace SharedLibrary.Extensions
{
    public static class CustomValidationResponse
    {
        public static IServiceCollection UseCustomValidationResponse(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.Where(x => x.Errors.Count > 0).SelectMany(y => y.Errors).Select(z => z.ErrorMessage);
                    ErrorDto errorDto = new ErrorDto(errors.ToList(), true);
                    var responseDto = ResponseDto<NoContentResult>.Fail(errorDto, 400);
                    return new BadRequestObjectResult(responseDto);
                };
            });
            return services;
        }
    }
}
