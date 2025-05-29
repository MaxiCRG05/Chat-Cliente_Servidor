using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat_Server
{
    class ClientInformation
    {
        public string name;
        public Socket socket;

        public ClientInformation(Socket s, string nombre)
        {
            name = nombre;
            socket = s;
        }

        public void MostrarInformacion()
        {
            Console.WriteLine($"Nombre del usuario: {name}\t Socket de {name}: {socket}");
        }

    }

    class Server
    {
        private IPHostEntry _host;
        private IPAddress _address;
        private IPEndPoint _ep;
        private Socket server_Socket;
        private Socket client_Socket;
        private List<ClientInformation> _clients = new List<ClientInformation>();
        
        public Server(string ip, int port)
        {
            try
            {
                _host = Dns.GetHostEntry(IPAddress.Parse(ip));
                _address = _host.AddressList[0];
                _ep = new IPEndPoint(_address, port);

                server_Socket = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                server_Socket.Bind(_ep);
                server_Socket.Listen(100);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
                Console.ReadKey();
            }
            
        }

        public string Receive_Name(Socket client)
        {
            byte[] buffer = new byte[1024];
            int bytesReceived = client.Receive(buffer);
            return byte2string(buffer.Take(bytesReceived).ToArray());
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

        public void SendMessageToClient(string sender, string clientName, string message)
        {
            var client = _clients.FirstOrDefault(c => c.name.Equals(clientName, StringComparison.OrdinalIgnoreCase));
            if (client != null)
            {
                byte[] msgBuffer = Encoding.UTF8.GetBytes($"{sender} te envió:\n {message}\0");
                client.socket.Send(msgBuffer);
            }
            else
            {
                SendMessageToClient("Server", sender ,$"El usuario {clientName} no está conectado.");
            }
        }

        public void Client_Connection(object s)
        {
            ClientInformation client = (ClientInformation)s;
            Socket socket = client.socket;
            byte[] buffer;

            try
            {
                while (true)
                {
                    buffer = new byte[1024];
                    int bytesReceived = socket.Receive(buffer);
                    string message = byte2string(buffer);
                    string[] parts = message.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    Console.WriteLine($"Mensaje recibido. {message}");
                    if (parts.Length >= 3)
                    {
                        string sender = parts[0];
                        string msg = parts[1];
                        string recipient = parts[2];

                        SendMessageToClient(sender, recipient, msg);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Se ha desconectado {client.name}.");
                _clients.Remove(client);
            }
        }

        public void Start()
        {
            Thread _thread;
            while(true)
            {
                try
                {
                    Console.WriteLine("Esperando conexiones...");
                    client_Socket = server_Socket.Accept();
                    string client_name = Receive_Name(client_Socket);
                    var clientInfo = new ClientInformation(client_Socket, client_name);
                    _clients.Add(clientInfo);
                    _thread = new Thread(Client_Connection);
                    _thread.Start(clientInfo);
                    Console.WriteLine($"Se ha conectado: {clientInfo.name}.");
                    SendMessageToClient("Server: ", clientInfo.name, "Se ha conectado correctamente.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Se ha producido una excepción: {e.Message}");
                }
            }
        }
    }
}