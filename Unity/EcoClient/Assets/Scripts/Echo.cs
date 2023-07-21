using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Echo : MonoBehaviour
{
    // 套接字
    Socket socket;
    // UGUI    
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button sendButton;
    //接收缓冲区
    byte[] readBuff = new byte[1024];
    string recvStr = "";

    private void Awake()
    {
        connectButton.onClick.AddListener(ConnectAsync);
        sendButton.onClick.AddListener(SendAsync);
    }

    // 点击连接按钮
    private void Connect()
    {
        //Socket
        socket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        //Connect
        socket.Connect("127.0.0.1", 8888);
    }

    //点击发送按钮
    public void Send()
    {
        //Send
        string sendStr = inputField.text;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
        socket.Send(sendBytes);
        //Recv
        byte[] readBuff = new byte[1024];
        int count = socket.Receive(readBuff);
        string recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
        text.text = recvStr;
        //Close
        socket.Close();
    }

    //点击连接按钮,异步
    public void ConnectAsync()
    {
        //Socket
        socket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        //Connect
        socket.BeginConnect("127.0.0.1", 8888, ConnectCallback, socket);
    }

    //Connect回调
    public void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Succ");
            socket.BeginReceive(readBuff, 0, 1024, 0,
                ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Connect fail" + ex.ToString());
        }
    }

    //Receive回调
    public void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);
            recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count) + "\n" + recvStr;

            socket.BeginReceive(readBuff, 0, 1024, 0,
                ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Receive fail" + ex.ToString());
        }
    }

    //点击发送按钮
    public void SendAsync()
    {
        //Send
        string sendStr = inputField.text;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
        //socket.Send(sendBytes);
        socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallBack, socket);

        //不需要Receive了
    }

    private void SendCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndSend(ar);
            Debug.Log("Socket Send Success: " + count);
        }
        catch(SocketException ex)
        {
            Debug.Log("Socket Send fail" + ex.ToString());
        }
    }

    public void Update()
    {
        text.text = recvStr;
    }

    //public void Update()
    //{
    //    if (socket == null)
    //    {
    //        return;
    //    }
    //    //填充checkRead列表
    //    checkRead.Clear();
    //    checkRead.Add(socket);
    //    //select
    //    Socket.Select(checkRead, null, null, 0);
    //    //check
    //    foreach (Socket s in checkRead)
    //    {
    //        byte[] readBuff = new byte[1024];
    //        int count = socket.Receive(readBuff);
    //        string recvStr =
    //            System.Text.Encoding.Default.GetString(readBuff, 0, count);
    //        text.text = recvStr;
    //    }
    //}

}

