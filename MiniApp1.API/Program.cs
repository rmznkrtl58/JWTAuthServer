using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniApp1.API.RequiredClaims;
using SharedLibrary.Extensions;
using SharedLibrary.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AppSettingsJsondaki tokenOptionýmý okutuyorum.
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
//program.cs yapýlandýrmasý
builder.Services.AddCustomTokenAuthExt(builder.Configuration);

//Uygulama ayaða kalkarken bir nesne örneði oluþsun ve uygulama ayaktayken o nesne örneðini kullansýn dinamik bir durum söz konusu deðil veritabaný iþlemlerinede girmediðim için addSingleton Kullandým.
builder.Services.AddSingleton<IAuthorizationHandler, BirthDateRequirementHandler>();
//Policy Tanýmlamalarý
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AnkaraPolicy", policy =>
    {
        //Citysi ankara olanlar eriþim saðlar
        policy.RequireClaim("city", "ankara");
    });
    opt.AddPolicy("AgePolicy", policy =>
    {
        //18 yaþýndan büyük olanlar eriþim saðlar
        policy.Requirements.Add(new BirthDateRequirement(18));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Aþaðýdaki sýralama önemli
app.UseCustomException();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
