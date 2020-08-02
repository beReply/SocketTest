using System;
using SocketServer.SocketClient.Clients;
using SocketServer.SocketServerProvider.Listen;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            FeatureSelect();

            Console.ReadLine();
        }


        static void FeatureSelect()
        {
            while (true)
            {
                Show();
                var key = Console.ReadLine();
                if (key == "s")
                {
                    Console.Write("请输入端口:");
                    var port = Convert.ToInt32(Console.ReadLine());
                    Listener.ListenerServer(port);
                }

                if (key == "c")
                {
                    Console.Write("请输入ip:");
                    var ip = Console.ReadLine();
                    Console.Write("请输入端口:");
                    var port = Convert.ToInt32(Console.ReadLine());
                    Client.ClientHandle(ip, port);
                }
                if (key == "h")
                {
                    Show();
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }


        static void Show()
        {
            Console.WriteLine("命令");
            Console.WriteLine("     s  服务端");
            Console.WriteLine("     c  客户端");
            Console.WriteLine("     h  帮助");
        }
    }
}