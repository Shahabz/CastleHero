using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

public class DataSender
{
    Queue<TcpPacket> msgs;
    TcpPacket tcpPacket;
    Socket client;
    byte[] msg;

    Object sendLock;

    public DataSender(Queue<TcpPacket> newQueue, Object newSendLock)
    {
        msgs = newQueue;
        sendLock = newSendLock;

        Thread sendThread = new Thread(new ThreadStart(DataSend));
        sendThread.Start();
    }

    public void DataSend()
    {
        while (true)
        {            
            if (msgs.Count != 0)
            {
                lock (sendLock)
                {
                    tcpPacket = msgs.Dequeue();
                }

                Console.WriteLine("보낸메시지 길이 :" + tcpPacket.msg.Length);

                try
                {
                    client = tcpPacket.client;
                    msg = tcpPacket.msg;

                    client.Send(msg);
                }
                catch
                {
                    Console.WriteLine("DataSender::DataSend 에러");
                }
            }
        }
    }
}