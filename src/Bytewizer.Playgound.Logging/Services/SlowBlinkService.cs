using System.Threading;

using Bytewizer.TinyCLR.Hosting;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Bytewizer.Playground.Logging
{
    internal class SlowBlinkService : BackgroundService
    {
        private readonly GpioPin _led;

        public SlowBlinkService()
        {
            var ledPin = FEZDuino.GpioPin.Led;

            _led = GpioController.GetDefault().OpenPin(ledPin);
            _led.SetDriveMode(GpioPinDriveMode.Output);
        }

        protected override void ExecuteAsync()
        {
            while (!CancellationRequested)
            {
                _led.Toggle();
                Thread.Sleep(1000);
            }
        }
    }
}