using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProPlan.Entities.Validators;
using ProPlan.Services.Abstracts;
using ProPlan.Services.Contracts;
using ProPlan.Services.Mapping;
using ProPlan.WebApi.Extensions;
using ProPlan.WebApi.Filters;
using ProPlan.WebApi.Middleware;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//burayý 20.01.2026 tarihinde kapattým sebebi yenisini aþagýya ekledim.
//builder.Services.AddControllers().AddApplicationPart(typeof(ProPlan.Presentation.AssemblyReference).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();

builder.Services.AddControllers(options =>
{
options.Filters.Add<FluentValidationFilter>();
})
    .AddApplicationPart(typeof(ProPlan.Presentation.AssemblyReference).Assembly)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true; // FluentValidation filter'ýmýz kullanýyor
}).AddJsonOptions(options =>
{
    // JSON serialization ayarlarý
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.WriteIndented = false; // Production'da false olacak
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureServiceLayer();
builder.Services.CrossOriginConfigure();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

//burayý service extensions a alalým.
builder.Services.AddAutoMapper(config =>
{
    //config.ShouldMapProperty = p => p.GetMethod.IsPublic;

    config.AddProfile<UserProfile>();
    config.AddProfile<CompanyProfile>();
    config.AddProfile<TaskAssignmentProfile>();
    config.AddProfile<TaskDefinitionProfile>();
    config.AddProfile<CompanyTaskProfile>();
    config.AddProfile<TaskSubItemProfile>();
});

builder.Services.ConfigureAuthenticationSchema(builder.Configuration);
builder.Services.ConfigurationSwaggerUIAuthorization();
builder.Services.ConfigureLoggerService();

var app = builder.Build();

//Middleware sýralamasý önemli olabilir dikkat et.
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//https e yönlendirmemek için kapattýk.
//app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();//önce Authentication sýrayý bozma
app.UseAuthorization();

app.MapControllers();

app.Run("http://0.0.0.0:5000");
//app.Run();    //swaggerla çalýþacagýmda aþagýyý kullan frontende api göndereceksen yukarýdaki kod aktif olamalý.
