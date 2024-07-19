using TestUDPServer.Mgr;

namespace TestUDPServer
{
    internal class Program
    {
        public static void Main()
        {
            //NetUDPMgr netMgr = new NetUDPMgr();
            NetTCPMgr netMgr = new NetTCPMgr();
            netMgr.Init();
        }
    }
}
