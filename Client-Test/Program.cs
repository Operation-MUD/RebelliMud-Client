using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Client_Test
{
    class Program
    {
        private static Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            Console.Title = "Client";
            LoopConnect();
            SendLoop();
            Console.ReadLine();
        }

        private static void SendLoop()
        {
            while (true)
            {
                Console.Write("What would you like to do?\n");
                string req = Console.ReadLine();
                byte[] buf = Encoding.ASCII.GetBytes(req);
                clientSock.Send(buf);

                Recv();
            }
        }

        private static void Recv()
        {
            byte[] rBuf = new byte[1024];
            int rec = clientSock.Receive(rBuf);
            byte[] data = new byte[rec];
            Array.Copy(rBuf, data, rec);
            string recv = Encoding.ASCII.GetString(data);

            string[] temp =  recv.Split(">");
            string chMode = string.Empty;

            if (temp[0].Contains("001"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                chMode = "[SYSTEM] : ";
            }
            else if (temp[0].Contains("002"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ResetColor();
            }

            for (int i = 1; i < temp.Length; i++)
            {
                Console.Write(chMode + temp[i] + "\n");
            }

            Console.ResetColor();
        }

        private static void LoopConnect()
        {
            int attempts = 0;
            while (!clientSock.Connected)
            {
                try
                {
                    attempts++;
                    clientSock.Connect(IPAddress.Loopback, 100);
                }
                catch(SocketException)
                {
                    Console.Clear();
                    Console.WriteLine("Attempts : " + attempts.ToString());
                }
                
            }
            Console.Clear();
            Console.WriteLine("Connected");
           
        }
    }
}
