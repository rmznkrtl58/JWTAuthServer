

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedLibrary.DTOs;
using SharedLibrary.Exceptions;
using System.Text.Json;

namespace SharedLibrary.Extensions
{
    public static class CustomExceptionHandle
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            //tüm hataları yakalayan bir middleware
            app.UseExceptionHandler(configure =>
            {
                //configure.Run();=>buna gelirse gelen istek bir sonraki middleware geçmez
                //configure.use();=>bunda gelirse devam edebilir
                configure.Run(async context =>
                {
                    //benim içimde olan serverda meydana gelen hataları yakalayacağım
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    
                    var errorFeature=context.Features.Get<ExceptionHandlerFeature>();

                    if (errorFeature != null)
                    {
                        var ex = errorFeature.Error;
                        ErrorDto errorDto = null;
                        if (ex is CustomException) 
                        {
                            errorDto = new ErrorDto(ex.Message, true);
                        }
                        else
                        {
                            errorDto = new ErrorDto(ex.Message, false);
                        }
                        var responseDto = ResponseDto<NoContentDto>.Fail(errorDto, 500);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(responseDto));
                    }
                });
            });
        }
    }
}
