using cgspamd.core.Applications;
using cgspamd.core.Contexts;
using cgspamd.core.Services;
using cgspamd.helper.Applications;
using cgspamd.helper.Models;
using cgspamd.helper.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using static cgspamd.core.Utils.Utils;

[assembly: InternalsVisibleTo("cgspamd.tests")]
namespace cgspamd.helper
{
    internal class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json");
            IConfiguration config = builder.Build();
            var appSettings = config.Get<AppSettings>();
            if (appSettings == null)
            {
                Console.Error.WriteLine("* Unable to read appsettings file.");
                return;
            }
            var serviceProvider = new ServiceCollection()
                .AddDbContext<StoreDbContext>(options => options.UseSqlite(appSettings.ConnectionString))
                .AddScoped<DatabaseService>()
                .AddScoped<FilterRulesApplication>()
                .AddScoped<HelperApplication>()
                .AddSingleton<ConsoleOutputService>()
                .AddSingleton<WorkerService>()
                .AddSingleton<AppSettings>(appSettings)
                .BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
            await dbService.InitDatabaseAsync();
            var console = serviceProvider.GetRequiredService<ConsoleOutputService>();
            console.PrintStartupMessage();
            var worker = serviceProvider.GetRequiredService<WorkerService>();
            await worker.Work();
        }
    }
}
