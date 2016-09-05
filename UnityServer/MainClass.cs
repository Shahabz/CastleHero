using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class UnityServer
{
    public enum Result
    {
        Success = 1,
        Fail = 2,        
    };

	public const short packetId = 1;
	public const short packetLength = 2;    

	public static void Main (string[] args)
	{
		Queue<TcpPacket> receiveData = new Queue<TcpPacket> ();
		Queue<TcpPacket> sendData = new Queue<TcpPacket> ();

		Object receiveLock = new Object ();
		Object sendLock = new Object ();

		Hashtable LoginUser = new Hashtable ();

        DataReceiver dataReceiver = new DataReceiver(receiveData, IPAddress.Parse ("192.168.94.88"), 3000, receiveLock, LoginUser);
		DataHandler dataHandler = new DataHandler (receiveData, sendData, receiveLock, sendLock, LoginUser);
		DataSender dataSender = new DataSender (sendData, sendLock);

        //while (true)
        //{
        //    Thread.Sleep(1000);

        //    foreach(DictionaryEntry client in dataReceiver.LoginUser)
        //    {
        //        Socket newSocket = (Socket) client.Key;
        //        Console.WriteLine(newSocket.Connected);
        //        Console.WriteLine(newSocket.RemoteEndPoint.ToString());
        //        Console.WriteLine("큐 : " + receiveData.Count);
        //    }
        //}
	}
}
