using Akka.Actor;
using Akka.IO;
using System;
using System.Net;

namespace TcpClient
{
    public class Client : ReceiveActor
    {
        public Client()
        {
            Receive<StartMessage>(_ => HandleStart());
            Receive<StopMessage>(_ => HandleStop());
            Receive<Tcp.Connected>(m => HandleConnected(m));
            Receive<Tcp.Received>(m => HandleReceived(m));
        }

        private void HandleStart()
        {
            Context.System.Tcp().Tell(new Tcp.Connect(new IPEndPoint(IPAddress.Loopback, 25603)));
        }

        private void HandleStop()
        {
            // Closes the TCP connection...
            Context.System.Tcp().Tell(Tcp.Close.Instance);
            // ...but causes also the following error message
            // [ERROR][29.04.2019 05:17:24][Thread 0004][akka://Akka-TcpDemo/system/IO-TCP] The
            //  supplied message type is invalid. Only Connect and Bind messages are supported.
            // 
            // Parametername: message
            // Cause: System.ArgumentException: The supplied message type is invalid. Only Conn
            // ect and Bind messages are supported.
            // Parametername: message
            //    bei Akka.IO.TcpManager.Receive(Object message)
            //    bei Akka.Actor.ActorBase.AroundReceive(Receive receive, Object message)
            //    bei Akka.Actor.ActorCell.ReceiveMessage(Object message)
            //    bei Akka.Actor.ActorCell.Invoke(Envelope envelope)
        }

        private void HandleConnected(Tcp.Connected m)
        {
            Console.WriteLine("tcp.connected");
            Sender.Tell(new Tcp.Register(Self));
        }

        private void HandleReceived(Tcp.Received m)
        {
            Console.WriteLine($"tcp.received: {m.Data}");
        }
    }
}