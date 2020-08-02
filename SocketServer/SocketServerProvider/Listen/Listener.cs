using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServer.SocketServerProvider.Listen
{
    public static class Listener
    {
        public static void ListenerServer(int port)
        {
            var socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = IPAddress.Any;

            var point = new IPEndPoint(ip, port);

            socketWatch.Bind(point);
            Console.WriteLine("监听成功");

            socketWatch.Listen(10); //设置并发最大值

            var th = new Thread(Listen) {IsBackground = true};
            th.Start(socketWatch);

            while (true)
            {
                
            }
        }

        /// <summary>
        /// 等待客户端的连接 并创建与之通信的socket
        /// </summary>
        /// <param name="obj"></param>
        private static void Listen(object obj)
        {
            var socketWatch = obj as Socket;

            while (true)
            {
                var socketConnect = socketWatch?.Accept(); //接收客户端请求，创建并返回一个负责通信的socket

                // 打印客户端消息
                Console.WriteLine($"{socketConnect?.RemoteEndPoint.ToString()}:连接成功");

                var thReceive = new Thread(Receive){IsBackground = true};
                thReceive.Start(socketConnect);

                var thSend = new Thread(Send) { IsBackground = true };
                thSend.Start(socketConnect);

            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void Send(object obj)
        {
            var socketConnect = obj as Socket;

            while (true)
            {
                try
                {
                    if (socketConnect?.Connected == false) break;
                    Console.WriteLine("输入字符，回车进行发送:");
                    var data = Console.ReadLine();
                    var encodingBytes = System.Text.Encoding.UTF8.GetBytes(data);
                    var res = socketConnect?.Send(encodingBytes);
                }
                catch (Exception e)
                {
                    Console.WriteLine("发送数据异常");
                    Console.WriteLine(e);
                }
            }
        }


        /// <summary>
        /// 服务器端接收客户端发来的消息
        /// </summary>
        /// <param name="obj"></param>
        private static void Receive(object obj)
        {
            var socketConnect = obj as Socket;

            while (true)
            {
                var buffer = new byte[1024 * 1024 * 2];

                // 与远程连接，容易引发异常
                try
                {
                    // 客户端连接成功后，服务器应该接受客户端发来的消息
                    // res 实际接受到的有效字节数
                    if (socketConnect?.Connected == false) break;
                    var count = socketConnect?.Receive(buffer);

                    //socketConnect?.Send(buffer, SocketFlags.None);

                    if (count == null || count == 0) break;

                    var str = Encoding.UTF8.GetString(buffer, 0, (int)count);
                    Console.WriteLine($"客户端{socketConnect.RemoteEndPoint}消息：{str}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("接收数据异常");
                    Console.WriteLine(e);
                }

                
            }
        }

    }
}
