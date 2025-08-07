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

//AppSettingsJsondaki tokenOption�m� okutuyorum.
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
//program.cs yap�land�rmas�
builder.Services.AddCustomTokenAuthExt(builder.Configuration);

//Uygulama aya�a kalkarken bir nesne �rne�i olu�sun ve uygulama ayaktayken o nesne �rne�ini kullans�n dinamik bir durum s�z konusu de�il veritaban� i�lemlerinede girmedi�im i�in addSingleton Kulland�m.
builder.Services.AddSingleton<IAuthorizationHandler, BirthDateRequirementHandler>();
//Policy Tan�mlamalar�
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AnkaraPolicy", policy =>
    {
        //Citysi ankara olanlar eri�im sa�lar
        policy.RequireClaim("city", "ankara");
    });
    opt.AddPolicy("AgePolicy", policy =>
    {
        //18 ya��ndan b�y�k olanlar eri�im sa�lar
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

//A�a��daki s�ralama �nemli
app.UseCustomException();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
