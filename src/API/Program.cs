using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cpa.Application;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;
using Cpa.Presentation.API.Converters;
using Cpa.Presentation.API.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Cpa.Presentation.API.Services;
using Cpa.Presentation.API.Middlewares;
using Cpa.Infrastructure.Identity;
using Cpa.Infrastructure.Persistence;
using Cpa.Infrastructure;

namespace Cpa.Presentation.API;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient();
        builder.Services.AddBigLionInfrastructure(builder.Configuration);
        builder.Services.AddBigLionApplication();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCors();

        builder.Services.AddDataProtection()
            .SetApplicationName("BigLion")
            .PersistKeysToFileSystem(new DirectoryInfo(@"/app/publish/secret"));

        builder.Services
            .AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())))
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                options.JsonSerializerOptions.Converters.Add(new JsonDateOnlyConverter());
                options.JsonSerializerOptions.Converters.Add(new JsonTimeOnlyConverter());
            });

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
            options.SuppressMapClientErrors = true;
        });

        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
      
        //----------------Identity JWt-------------------
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration
            .GetSection(JwtSettings.SectionName)
            .Get<JwtSettings>()!;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwt.Issuer,
                ValidAudience = jwt.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.SecretKey))
            };
    });

        builder.Services.AddAuthorization();

        //---------------------------------------

        builder.Services.AddMemoryCache();
        //builder.Services.AddScoped<ICheckupItemCacheService, CheckupItemCacheService>();
        //builder.Services.AddScoped<ICheckupStatusService, CheckupStatusService>();

        // basic policy
        // this authorization should be config in the infrastructure?, revise later
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(CpaPolicies.RequireAuthenticatedUser, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "BigLion-api");
                policy.RequireClaim("scope", "openid");
                policy.RequireClaim("scope", "profile");
                policy.RequireClaim("scope", "offline_access");
            });

            options.AddPolicy(CpaPolicies.RequireDoctor, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(BigLionRoles.Doctor);
                policy.RequireClaim("scope", "BigLion-api");
                policy.RequireClaim("scope", "openid");
                policy.RequireClaim("scope", "profile");
                policy.RequireClaim("scope", "offline_access");
                policy.RequireClaim("scope", "license");
            });

            options.AddPolicy(CpaPolicies.RequireNurse, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(BigLionRoles.Nurse);
                policy.RequireClaim("scope", "BigLion-api");
                policy.RequireClaim("scope", "openid");
                policy.RequireClaim("scope", "profile");
                policy.RequireClaim("scope", "offline_access");
            });

            options.AddPolicy(CpaPolicies.RequireAdmin, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(BigLionRoles.Admin);
                policy.RequireClaim("scope", "BigLion-api");
                policy.RequireClaim("scope", "openid");
                policy.RequireClaim("scope", "profile");
                policy.RequireClaim("scope", "offline_access");
            });

            options.AddPolicy(CpaPolicies.RequireEmployee, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(BigLionRoles.Employee);
                policy.RequireClaim("scope", "BigLion-api");
                policy.RequireClaim("scope", "openid");
                policy.RequireClaim("scope", "profile");
                policy.RequireClaim("scope", "offline_access");
                policy.RequireClaim("scope", "employee_id");
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BigLion API",
                Version = "v1",
                Description = "เอกสารเรียกใช้งาน API BigLion",
                Contact = new OpenApiContact
                {
                    Name = "ติดต่อ BigLion",
                    Email = "teerapoldev@gmail.com"
                },
                License = new OpenApiLicense
                {
                    Name = "ลิขสิทธิ์ของ BigLion",
                    Url = new Uri("https://www.Cpa.com")
                }
            });

 
            List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "BigLion*.xml", SearchOption.TopDirectoryOnly).ToList();
            xmlFiles.ForEach(file => c.IncludeXmlComments(file, includeControllerXmlComments: true));
        });

        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<CpaDatabaseContextInitializer>();
            await initializer.MigrationAsync();
            await initializer.SeedDataAsync(scope);
        }

        var basePath = new PathString(Environment.GetEnvironmentVariable("ASPNETCORE_BASE_PATH"));
        if (!string.IsNullOrEmpty(basePath))
        {
            app.UsePathBase(basePath);
        }

        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "BigLion API";
            if (!string.IsNullOrEmpty(basePath))
            {
                c.InjectStylesheet($"{basePath}/swagger/ui/fonts/fonts.css");
                c.InjectStylesheet($"{basePath}/swagger/ui/custom.css");
                c.InjectJavascript($"{basePath}/swagger/ui/custom.js");
                c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "BigLion API v1");
            }
            else
            {
                c.InjectStylesheet("/swagger/ui/fonts/fonts.css");
                c.InjectStylesheet("/swagger/ui/custom.css");
                c.InjectJavascript("/swagger/ui/custom.js");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BigLion API v1");
            }

            c.DefaultModelsExpandDepth(-1);
            c.ConfigObject.AdditionalItems.Add("syntaxHighlight", true);
           
        });

        if (builder.Environment.IsProduction())
        {
            var forwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };

            forwardedHeaderOptions.KnownIPNetworks.Clear();
            forwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardedHeaderOptions);
        }

        app.UseHttpsRedirection();
        app.UseExceptionMiddleware();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseRouting();
        app.UseCors(builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
              

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
                name: "default",
                pattern: "{controller:slugify}/{action:slugify}/{id:slugify?}");

        app.Run();
    }
}
