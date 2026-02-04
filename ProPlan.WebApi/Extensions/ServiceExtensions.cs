using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProPlan.Entities.JwtCustomClaims;
using ProPlan.Repositories.Abstract;
using ProPlan.Repositories.Contracts;
using ProPlan.Services.Abstracts;
using ProPlan.Services.Auth.Abstract;
using ProPlan.Services.Auth.Contract;
using ProPlan.Services.Contracts;
using System.Text;

namespace ProPlan.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
                    b.MigrationsAssembly("ProPlan.WebApi")));
        }
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }
        public static void ConfigureServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyTaskService, CompanyTaskService>();
            services.AddScoped<ITaskDefinitionService, TaskDefinitionService>();
            services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
            services.AddScoped<ITaskSubItemService, TaskSubItemService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDashboardService, DashboardService>();
        }
        public static void CrossOriginConfigure(this IServiceCollection services)
        {
            /*
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            */
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder
                        .WithOrigins(
                            "http://proplanbusiness.com",
                            "https://proplanbusiness.com"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );
            });
        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILoggerService, LoggerManager>();
        }
        public static void ConfigureAuthenticationSchema(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key appsetting de tanımlı degil yada okuyamıyor.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        NameClaimType = CustomJwtClaimTypes.UserId,
                        RoleClaimType = CustomJwtClaimTypes.Role,             
                    };
                });
        }
        public static void ConfigurationSwaggerUIAuthorization(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title = "My API",
                            Version = "v1"
                        });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "JWT Bearer token giriniz. Örn: Bearer {token}"
                        });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {{
                            new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                            Array.Empty<string>()
                                }
                        });
                    });
        }
    }
}
