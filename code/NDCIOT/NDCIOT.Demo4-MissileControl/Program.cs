using System;
using MissileSharp;
using NDCIOT.Demo4_MissileControl.Model;

namespace NDCIOT.Demo4_MissileControl
{
    /// <summary>
    /// Credits to Christian Specht for writing MissileSharp
    /// </summary>
    class Program
    {             
        static void Main(string[] args)
        {
            var launcher = new CommandCenter(new ThunderMissileLauncher());

            var conn = new XSockets.Client40.XSocketClient("ws://127.0.0.1:4502", "http://localhost", "Missile");

            conn.Open();
            
            //Go to start position
            launcher.Reset();

            //Subscribe to command            
            conn.Controller("Missile").Subscribe<XCommand>("command", x => launcher.RunCommand(new LauncherCommand(x.Command, x.Value)));

            Console.ReadLine();
        }
    }
}
