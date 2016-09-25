using System;
using System.Net;
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

    public static void Main(string[] args)
    {
        Queue<TcpPacket> receiveData = new Queue<TcpPacket>();
        Queue<TcpPacket> sendData = new Queue<TcpPacket>();

        object receiveLock = new Object();
        object sendLock = new Object();

        DataReceiver dataReceiver = new DataReceiver(receiveData, IPAddress.Loopback, 3000, receiveLock);
        DataHandler dataHandler = new DataHandler(receiveData, sendData, receiveLock, sendLock);
        DataSender dataSender = new DataSender(sendData, sendLock);

        //while (true)
        //{
        //    Thread.Sleep(1000);

        //    foreach (DictionaryEntry client in dataHandler.database.UserData)
        //    {
        //        UserData newUserData = (UserData)client.Value;
        //        Console.WriteLine(newUserData.BuildBuilding);
        //    }
        //}
    }
}
