using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public static class Data
{
    public static List<string> data { get; set; }
}
namespace Destruct.Utilities
{
    public class Server
    {
        public static UdpClient udpServer;
        public static TcpListener tcpServer;
        public byte[] bytes;
        public string tcpData;
        public string udpData;
        public static Dictionary<int, TcpClient> tcpClients;
        public static Dictionary<int, UdpClient> udpClients;

        public static void Init()
        {
            IPHostEntry host;
            string localIP = "10.97.161.131";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            IPAddress serverLoc = new IPAddress(localIP.Split('.').Select(i => byte.Parse(i)).ToArray());
            tcpServer = new TcpListener(serverLoc, 13000);
            tcpServer.Start();
        }
        
        public static void ListenUdp()
        {
            while (true)
            {
                try
                {
                    udpClients = new Dictionary<int, UdpClient>();
                    int i = 0;
                    while (true)
                    {
                        var client = tcpServer.AcceptTcpClient();
                        if (!tcpClients.Any(j => ((IPEndPoint)j.Value.Client.RemoteEndPoint).Address.ToString() == ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()))
                            tcpClients.Add(i, client);
                        else
                            break;
                        i++;
                    }
                    for (int j = 0; j < tcpClients.Count; j++)
                    {
                        NetworkStream stream = tcpClients[j].GetStream();
                        byte[] bytes = new byte[256];
                        i = 0;
                        List<string> data = new List<string>();
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            try
                            {
                                if (!Data.data.Any(x => x[0] == System.Text.Encoding.ASCII.GetString(bytes, 0, i).ToUpper()[0]))
                                    Data.data.Add(System.Text.Encoding.ASCII.GetString(bytes, 0, i).ToUpper());
                                else
                                    Data.data.Remove(Data.data.FirstOrDefault(x => x[0] == System.Text.Encoding.ASCII.GetString(bytes, 0, i).ToUpper()[0]));
                            }
                            catch
                            {

                            }
                            //Data.data = data;
                        }
                    }
                }
            catch{}
            }
        }
        public static void ListenTcp()
        {
            while (true)
            {
                try
                {
                    tcpClients = new Dictionary<int, TcpClient>();
                    udpClients = new Dictionary<int, UdpClient>();
                    int i = 0;
                    while (true)
                    {
                        var client = tcpServer.AcceptTcpClient();
                        if (!tcpClients.Any(j => ((IPEndPoint)j.Value.Client.RemoteEndPoint).Address.ToString() == ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()))
                            tcpClients.Add(i, client);
                        else
                            break;
                        i++;
                    }
                    for (int j = 0; j < tcpClients.Count; j++)
                    {
                        NetworkStream stream = tcpClients[j].GetStream();
                        byte[] bytes = new byte[256];
                        i = 0;
                        List<string> data = new List<string>();
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            try
                            {
                                if (!Data.data.Any(x => x[0] == System.Text.Encoding.ASCII.GetString(bytes, 0, i).ToUpper()[0]))
                                    Data.data.Add(System.Text.Encoding.ASCII.GetString(bytes, 0, i).ToUpper());
                                else
                                    Data.data.Remove(Data.data.FirstOrDefault(x => x[0] == System.Text.Encoding.ASCII.GetString(bytes, 0, i).ToUpper()[0]));
                            }
                            catch
                            {

                            }
                            //Data.data = data;
                        }
                    }
                }
            catch{}
            }
        }
    }
    public class Client
    {
        static string server;
        public static string message;

        public static void Init(String serverp, String messagep)
        {
            server = serverp;
            message = messagep;
        }

        public static void Connect()
        {
            while (true)
            {
                try
                {
                    Int32 port = 13000;
                    TcpClient client = new TcpClient(server, port);

                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    NetworkStream stream = client.GetStream();

                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    data = new Byte[256];

                    String responseData = String.Empty;

                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    //Console.WriteLine("Received: {0}", responseData);

                    stream.Close();
                    client.Close();
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }

                Console.WriteLine("\n Press Enter to continue...");
                Console.Read();
            }
        }
    }
}
