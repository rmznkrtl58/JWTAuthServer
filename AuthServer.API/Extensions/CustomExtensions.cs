using AuthServer.Core.Configuration;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorkInterfaces;
using AuthServer.Data.Context;
using AuthServer.Data.Repositories;
using AuthServer.Data.UnitOfWorkPattern;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SharedLibrary.Options;
using System.Collections.Generic;
using System;
using SharedLibrary.Services;
using FluentValidation.AspNetCore;
using SharedLibrary.Extensions;

namespace AuthServer.API.Extensions
{
    public static class CustomExtensions
    {
        public static IServiceCollection AddExt(this IServiceCollection services, IConfiguration configuration)
        {
            //Newlemelerden kurtarmak için,İnterfacemi gördüğüm zaman bir defa nesne oluşturmak için
            //Generic yapılarımı kullanırken typeOf ile kullanırım
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            //Api->appsettingsJsondaki TokenOption yapılandırmamın altındaki özellikleri CustomTokenOption üzerinden mapliyorum ve yapılandırmamı DI Containerda geçiyorum.
            services.Configure<CustomTokenOption>(configuration.GetSection("TokenOption"));
            services.Configure<List<Client>>(configuration.GetSection("Clients"));

            //Dbcontext yapılandırması
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer"), sqlServerOption =>
                {
                    sqlServerOption.MigrationsAssembly("AuthServer.Data");
                });
            });

            //Identity Yapılandırması
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

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
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey),
                    //Doğrulamalar
                    ValidateIssuer = true,//tokeni dağıtan serveri doğrula
                    ValidateAudience = true,//İstek atıcağım apilerimin url'lerini doğrula
                    ValidateLifetime = true,//token süresini doğrula
                    ValidateIssuerSigningKey = true,//token'a atılan imzayı doğrula
                    ClockSkew = TimeSpan.Zero//tolerans edilecek zaman
                };
            });

            //FluentValidation Yapılandırması
            services.AddControllers().AddFluentValidation(opt =>
            { 
                //FromAssemblyContaining=>AuthServerApiAssembly classımın olduğu katmanımda geçen AbstractValidator implemente eden claslarıma register uygular AuthServerApiAssembly classımı ben oluşturdum.

opt.RegisterValidatorsFromAssemblyContaining<AuthServerApiAssembly>();
            });

            //FluentValidationExtensions yapılandırması
            services.UseCustomValidationResponse(configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthServer.API", Version = "v1" });
            });
            return services;
        }
    }
}
