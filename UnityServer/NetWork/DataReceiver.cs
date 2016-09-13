using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

public class DataReceiver
{
	public Socket listenSock;
	Queue<TcpPacket> msgs;

	object receiveLock;

    AsyncCallback asyncAcceptCallback;
    AsyncCallback asyncReceiveLengthCallBack;
	AsyncCallback asyncReceiveDataCallBack;

	public DataReceiver(Queue<TcpPacket> newQueue, IPAddress newAddress, int newPort, object newLock)
	{
		listenSock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		listenSock.Bind (new IPEndPoint (newAddress, newPort));
		listenSock.Listen (10);

		msgs = newQueue;
		receiveLock = newLock;

        asyncAcceptCallback = new AsyncCallback (HandleAsyncAccept);
		asyncReceiveLengthCallBack = new AsyncCallback (HandleAsyncReceiveLength);
		asyncReceiveDataCallBack = new AsyncCallback (HandleAsyncReceiveData);

		listenSock.BeginAccept (asyncAcceptCallback, (Object)listenSock);
	}

	public void HandleAsyncAccept(IAsyncResult asyncResult)
	{
		Socket listenSock = (Socket) asyncResult.AsyncState;
		Socket clientSock = listenSock.EndAccept (asyncResult);

        Console.WriteLine(clientSock.RemoteEndPoint.ToString() + " 접속");

        TcpClient tcpClient = new TcpClient (clientSock);

        listenSock.BeginAccept(asyncAcceptCallback, (Object)listenSock);

        AsyncData asyncData = new AsyncData (clientSock);
		clientSock.BeginReceive (asyncData.msg, 0, UnityServer.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (Object)asyncData);
	}

	public void HandleAsyncReceiveLength(IAsyncResult asyncResult)
	{
		AsyncData asyncData = (AsyncData) asyncResult.AsyncState;
		Socket clientSock = asyncData.clientSock;

		try
		{
			asyncData.msgSize = (short) clientSock.EndReceive (asyncResult);
		}
		catch
		{
			return;
		}

        //데이터를 받았을 때
        if (asyncData.msgSize >= UnityServer.packetLength)
		{
            short msgSize = 0;

            try
            {
                msgSize = BitConverter.ToInt16(asyncData.msg, 0);
                asyncData = new AsyncData(clientSock);
                clientSock.BeginReceive(asyncData.msg, 0, msgSize + UnityServer.packetId, SocketFlags.None, asyncReceiveDataCallBack, (Object)asyncData);
            }
            catch
            {
                Console.WriteLine("DataReceiver::BitConverter 에러");
                asyncData = new AsyncData(clientSock);
                clientSock.BeginReceive(asyncData.msg, 0, UnityServer.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (Object)asyncData);
            }
        }
		else
        {
            asyncData = new AsyncData(clientSock);
            clientSock.BeginReceive(asyncData.msg, 0, UnityServer.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (Object)asyncData);
        }
	}

	public void HandleAsyncReceiveData(IAsyncResult asyncResult)
	{
		AsyncData asyncData = (AsyncData) asyncResult.AsyncState;
		Socket clientSock = asyncData.clientSock;

		try
		{
			asyncData.msgSize = (short) clientSock.EndReceive (asyncResult);
		}
		catch
		{
			return;
		}

        if (asyncData.msgSize >= UnityServer.packetId)
		{
			Array.Resize (ref asyncData.msg, asyncData.msgSize);
            TcpPacket paket = new TcpPacket (asyncData.msg, clientSock);

            lock (receiveLock)
            {
                try
                {
                    msgs.Enqueue (paket);
				}
				catch
				{
					Console.WriteLine ("DataReceiver::Enqueue 에러");
				}
			}
		}

		asyncData = new AsyncData(clientSock);
		clientSock.BeginReceive (asyncData.msg, 0, UnityServer.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (Object)asyncData);
	}
}


public class AsyncData
{
	public Socket clientSock;
	public byte[] msg;
	public short msgSize;
	public const int msgMaxLength = 1024;

	public AsyncData (Socket newClient)
	{
		msg = new byte[msgMaxLength];
		clientSock = newClient;
	}
}

public class TcpPacket
{
	public byte[] msg;
	public Socket client;

	public TcpPacket (byte[] newMsg, Socket newclient)
	{
		msg = newMsg;
		client = newclient;
	}
}