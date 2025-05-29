using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Cliente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string nombre, ip;
            int port;
            Client _client;


            Console.WriteLine("Escribe tu nombre: ");
            nombre = Console.ReadLine();
            Console.WriteLine("Escribe la ip a conectarse: ");
            ip = Console.ReadLine();
            Console.WriteLine("Escribe el puerto: ");
            port = int.Parse(Console.ReadLine());

            _client = new Client(ip, port, nombre);
            _client.Start();
        }
    }
}
