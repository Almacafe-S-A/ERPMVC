using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ERPMVC.Helpers
{
    public class Bascula
    {
    }

    class Transmitter
    {
        public Boolean Transmit(String ip, String port, String data)
        {
            TcpClient client = new TcpClient();
            int _port = 0;
            int.TryParse(port, out _port);
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), _port);
            client.Connect(serverEndPoint);
            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(data);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
            return true;
        }
    }

    public class Listener
    {

        private TcpListener tcpListener;
        private Thread listenThread;
        // Set the TcpListener on port 13000.
        Int32 port = 8081;
        IPAddress localAddr = IPAddress.Parse("192.168.1.3");
        Byte[] bytes = new Byte[256];
        //MainWindow mainwind = null;

        public Listener(int puerto,string IP)
        {
            port = puerto;
            localAddr = IPAddress.Parse(IP);
        }
        public void Server()
        {
            //mainwind = wind;
            this.tcpListener = new TcpListener(IPAddress.Any, port);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();

        }

        public void ServerStop()
        {
            //mainwind = wind;
            this.listenThread.Suspend();

        }
        private void ListenForClients()
        {
            this.tcpListener.Start();


            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                //Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                //clientThread.Start(client);
                
                string peso = HandleClientComm(client);
            }
        }
        public string HandleClientComm(object cliente)
        {
            this.tcpListener.Start();
            TcpClient client = this.tcpListener.AcceptTcpClient();

            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }

            }


            tcpClient.Close();
            ASCIIEncoding encoder = new ASCIIEncoding();
            return encoder.GetString(message, 0, bytesRead);
        }
    }

}
