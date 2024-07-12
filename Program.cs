using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using ProtobufGen;
namespace TestUDPServer
{
    internal class Program
    {
        public static void Main()
        {
            NetMgr netMgr = new NetMgr();
            netMgr.Init();
        }
    }
}
