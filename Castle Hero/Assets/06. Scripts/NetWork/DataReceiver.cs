using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public class DataReceiver
{
    AsyncCallback asyncReceiveLengthCallBack;
    AsyncCallback asyncReceiveDataCallBack;

    object receiveLock;
    Queue<byte[]> msgs;
    Socket clientSock;

    public DataReceiver(Socket newSocket, Queue<byte[]> newMsgs, object newReceiveLock)
    {
        clientSock = newSocket;
        receiveLock = newReceiveLock;
        msgs = newMsgs;

        asyncReceiveLengthCallBack = new AsyncCallback(HandleAsyncLengthReceive);
        asyncReceiveDataCallBack = new AsyncCallback(HandleAsyncDataReceive);

        AsyncData asyncData = new AsyncData(clientSock);
        clientSock.BeginReceive(asyncData.msg, 0, NetWorkManager.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (object)asyncData);
    }

    void HandleAsyncLengthReceive(IAsyncResult asyncResult)
    {
        AsyncData asyncData = (AsyncData)asyncResult.AsyncState;
        Socket clientSock = asyncData.clientSock;

        try
        {
            asyncData.msgSize = (short)clientSock.EndReceive(asyncResult);
        }
        catch
        {
            Debug.Log("NetWorkManager::HandleAsyncLengthReceive.EndReceive 에러");
            return;
        }

        if (asyncData.msgSize >= NetWorkManager.packetLength)
        {           
            short msgSize = 0;

            try
            {
                msgSize = BitConverter.ToInt16(asyncData.msg, 0);
                asyncData = new AsyncData(clientSock);
                clientSock.BeginReceive(asyncData.msg, 0, msgSize + NetWorkManager.packetId, SocketFlags.None, asyncReceiveDataCallBack, (object)asyncData);
            }
            catch
            {
                Console.WriteLine("NetWorkManager::HandleAsyncLengthReceive.BitConverter 에러");
                asyncData = new AsyncData(clientSock);
                clientSock.BeginReceive(asyncData.msg, 0, NetWorkManager.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (object)asyncData);
            }
        }
        else
        {
            asyncData = new AsyncData(clientSock);
            clientSock.BeginReceive(asyncData.msg, 0, NetWorkManager.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (object)asyncData);
        }
    }

    void HandleAsyncDataReceive(IAsyncResult asyncResult)
    {
        AsyncData asyncData = (AsyncData)asyncResult.AsyncState;
        Socket clientSock = asyncData.clientSock;

        try
        {
            asyncData.msgSize = (short)clientSock.EndReceive(asyncResult);
        }
        catch
        {
            Debug.Log("NetWorkManager::HandleAsyncDataReceive.EndReceive 에러");
            return;
        }

        if (asyncData.msgSize >= NetWorkManager.packetId)
        {
            Array.Resize(ref asyncData.msg, asyncData.msgSize);

            lock (receiveLock)
            {
                try
                {
                    msgs.Enqueue(asyncData.msg);
                }
                catch
                {
                    Console.WriteLine("NetWorkManager::HandleAsyncDataReceive.Enqueue 에러");
                }
            }
        }

        asyncData = new AsyncData(clientSock);
        clientSock.BeginReceive(asyncData.msg, 0, NetWorkManager.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (object)asyncData);
    }
}