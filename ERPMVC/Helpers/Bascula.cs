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
        
        public async Task<string> ClienteTcpLectura(string ip, int puerto)
        {
            TcpClient cliente = new TcpClient(ip, puerto);
            NetworkStream clientStream = cliente.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;
            string peso = "";

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 8, 14);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    break;
                }
                finally
                {
                    clientStream.Close();
                    cliente.Close();
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                //mainwind.setText();
                peso = encoder.GetString(message);

                Console.WriteLine("Peso:" + peso);
                if (peso.Length>2)
                {
                    break;
                }
            }

            cliente.Close();
            return peso;
        }
    }
}