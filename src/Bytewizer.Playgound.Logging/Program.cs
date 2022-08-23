using Bytewizer.TinyCLR.Boards;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Hosting.Configuration;
using Bytewizer.TinyCLR.Hosting.Configuration.Json;
using GHIElectronics.TinyCLR.Pins;

namespace Bytewizer.Playground.Logging
{
    internal class Program
    {
        static void Main()
        {
            var hostBuilder = HostBoard.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //services.AddDiagnostics();
                    //services.AddWireless(
                    //    context.Configuration.GetValue("wireless:ssid"),
                    //    context.Configuration.GetValue("wireless:psk")
                    //);

                    services.AddEthernet();
                    services.AddNetworkTime();
                    services.AddHostedService(typeof(NetworkStatusService));
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json", optional:false);
                }); 
    
            var host = hostBuilder.Build();
            
            host.Run();
        }

        //private static byte[] GetMacAddress()
        //{
        //    var i2cController = I2cController.FromName(SC20100.I2cBus.I2c1);
        //    var i2cSettings = new I2cConnectionSettings(0x50);
        //    var i2cDevice = i2cController.GetDevice(i2cSettings);

        //    // Microchip 24AA025E48T EUI-48 Node Address
        //    var writeBuffer = new byte[1] { 0xFA }; // node address value start location
        //    var readBuffer = new byte[6]; // EUI-48 6-byte size

        //    try
        //    {
        //        i2cDevice.WriteRead(writeBuffer, readBuffer);
        //    }
        //    catch
        //    {
        //        readBuffer = new byte[6] { 0x00, 0x8D, 0xB4, 0x49, 0xAD, 0xBD };
        //    }

        //    return readBuffer;
        //}
    }
}