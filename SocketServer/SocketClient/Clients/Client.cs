using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServer.SocketClient.Clients
{
    public static class Client
    {
        public static void ClientHandle(string ip, int port)
        {
            var socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //确定IP(地址)和point(端口)
                var ipAddress = IPAddress.Parse(ip);
                var point = new IPEndPoint(ipAddress, port);

                socketClient.Connect(point);

                Console.WriteLine("连接成功！");
            }
            catch (Exception)
            {
                Console.WriteLine("连接错误。");
                return;
            }
            

            var thSend = new Thread(ClientSendData){IsBackground = true};
            thSend.Start(socketClient);

            var thReceive = new Thread(Receive) { IsBackground = true};
            thReceive.Start(socketClient);


            while (true)
            {
            }
        }

        private static void Receive(object obj)
        {
            var socketClient = obj as Socket;

            while (true)
            {
                var buffer = new byte[1024 * 1024 * 2];

                // 与远程连接，容易引发异常
                try
                {
                    // 客户端连接成功后，服务器应该接受客户端发来的消息
                    // res 实际接受到的有效字节数
                    if (socketClient?.Connected == false) break;
                    var count = socketClient?.Receive(buffer);

                    //socketConnect?.Send(buffer, SocketFlags.None);

                    if (count == null || count == 0) break;

                    var str = Encoding.UTF8.GetString(buffer, 0, (int)count);
                    Console.WriteLine($"服务器{socketClient.RemoteEndPoint}消息：{str}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("接收数据异常");
                    Console.WriteLine(e);
                }


            }
        }

        private static void ClientSendData(object obj)
        {
            while (true)
            {
                try
                {
                    var socketClient = obj as Socket;
                    Console.WriteLine("输入字符，回车进行发送。");
                    var data = Console.ReadLine();
                    var encodingBytes = Encoding.UTF8.GetBytes(data);
                    var res = socketClient?.Send(encodingBytes);
                }
                catch (Exception e)
                {
                    Console.WriteLine("发送异常");
                    Console.WriteLine(e);
                }
            }
        }
    }
}
