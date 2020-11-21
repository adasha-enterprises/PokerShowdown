using iQmetrix.PokerHandShowdown.Implementations;
using iQmetrix.PokerHandShowdown.Configuration;
using Microsoft.Extensions.DependencyInjection;
using iQmetrix.PokerHandShowdown.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.IO;

namespace iQmetrix.PokerHandShowdown.App
{
    public class Program
    {
        public static IConfigurationRoot Configuration;
        public static IOptions<ConfigSettings> ConfigSettings;

        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingcontext, config) =>
                {
                    if (File.Exists($"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}//appsettings.json"))
                    {
                        config.SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName).AddJsonFile("appsettings.json");
                    }

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }

                    Configuration = config.Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<ConfigSettings>(Configuration);
                    services.AddSingleton<IHostedService, PokerHandShowdown>();
                    services.AddScoped<ICardGameService, PokerGameService>();
                    services.AddScoped<ICardHandCreator, PokerHandCreator>();
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
