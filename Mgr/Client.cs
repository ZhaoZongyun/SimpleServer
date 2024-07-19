using System.Net.Sockets;
using System.Threading;
using System;

namespace TestUDPServer.Mgr
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class Client
    {
        internal TcpClient tcpClient;
        internal NetworkStream stream;
        internal byte[] receiveBuffer;
        Thread thread;
        private Action<Client, Exception> disconnectCallback;

        public Client(TcpClient tcpClient, NetworkStream stream)
        {
            this.tcpClient = tcpClient;
            this.stream = stream;
            receiveBuffer = new byte[1024];
        }

        internal void Init(Action<Client, Exception> disconnectCallback)
        {
            this.disconnectCallback = disconnectCallback;

            // 接收消息，方式一，异步调用 + 尾递归
            Receive();

            // 接收消息，方式二，线程阻塞
            //thread = new Thread(new ThreadStart(ReceiveThread));
            //thread.Start();
        }

        void Receive()
        {
            try
            {
                stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReadCallback, stream);
            }
            catch (Exception e)
            {
                Console.WriteLine("11111111: " + e.Message);
                disconnectCallback(this, e);
                return;
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            int length = 0;
            try
            {
                length = stream.EndRead(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine("2222222222: " + e.Message);
                disconnectCallback(this, e);
                return;
            }

            if (length == 0)
            {
                Console.WriteLine("客户端主动断开连接");
                disconnectCallback(this, new Exception("客户端主动断开连接"));
                return;
            }

            string message = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);

            Console.WriteLine($"接收到消息：{message}");
            string send = "服务器收到了：" + message;
            Send(send);

            // 尾递归
            Receive();
        }

        // 接收线程
        void ReceiveThread()
        {
            while (true)
            {
                int length = tcpClient.Available;
                if (tcpClient.Connected && length > 0)
                {
                    stream.Read(receiveBuffer, 0, length);
                    string message = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);

                    Console.WriteLine($"接收到消息：{message}");

                    string send = "服务器收到了：" + message;
                    Send(send);
                }
            }
        }

        public void Send(string message)
        {
            byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(message);

            // 发送方式一，用 NetworkStream
            stream.Write(sendBytes, 0, sendBytes.Length);

            // 发送方式二，用 TcpClient 的 Client（Socket）
            //int length = tcpClient.Client.Send(sendBytes);
            //Console.WriteLine($"发送字节数：{length}");
        }

        public void Close()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
        }
    }
}