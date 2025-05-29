using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ip;
            int port;
            Server _server;
            try
            {
                Console.WriteLine("Ingresa la IP: ");
                ip = Console.ReadLine();
                Console.WriteLine("Ingresa el puerto: ");
                port = int.Parse(Console.ReadLine());
                _server = new Server(ip, port);
                _server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
            }
        }
    }
}
