using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Options;
using SharedLibrary.Services;
using System;


namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {
        public static IServiceCollection AddCustomTokenAuthExt(this IServiceCollection services,IConfiguration configuration)
        {
            //JWT Authentication&Authorization Yapılandırması
            services.AddAuthentication(opt =>
            {
                //Authentication Scheme->benim kimlik doğrulama işlemimde kullanacağım iskelet yapı
                //ben bir tane üyelik sistemi kurduğum için bir JwtBearerDefaultsa ait schemeyı kullanıyorum
                //iki tane üyelik sistemim olsaydı kendi custom şemamı belirtip ona göre yapılandırma yapacaktım
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //DefaultChallengeScheme=>benim JWT authenticationdaki kullanıcağım şema ile haberleştirme yaptığım yapıdır.
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //JWT Authenticationda kullanacağım şemada JWTBearerDefaultstaki şemayı aynen kullanıyorum
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                //AppSettingsteki TokenOption yapılanmasını okuyup CustomTokenOption classımdaki proplarıma mapladım
                var tokenOption = configuration.GetSection("TokenOption").Get<CustomTokenOption>();
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //Tokenimin doğrulamasını yaptığım yer burdaki atamalarını yaptığım özellikler ile apilerime yapılan istekte gelen tokenimin payloadındaki özelliler eşleşiyorsa doğrulama gerçekleşmiş olacak ve istek başarılı olarak dönecek

                    //Atamalar
                    ValidIssuer = tokenOption.Issuer,
                    ValidAudience = tokenOption.Audience[0],
                    IssuerSigningKey =SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey),
                    //Doğrulamalar
                    ValidateIssuer = true,//tokeni dağıtan serveri doğrula
                    ValidateAudience = true,//İstek atıcağım apilerimin url'lerini doğrula
                    ValidateLifetime = true,//token süresini doğrula
                    ValidateIssuerSigningKey = true,//token'a atılan imzayı doğrula
                    ClockSkew = TimeSpan.Zero//tolerans edilecek zaman
                };
            });
            return services;
        }
    }
}
