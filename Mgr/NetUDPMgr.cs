using System;
using System.Net.Sockets;
using System.Net;

namespace TestUDPServer
{
    public class NetUDPMgr
    {
        private UdpClient udpClient;
        private IPEndPoint endPoint;

        public void Init()
        {
            udpClient = new UdpClient(11000);
            endPoint = new IPEndPoint(IPAddress.Any, 11000);

            while (true)
            {
                Console.WriteLine("服务器等待接收消息。。。");
                byte[] bytes = udpClient.Receive(ref endPoint);
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                Console.WriteLine($"接收到消息：{message}，来自{endPoint}");

                string send = "服务器收到了：" + message;
                byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(send);
                udpClient.Send(sendBytes, sendBytes.Length, endPoint);
            }
        }
    }
}