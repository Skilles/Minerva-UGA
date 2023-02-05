using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Minerva.Config;
using Minerva.Infrastructure;
using Minerva.Utility;
using Minerva.Validation;

namespace Minerva;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = new MinervaConfig();

        builder.Configuration.Bind(config);

        ConfigureServices(builder.Services, config);

        var app = builder.Build();

        ConfigureApp(app);

        app.Run();

        static void ConfigureApp(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

            app.UseValidationException();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseFastEndpoints(cfg =>
            {
                cfg.Endpoints.RoutePrefix = "api";


                cfg.Errors.ResponseBuilder = (failures, _, _) =>
                {
                    return new ValidationFailureResponse
                    {
                        Errors = failures.Select(y => y.ErrorMessage).ToList()
                    };
                };
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3(s => { s.ConfigureDefaults(); });
            }


            app.MapFallbackToFile("index.html");
        }

        static void ConfigureServices(IServiceCollection services, MinervaConfig config)
        {
            services.AddLogging();
            services.AddMinervaServices(config);
            services.AddFastEndpoints();
            services.AddSwaggerDoc();
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.Jwt.Issuer,
                        ValidAudience = config.Jwt.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Jwt.Key))
                    };
                });
            services.AddCors();
        }
    }
}