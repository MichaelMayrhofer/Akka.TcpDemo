using Akka.Actor;
using Akka.IO;
using System;
using System.Net;

namespace TcpClient
{
    public class Client : ReceiveActor
    {
        private IActorRef connectionActor;

        public Client()
        {
            Receive<StartMessage>(_ => HandleStart());
            Receive<StopMessage>(_ => HandleStop());
            Receive<Tcp.Connected>(m => HandleConnected(m));
            Receive<Tcp.Received>(m => HandleReceived(m));
        }

        private void HandleStart()
        {
            Console.WriteLine("tcp.connecting");
            Context.System.Tcp().Tell(new Tcp.Connect(new IPEndPoint(IPAddress.Loopback, 25603)));
        }

        private void HandleStop()
        {
            // Closes the TCP connection...
            connectionActor?.Tell(Tcp.Close.Instance);
            Context.Stop(Self);
        }

        private void HandleConnected(Tcp.Connected m)
        {
            Console.WriteLine("tcp.connected");
            connectionActor = Sender;
            Sender.Tell(new Tcp.Register(Self));
        }

        private void HandleReceived(Tcp.Received m)
        {
            Console.WriteLine($"tcp.received: {m.Data}");
        }
    }
}