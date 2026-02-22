using cgspamd.core.Services;
using cgspamd.helper.Applications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.helper.Services
{
    
    internal class WorkerService
    {
        private DateTime startDay;
        private CancellationTokenSource cancellationTokenSource = new();
        IServiceProvider _serviceProvider;
        public WorkerService(IServiceProvider provider) 
        {
            _serviceProvider = provider;
            startDay = DateTime.Today;
        }
        public async Task Work()
        {
            _ = MaintainDBAsync();
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                string? line = await Console.In.ReadLineAsync();
                if (line == null)
                {
                    break;
                }
                _ = Task.Run(async () =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var app = scope.ServiceProvider.GetRequiredService<HelperApplication>();
                    await app.ProcessMessageAsync(line, cancellationTokenSource);
                });
            }
        }
        public async Task MaintainDBAsync()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                DateTime todayDay = DateTime.Today;
                while (todayDay==startDay && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    await Task.Delay(60*1000, cancellationTokenSource.Token);
                    todayDay = DateTime.Today;
                }
                startDay = todayDay;
                using var scope = _serviceProvider.CreateScope();
                var app = scope.ServiceProvider.GetRequiredService<DatabaseService>();
                await app.TruncateWalAsync();
            }
        }

    }
}
