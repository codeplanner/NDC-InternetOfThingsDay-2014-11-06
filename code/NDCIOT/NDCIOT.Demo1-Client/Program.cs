using System;
using XSockets.Client40;

namespace NDCIOT.Demo1_Client
{
    class Program
    {        
        static void Main(string[] args)
        {
            var conn = new XSocketClient("ws://localhost:4502", "http://localhost", "ChatState");

            var chat = conn.Controller("ChatState");
            chat.OnOpen += (sender, connectArgs) =>
            {
                chat.SetEnum("Color", "Blue");
                Console.WriteLine("Controller Open");
                Console.WriteLine("Hit enter to quit...");
            };

            chat.On<string>("sometopic", Console.WriteLine);

            conn.Open();

            Console.ReadLine();
        }        
    }
}
