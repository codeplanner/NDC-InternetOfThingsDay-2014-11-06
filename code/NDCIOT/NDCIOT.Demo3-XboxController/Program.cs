using System;
using System.Threading;
using System.Threading.Tasks;
using SharpDX.XInput;
using XSockets.Client40;
using XSockets.Client40.Common.Event.Arguments;

namespace NDCIOT.Demo3_XboxController
{
    class Program
    {
        /// <summary>
        /// Provides access to the XBox360 controller
        /// </summary>
        private static SharpDX.XInput.Controller _controller;

        /// <summary>
        /// Full duplex communication
        /// </summary>
        private static XSocketClient _client;

        /// <summary>
        /// Currenct command state
        /// </summary>
        private static QuintusCommand _command;

        /// <summary>
        /// Flag for game loop
        /// </summary>
        private static bool _gameOn;

        static void Main(string[] args)
        {
            _command = new QuintusCommand();

            _controller = new SharpDX.XInput.Controller(UserIndex.One);

            _client = new XSocketClient("ws://127.0.0.1:4502", "http://localhost", "generic");

            _client.Controller("generic").OnOpen += OnOpen;
            _client.Controller("generic").OnClose += OnClose;

            //When the player hits an enemy the controller will vibrate
            _client.Controller("generic").On<int>("vibrate", duration =>
            {
                _controller.SetVibration(new Vibration { LeftMotorSpeed = 32768, RightMotorSpeed = 32768 });
                Thread.Sleep(duration);
                _controller.SetVibration(new Vibration { LeftMotorSpeed = 0, RightMotorSpeed = 0 });
            });

            _client.Open();

            Console.ReadLine();
        }

        static void OnClose(object sender, EventArgs e)
        {
            _gameOn = false;
            Console.WriteLine("Disconnected, game ended!");
        }

        static void OnOpen(object sender, OnClientConnectArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                _gameOn = true;
                Console.WriteLine("Connected, control the game!");
                while (_gameOn)
                {
                    var state = _controller.GetState();

                    //If there was any change we send a new command
                    if (GetCommand(state))
                    {
                        _client.Controller("generic").Invoke("cmd", _command);
                    }

                    //Let the gameloop wait 30 ms
                    Thread.Sleep(30);
                }
            });
        }

        private static bool GetCommand(State state)
        {
            var changed = false;

            //Collect data
            var aBtn = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
            var upBtn = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp);
            var downBtn = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown);
            var rightBtn = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight);
            var leftBtn = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft);

            //Did anything change since last time?
            if (aBtn != _command.action || upBtn != _command.up || downBtn != _command.down || rightBtn != _command.right ||
                leftBtn != _command.left)
            {
                //Flag that changes occured
                changed = true;

                //Set values since atleast one has changed
                _command.action = aBtn;
                _command.up = upBtn;
                _command.down = downBtn;
                _command.right = rightBtn;
                _command.left = leftBtn;
            }
            return changed;
        }
    }
}
