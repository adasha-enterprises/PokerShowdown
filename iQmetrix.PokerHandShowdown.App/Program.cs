using iQmetrix.PokerHandShowdown.Implementations;
using iQmetrix.PokerHandShowdown.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace iQmetrix.PokerHandShowdown.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    services.AddSingleton<IHostedService, PokerHandShowdown>();
                    services.AddScoped<ICardGameService, PokerGameService>();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                    logging.AddFilter("Microsoft", LogLevel.Warning); //Try and suppress excessive EF Log output.
                    logging.AddConsole();
                });

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
