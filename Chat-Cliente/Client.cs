using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat_Cliente
{
    class Client
    {
        private IPHostEntry _host;
        private IPAddress _address;
        private IPEndPoint _ep;
        private Socket _socket;
        private string name;
        private bool isActiveMenu = true;
        private bool isMenuActive;

        public Client(string ip, int port, string user) 
        {
            _host = Dns.GetHostEntry(IPAddress.Parse(ip));
            _address = _host.AddressList[0];
            _ep = new IPEndPoint(_address, port);
            name = user;
            _socket = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            _socket.Connect(_ep);
            Send_Name();
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
            Console.WriteLine("Conectado al servidor.");
            ShowMenu();
        }

        public void Send_Name()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(name + "\0");
            _socket.Send(buffer);
        }

        public void SendMessage(string recipient, string message)
        {
            string fullMessage = $"{name}\\{message}\\{recipient}\\";
            byte[] buffer = Encoding.UTF8.GetBytes(fullMessage);
            _socket.Send(buffer);
            Console.WriteLine($"Mensaje enviado. {fullMessage}");
            Console.ReadKey();
        }

        public void SendMessages()
        {
            Console.Write("Ingresa el nombre del destinatario: ");
            string recipient = Console.ReadLine();
            Console.Write("Ingresa tu mensaje: ");
            string message = Console.ReadLine();
            SendMessage(recipient, message);
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Hola {name}, por favor selecciona una opción:");
                Console.WriteLine("--- Menú ---");
                Console.WriteLine("Selecciona una opción: ");
                Console.WriteLine("1. Enviar un mensaje");
                Console.WriteLine("2. Salir");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        SendMessages();
                        break;
                    case "2":
                        Stop();
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Intenta de nuevo.");
                        break;
                }
            }
        }

        public void ReceiveMessages()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesReceived = _socket.Receive(buffer);
                    string message = byte2string(buffer.Take(bytesReceived).ToArray());
                    Console.WriteLine($"Mensaje recibido: {message}");
                }
                catch (SocketException)
                {
                    Console.WriteLine("Se ha perdido la conexión con el servidor.");
                    break;
                }
            }
        }

        public void Stop()
        {
            isMenuActive = false;
            _socket.Close();
            Console.WriteLine("Se ha detenido correctamente el cliente.");
            Console.ReadKey();
        }

        public string byte2string(byte[] buffer)
        {
            string message;
            int endindex;

            message = Encoding.UTF8.GetString(buffer);
            endindex = message.IndexOf('\0');
            if (endindex > 0)
                message = message.Substring(0, endindex);
            return message;
        }
    }
}