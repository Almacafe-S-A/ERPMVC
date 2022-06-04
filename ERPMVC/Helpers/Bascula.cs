using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ERPMVC.Helpers
{
    public class Bascula
    {
    }


    class Listener
    {

        private TcpListener tcpListener;
        private Thread listenThread;
        // Set the TcpListener on port 13000.
        Int32 port = 90;
        IPAddress localAddr = IPAddress.Parse("192.168.0.7");
        Byte[] bytes = new Byte[256];
        //MainWindow mainwind = null;
        //public void Server(MainWindow wind)
        public void Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, port);
            //this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();

        }
        public async Task<string> ListenForClients()
        {
           // this.tcpListener = new TcpListener(localAddr., port);
            //this.tcpListener.Start();
            string peso = "";

            while (peso.Length<1)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                //Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                //clientThread.Start(client);
                peso = await HandleClientComm(client);
            }
            //this.tcpListener.Stop();
            return peso;
        }
        public async Task<string> HandleClientComm(object client)
        {
            //TcpClient tcpClient = (TcpClient)client;

            TcpClient cliente = new TcpClient("192.168.0.7", 90);
            NetworkStream clientStream = cliente.GetStream();



            byte[] message = new byte[4096];
            int bytesRead;
            string peso = "";

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 8);
                }
                catch(Exception ex)
                {
                    //a socket error has occured
                    // System.Windows.MessageBox.Show("socket");
                    Console.WriteLine("Error:" + ex.Message);
                    break;
                }
                finally
                {
                    clientStream.Close();
                    cliente.Close();
                }

                //if (bytesRead == 0)
                //{
                //    //the client has disconnected from the server
                //    // System.Windows.MessageBox.Show("disc");
                //    break;
                //}

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                //mainwind.setText();
                peso = encoder.GetString(message);

                Console.WriteLine("Peso:" + peso);
                if (peso.Length>2)
                {
                    break;
                }
                //System.Windows.MessageBox.Show(encoder.GetString(message, 0, bytesRead));
                // System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
            }

            cliente.Close();
            return peso;
        }
    }
}