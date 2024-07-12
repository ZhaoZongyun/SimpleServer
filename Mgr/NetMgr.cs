using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using ProtobufGen;

namespace TestUDPServer
{
    public partial class NetMgr
    {
        private const int port = 11000;
        private UdpClient udpClient;
        private IPEndPoint endPoint;

        public void Init()
        {
            udpClient = new UdpClient(port);
            endPoint = new IPEndPoint(IPAddress.Any, port);

            AddNetMessageEvent(NetMessageId.ReqRegister, OnReqRegister);
            AddNetMessageEvent(NetMessageId.ReqLogin, OnReqLogin);

            while (true)
            {
                Console.WriteLine("服务器等待接收消息。。。");
                byte[] bytes = udpClient.Receive(ref endPoint);
                //消息id，前两位
                short id = GetInt16(bytes, 0);
                NetMessageId messageId = (NetMessageId)id;
                byte[] contentBytes = bytes.Skip(2).Take(bytes.Length - 2).ToArray();
                Console.WriteLine($"从客户端({endPoint})接收到消息 " + messageId.ToString());

                OnReceive(messageId, contentBytes);
            }
        }

        private void OnReqRegister(IMessage message)
        {
            ReqRegister req = message as ReqRegister;
            Console.WriteLine($"注册用户名: " + req.Username);
            Console.WriteLine($"注册密码: " + req.Password);

            ResRegister(0, 1, req.Username);
        }

        private void OnReqLogin(IMessage message)
        {
            ReqLogin req = message as ReqLogin;
            Console.WriteLine($"登录用户名: " + req.Username);
            Console.WriteLine($"登录密码: " + req.Password);

            ResLogin(0, 1, req.Username);
        }

        public void Send(NetMessageId messageId, byte[] totalBytes)
        {
            udpClient.Send(totalBytes, totalBytes.Length, endPoint);
        }

        /// <summary>
        /// 从字节数组取一个short
        /// </summary>
        private static short GetInt16(byte[] bytes, int offset)
        {
            return (short)(bytes[offset] << 8 | bytes[offset + 1]);
        }

        /// <summary>
        /// 从字节数组取一个int
        /// </summary>
        private static int GetInt32(byte[] bytes, int offset)
        {
            return bytes[offset] << 24 | bytes[offset + 1] << 16 | bytes[offset + 2] << 8 | bytes[offset + 3];
        }

        #region 应用层对内

        private Dictionary<NetMessageId, List<Action<IMessage>>> _callbackDict = new Dictionary<NetMessageId, List<Action<IMessage>>>();

        /// <summary>
        /// 添加网络消息监听
        /// </summary>
        public void AddNetMessageEvent(NetMessageId id, Action<IMessage> action)
        {
            if (!_callbackDict.ContainsKey(id))
            {
                _callbackDict.Add(id, new List<Action<IMessage>>());
                _callbackDict[id].Add(action);
            }
            else
            {
                if (!_callbackDict[id].Contains(action))
                    _callbackDict[id].Add(action);
            }
        }

        /// <summary>
        /// 调用网络消息监听
        /// </summary>
        private void CallNetMessageEvent(NetMessageId id, IMessage data)
        {
            List<Action<IMessage>> list;
            _callbackDict.TryGetValue(id, out list);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i](data);
                }
            }
        }

        #endregion

        /// <summary>
        /// 根据消息id和proto实例创建一条消息的完整字节数组
        /// </summary>
        private byte[] CreateTotalBytes(NetMessageId messageId, IMessage data)
        {
            byte[] contentBytes = data.ToByteArray();
            int totalLength = contentBytes.Length + 2;
            byte[] allBytes = new byte[totalLength];

            ushort id = (ushort)messageId;
            allBytes[0] = (byte)(id >> 8);
            allBytes[1] = (byte)id;

            Array.Copy(contentBytes, 0, allBytes, 2, contentBytes.Length);

            return allBytes;
        }
    }
}