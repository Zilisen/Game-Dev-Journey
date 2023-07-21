using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
namespace EchoServer
{
    class ClientState
    {
        public Socket socket;
        public byte[] readBuff = new byte[1024];
    }

    class MainClass
    {
        // 监听socket
        static Socket listenfd;
        // 客户端sockets及状态信息
        static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

        public static void Main_(string[] args)
        {
            Console.WriteLine("Hello World! ");
            //Socket
            listenfd = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            //Bind
            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(ipAdr, 8888);
            listenfd.Bind(ipEp);
            //Listen
            listenfd.Listen(0);
            Console.WriteLine("[服务器]启动成功");
            //while (true)
            //{
            //    //Accept
            //    Socket connfd = listenfd.Accept();
            //    Console.WriteLine("[服务器]Accept");
            //    //Receive
            //    byte[] readBuff = new byte[1024];
            //    int count = connfd.Receive(readBuff);
            //    string readStr = System.Text.Encoding.Default.GetString(readBuff,
            //                      0, count);
            //    Console.WriteLine("[服务器接收]" + readStr);
            //    //Send
            //    string sendStr = System.DateTime.Now.ToString();
            //    byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
            //    connfd.Send(sendBytes);
            //}
            listenfd.BeginAccept(AcceptCallback, listenfd);
            Console.ReadLine(); // 等待
        }

        //Accept回调
        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine("[服务器]Accept");
                Socket listenfd = (Socket)ar.AsyncState;
                Socket clientfd = listenfd.EndAccept(ar);
                //clients列表
                ClientState state = new ClientState();
                state.socket = clientfd;
                clients.Add(clientfd, state);
                //接收数据BeginReceive
                clientfd.BeginReceive(state.readBuff, 0, 1024, 0,
                    ReceiveCallback, state);
                //继续Accept
                listenfd.BeginAccept(AcceptCallback, listenfd);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket Accept fail" + ex.ToString());
            }
        }

        //Receive回调
        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                ClientState state = (ClientState)ar.AsyncState;
                Socket clientfd = state.socket;
                int count = clientfd.EndReceive(ar);
                //客户端关闭
                if (count == 0)
                {
                    clientfd.Close();
                    clients.Remove(clientfd);
                    Console.WriteLine("Socket Close");
                    return;
                }

                string recvStr =
                    System.Text.Encoding.Default.GetString(state.readBuff, 0, count);
                byte[] sendBytes =
                    System.Text.Encoding.Default.GetBytes(clientfd.RemoteEndPoint.ToString() + " : " + recvStr);
                foreach (ClientState s in clients.Values)
                {
                    s.socket.Send(sendBytes);
                }
                //clientfd.Send(sendBytes); //减少代码量，不用异步
                clientfd.BeginReceive(state.readBuff, 0, 1024, 0,
                    ReceiveCallback, state);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket Receive fail" + ex.ToString());
            }
        }
    }
}