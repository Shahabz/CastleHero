using System;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

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

		object receiveLock = new object ();
		object sendLock = new object ();

		Hashtable LoginUser = new Hashtable ();

        DataReceiver dataReceiver = new DataReceiver(receiveData, IPAddress.Parse ("192.168.94.88"), 3000, receiveLock, LoginUser);
		DataHandler dataHandler = new DataHandler (receiveData, sendData, receiveLock, sendLock, LoginUser);
		DataSender dataSender = new DataSender (sendData, sendLock);
	}
}
