using Bytewizer.TinyCLR.Boards;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Hosting.Configuration;
using Bytewizer.TinyCLR.Hosting.Configuration.Json;

using GHIElectronics.TinyCLR.Devices.Display;

namespace Bytewizer.Playground.MatrixRain
{
    internal class Program
    {
        static void Main()
        {
            IHost host = HostBoard.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTouchScreen(
                        DisplayOrientation.Degrees0
                    );
                    services.AddWireless(
                        context.Configuration.GetValue("wireless:ssid"),
                        context.Configuration.GetValue("wireless:psk")
                    );
                    services.AddNetworkTime();
                    services.AddHostedService(typeof(MatrixRainService));
                    services.AddHostedService(typeof(NetworkStatusService));
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json", optional: false);
                })
                .Build();

            host.Run();
        }
    }
}