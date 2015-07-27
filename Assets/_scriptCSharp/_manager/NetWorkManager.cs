using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;
public class NetWorkManager : MonoBehaviour {
    Socket client;
    int Pivot = 0;
    byte[] FinalData = new byte[1024 * 10];
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Connect(string ip, int port)
    {
        IPAddress ipAddress = IPAddress.Parse(ip);
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
        client.NoDelay = true;

    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void Receive(Socket client)
    {
        try
        {
            StateObject state = new StateObject();
            state.workSocket = client;
            client.BeginReceive(state.buffer, 0, 1024, 0, new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            int bytesRead = client.EndReceive(ar);
            if (bytesRead > 0)
            {
                AppendBytes(FinalData, ref Pivot, state.buffer, bytesRead);
                client.BeginReceive(state.buffer, 0, 1024, 0,new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                if (Pivot > 0)
                {
                    //这里对FinalData进行处理  一般是将数据丢入一个队列中，循环遍历队列，取出数据进行处理
                }
                FinalData = new byte[1024 * 10];
                Pivot = 0;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    public void Send(Socket client, byte[] byteData)
    {
        client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
    }



    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;
            int bytesSent = client.EndSend(ar);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

 

    void AppendBytes(byte[] to, ref int curPivot, byte[] from, int len)
    {
        if(from == null || from.Length == 0)
            return;
        for (int i = curPivot; i < len + curPivot; i++)
            to[i] = from[i - curPivot];
        curPivot += from.Length;
    }
}

class StateObject
{
    // Client socket.
    public Socket workSocket = null;
    // Receive buffer.
    public byte[] buffer = new byte[1024];

}
