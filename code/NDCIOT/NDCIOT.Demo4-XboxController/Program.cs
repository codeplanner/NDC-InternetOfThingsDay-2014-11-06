using System;
using System.Threading;
using System.Threading.Tasks;
using SharpDX.XInput;
using XSockets.Client40;
using XSockets.Client40.Common.Event.Arguments;

namespace NDCIOT.Demo4_XboxController
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
        private static MissileCommand _command;

        /// <summary>
        /// Flag for game loop
        /// </summary>
        private static bool _gameOn;

        static void Main(string[] args)
        {
            _command = new MissileCommand();

            _controller = new SharpDX.XInput.Controller(UserIndex.One);

            _client = new XSocketClient("ws://127.0.0.1:4502", "http://localhost", "missile");

            _client.Controller("missile").OnOpen += OnOpen;
            _client.Controller("missile").OnClose += OnClose;           

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
                Console.WriteLine("Connected, control the missile launcher!");
                while (_gameOn)
                {
                    var state = _controller.GetState();

                    //If there was any change we send a new command
                    GetCommand(state);

                    if (_command.Up)
                    {
                        _client.Controller("missile").Invoke("command", new {Command="up",Value=10});
                        
                    }
                    if (_command.Down)
                    {
                        _client.Controller("missile").Invoke("command", new { Command = "down", Value = 10 });
                        
                    }
                    if (_command.Left)
                    {
                        _client.Controller("missile").Invoke("command", new { Command = "left", Value = 10 });
                        
                    }
                    if (_command.Right)
                    {
                        _client.Controller("missile").Invoke("command", new { Command = "right", Value = 10 });
                        
                    }
                    if (_command.Fire)
                    {
                        _client.Controller("missile").Invoke("command", new { Command = "fire", Value = 1 });
                        Thread.Sleep(100);
                    } 

                    //Let the gameloop wait 30 ms
                    Thread.Sleep(30);
                }
            });
        }

        private static void GetCommand(State state)
        {
            _command.Right = state.Gamepad.LeftThumbX > 6000;
            _command.Left = state.Gamepad.LeftThumbX < -6000;
            _command.Up = state.Gamepad.LeftThumbY > 6000;
            _command.Down = state.Gamepad.LeftThumbY < -6000;
            _command.Fire = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
        }        
    }
}
