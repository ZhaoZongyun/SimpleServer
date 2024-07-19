using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;

namespace TestUDPServer.Mgr
{
    public class NetTCPMgr
    {
        TcpListener listener;
        private List<Client> clientList;
        IPEndPoint endPoint;

        public void Init()
        {
            clientList = new List<Client>();
            endPoint = new IPEndPoint(IPAddress.Any, 11000);
            listener = new TcpListener(endPoint);

            try
            {
                Console.WriteLine("服务器等待接连接。。。");
                listener.Start();

                while (true)
                {
                    var tcpClient = listener.AcceptTcpClient();
                    Client client = new Client(tcpClient, tcpClient.GetStream());
                    clientList.Add(client);
                    client.Init(OnClientDisconnect);

                    Console.WriteLine($"服务器等接收到连接，来自{tcpClient.Client.RemoteEndPoint}");
                }
            }
            finally
            {
                listener.Stop();
            }
        }

        private void OnClientDisconnect(Client client, Exception exception)
        {
            Console.WriteLine($"移除连接，来自{client.tcpClient.Client.RemoteEndPoint}");
            client.Close();
            clientList.Remove(client);
        }
    }
}