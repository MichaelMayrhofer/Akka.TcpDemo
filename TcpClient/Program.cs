using Akka.Actor;
using System;

namespace TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("Akka-TcpDemo");
            var client = system.ActorOf(Props.Create(() => new Client()), "client");

            Console.WriteLine("press [enter] to start tcp client");
            Console.ReadLine();
            client.Tell(new StartMessage());

            Console.WriteLine("try to stop client automatically");
            client.Tell(new StopMessage());

            Console.WriteLine("press [enter] to close demo");
            Console.ReadLine();
        }
    }
}
