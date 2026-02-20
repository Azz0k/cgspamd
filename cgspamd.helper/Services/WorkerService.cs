using cgspamd.helper.Applications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.helper.Services
{
    internal class WorkerService
    {
        private CancellationTokenSource cancellationTokenSource = new();
        IServiceProvider _serviceProvider;
        public WorkerService(IServiceProvider provider) 
        {
            _serviceProvider = provider;
        }
        public async Task Work()
        {
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
    }
}
