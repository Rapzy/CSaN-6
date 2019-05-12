using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener tcpListener;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(localAddr, 3333);
            tcpListener.Start();

            Console.WriteLine("Waiting for connections... ");
            TcpClient client = tcpListener.AcceptTcpClient();
            Console.WriteLine("Client connected. ");
            NetworkStream netStream;

            while (true)
            {
                Console.WriteLine("Waiting for requests...");
                netStream = client.GetStream();
                byte[] data = new byte[256];
                //int bytes = netStream.Read(data, 0, data.Length);
                BinaryFormatter formatter = new BinaryFormatter();
                Request request = (Request)formatter.Deserialize(netStream);
                if (request is RequestFile)
                {
                    RequestFile req = (RequestFile)request;
                    byte[] response = Tasks.SendBlock(req.FileName, req.Offset, req.Length);
                    netStream.Write(response, 0, response.Length);
                    Console.WriteLine("Sended {0} bytes from {1}", response.Length, req.FileName);
                }
            }
            netStream.Close();
            client.Close();
            Console.WriteLine("\nPress <Enter> to terminate the server.");
            Console.ReadLine();
        }
    }
}
