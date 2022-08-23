using System;
using System.Drawing;
using System.Threading;

using Bytewizer.TinyCLR.Boards;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.Playground.MatrixRain.Properties;

using GHIElectronics.TinyCLR.Drivers.FocalTech.FT5xx6;

namespace Bytewizer.Playground.MatrixRain
{
    internal class MatrixRainService : BackgroundService
    {
        private static readonly Random _rand = new Random();

        private static readonly Font _digitalFont
                = Resources.GetFont(Resources.FontResources.RobotoFont);

        private static readonly Font _digitalSmall
                = Resources.GetFont(Resources.FontResources.DigitalSmall);

        private static readonly Font _digitalMedium
                = Resources.GetFont(Resources.FontResources.DigitalMedium);

        private static readonly Font _digitalLarge
                = Resources.GetFont(Resources.FontResources.DigitalLarge);

        private static readonly Font _smallUxIcons
                = Resources.GetFont(Resources.FontResources.SmallUxIcons);

        private readonly SolidBrush _blackBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush _whiteBrush = new SolidBrush(Color.White);
        private readonly SolidBrush _greenBrush = new SolidBrush(Color.Green);

        private readonly int[] _y;
        private readonly int _width;
        private readonly int _height;
        private readonly int _fontWidth = 16;
        private readonly int _fontHeight = 16;

        private readonly Graphics _displayCanvas;
        private readonly IConfiguration _configuration;
        private readonly TouchScreenService _screen;
        
        private static int _touchState = 1;

        public MatrixRainService(TouchScreenService screen, IConfiguration configuration)
        {
            _screen = screen;
            _configuration = configuration;

            _screen.TouchController.TouchDown += Touch_TouchDown;
            
            _displayCanvas = Graphics.FromHdc(_screen.DisplayController.Hdc);
            _displayCanvas.Clear();

            _fontHeight = 16;
            _fontWidth = 16;
            _height = _screen.Height / _fontHeight;
            _width = _screen.Width / _fontWidth;

            _y = new int[_width];

            for (int i = 0; i < _width; ++i)
            {
                _y[i] = _rand.Next(_height);
            }
        }

        protected override void ExecuteAsync()
        {
            _screen.Enable();

            while (!CancellationRequested)
            {
                if (_touchState == 0)
                {
                    DrawDate();
                }
                else
                {
                    DrawRain();
                }
            }
        }

        private void DrawRain()
        {
            int x;
            for (x = 0; x < _width; ++x)
            {
                _displayCanvas.FillRectangle(_blackBrush, x * _fontWidth, _y[x] * _fontHeight, _fontWidth, _fontHeight);
                _displayCanvas.DrawString(AsciiCharacter.ToString(), _digitalFont, _whiteBrush, x * _fontWidth, _y[x] * _fontHeight);

                int temp = _y[x] - 1;
                _displayCanvas.FillRectangle(_blackBrush, x * _fontWidth, InScreenYPosition(temp, _height) * _fontHeight, _fontWidth, _fontHeight);
                _displayCanvas.DrawString(AsciiCharacter.ToString(), _digitalFont, _greenBrush, x * _fontWidth, InScreenYPosition(temp, _height) * _fontHeight);

                int temp1 = _y[x] - 10;
                _displayCanvas.FillRectangle(_blackBrush, x * _fontWidth, InScreenYPosition(temp1, _height) * _fontHeight, _fontWidth, _fontHeight);

                _y[x] = InScreenYPosition(_y[x] + 1, _height);
            }
            _displayCanvas.Flush();

            Thread.Sleep(75);
        }

        private void DrawDate()
        {
            var time = DateTime.Now;

            _displayCanvas.Clear();

            var connected = (bool)_configuration[BoardSettings.WirelessConnected];
                      
            _displayCanvas.DrawTextInRect(
                connected ? "\ue63e" : "\ue648",
                5,
                3,
                _screen.Width - 10,
                _screen.Height - 6,
                Graphics.DrawTextAlignment.AlignmentRight,
                Color.Green,
                _smallUxIcons
            );


            _displayCanvas.DrawTextInRect(
                time.ToString("dddd, MMMM dd yyyy"),
                5,
                5,
                _screen.Width - 10,
                _screen.Height - 10,
                Graphics.DrawTextAlignment.AlignmentLeft,
                Color.Green,
                _digitalSmall
            );

            _displayCanvas.DrawTextInRect(
                time.ToString("hh:mm"),
                30,
                55,
                _screen.Width - 30,
                _screen.Height - 55,
                Graphics.DrawTextAlignment.AlignmentLeft,
                Color.Green,
                _digitalLarge
            );

            _displayCanvas.DrawTextInRect(
                ":",
                335,
                90,
                _screen.Width - 335,
                _screen.Height - 90,
                Graphics.DrawTextAlignment.AlignmentLeft,
                Color.Green,
                _digitalMedium
            );

            _displayCanvas.DrawTextInRect(
                time.ToString("ss"),
                375,
                90,
                _screen.Width - 375,
                _screen.Height - 90,
                Graphics.DrawTextAlignment.AlignmentLeft,
                Color.Green,
                _digitalMedium
            );

            _displayCanvas.DrawTextInRect(
                time.ToString("tt"),
                420,
                75,
                _screen.Width - 420,
                _screen.Height - 75,
                Graphics.DrawTextAlignment.AlignmentLeft,
                Color.Green,
                _digitalSmall
            );

            _displayCanvas.Flush();
            Thread.Sleep(1000);
        }

        private void Touch_TouchDown(FT5xx6Controller sender, TouchEventArgs e)
        {
            if (_touchState == 0)
            {
                for (int i = 0; i < _width; ++i)
                {
                    _y[i] = _rand.Next(_height);
                }
                Interlocked.Increment(ref _touchState);
            }
            else
            {
                Interlocked.Decrement(ref _touchState);
            }
        }

        private static int InScreenYPosition(int yPosition, int height)
        {
            if (yPosition < 0)
                return yPosition + height;
            else if (yPosition < height)
                return yPosition;
            else
                return 0;
        }

        private static char AsciiCharacter
        {
            get
            {
                int t = _rand.Next(10);
                if (t <= 2)
                    return (char)('0' + _rand.Next(10));
                else if (t <= 4)
                    return (char)('a' + _rand.Next(27));
                else if (t <= 6)
                    return (char)('A' + _rand.Next(27));
                else
                    return (char)(_rand.Next(255));
            }
        }
    }
}
