using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Heimdall.Client.Server
{
    public class CarbonIntensityStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                using (var scope = builder.ApplicationServices.CreateScope())
                {
                    var tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;
                    var gamesRepository = scope.ServiceProvider.GetRequiredService<INationalGridRepository>();
                    _ = Task.Run(() => gamesRepository.TryExecuteAsync(token), token);
                }

                next(builder);
            };
        }
    }
}