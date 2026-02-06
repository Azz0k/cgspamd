using cgspamd.api.Application;
using cgspamd.api.Models;
using cgspamd.core.Application;
using cgspamd.core.Context;
using cgspamd.core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("cgspamd.tests")]
namespace cgspamd.api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string defaultConnectionString = "Data Source=Store.sqlite";
            var settignsBuilder = new ConfigurationBuilder().AddJsonFile($"appsettings.json");
            IConfiguration config = settignsBuilder.Build();
            string? allowedOrigins = config.GetSection("AllowedOrigins").Get<string>();
            string? connectionString = config.GetSection("ConnectionString").Get<string>() ?? defaultConnectionString;
            string secretCode = config.GetSection("JWTSecretCode").Get<string>() ?? Guid.NewGuid().ToString();
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSpaStaticFiles(conf =>
            {
                conf.RootPath = "wwwroot";
            });
            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlite(connectionString));
            builder.Services.AddScoped<DatabaseService>();
            builder.Services.AddScoped<FilterRulesApplication>();
            builder.Services.AddScoped<UsersApplication>();
            builder.Services.AddScoped<UserAuthenticationApplication>();
            builder.Services.Configure<APISettings>(opt => opt.JwtSecretCode = secretCode);
            builder.Services.AddScoped<APISettings>();
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretCode)),
                        ValidateIssuerSigningKey = true,
                    };
                });
            if (allowedOrigins != null)
            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("FrontEnd", policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });
            }
            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var dbService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
            await dbService.InitDatabaseAsync();
            app.UseStaticFiles();
            if (allowedOrigins != null)
            {
                app.UseCors("FrontEnd");
            }
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";
            });
            app.Run();
        }
    }
}
