using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Loopback, 25603);
            try
            {
                server.Start();
                Console.WriteLine("tcp listener started");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"cannot start tcp server: {ex.Message}");
                Console.ReadLine();
                return;
            }

            while (true)
            {
                Console.WriteLine("wait for client");
                var client = server.AcceptTcpClient();
                Console.WriteLine("client connected");
                var stream = client.GetStream();

                bool clientConnected = true;
                while (clientConnected)
                {
                    var data = Encoding.ASCII.GetBytes(DateTime.Now.ToString());
                    try
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("client disconnected");
                        clientConnected = false;
                        continue;
                    }

                    Console.WriteLine("write data");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
