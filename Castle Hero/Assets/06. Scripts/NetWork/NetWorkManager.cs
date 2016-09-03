using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

public class NetWorkManager : MonoBehaviour
{
    public const short packetLength = 2;
    public const byte packetId = 1;

    [SerializeField]
    string ip = "192.168.94.88";
    [SerializeField]
    int port = 3000;

    UIManager uiManager;
    GameManager gameManager;
    DataManager dataManager;
    LoadingManager loadingManager;

    Socket clientSock;
    Queue<byte[]> msgs;
    System.Object receiveLock;

    RecvNotifier recvNotifier;
    public delegate void RecvNotifier(byte[] data);
    private Dictionary<int, RecvNotifier> m_notifier = new Dictionary<int, RecvNotifier>();

    AsyncCallback asyncReceiveLengthCallBack;
    AsyncCallback asyncReceiveDataCallBack;

    void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        loadingManager = GameObject.FindGameObjectWithTag("LoadingManager").GetComponent<LoadingManager>();

        m_notifier.Add((int)ServerPacketId.CreateResult, OnReceivedCreateAccountResult);
        m_notifier.Add((int)ServerPacketId.DeleteResult, OnReceivedDeleteAccountResult);
        m_notifier.Add((int)ServerPacketId.LoginResult, OnReceivedLoginAccountResult);
        m_notifier.Add((int)ServerPacketId.HeroData, OnReceivedHeroData);
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start ()
    {
        clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSock.Connect(IPAddress.Parse(ip), port);
        msgs = new Queue<byte[]>();
        receiveLock = new object();

        asyncReceiveLengthCallBack = new AsyncCallback(HandleAsyncLengthReceive);
        asyncReceiveDataCallBack = new AsyncCallback(HandleAsyncDataReceive);

        AsyncData asyncData = new AsyncData(clientSock);
        clientSock.BeginReceive(asyncData.msg, 0, NetWorkManager.packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (System.Object)asyncData);

        StartCoroutine(DataHandle());
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

        if (asyncData.msgSize >= packetLength)
        {
            short msgSize = 0;

            try
            {
                msgSize = BitConverter.ToInt16(asyncData.msg, 0);
                asyncData = new AsyncData(clientSock);
                clientSock.BeginReceive(asyncData.msg, 0, msgSize + packetId, SocketFlags.None, asyncReceiveDataCallBack, (System.Object)asyncData);
            }
            catch
            {
                Console.WriteLine("NetWorkManager::HandleAsyncLengthReceive.BitConverter 에러");
                asyncData = new AsyncData(clientSock);
                clientSock.BeginReceive(asyncData.msg, 0, packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (System.Object)asyncData);
            }
        }
        else
        {
            asyncData = new AsyncData(clientSock);
            clientSock.BeginReceive(asyncData.msg, 0, packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (System.Object)asyncData);
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

        if (asyncData.msgSize >= packetId)
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
        clientSock.BeginReceive(asyncData.msg, 0, packetLength, SocketFlags.None, asyncReceiveLengthCallBack, (System.Object)asyncData);
    }

    IEnumerator DataHandle()
    {
        while (true)
        {
            if (msgs.Count > 0)
            {
                byte[] packet = msgs.Dequeue();

                byte msgType = packet[0];
                byte[] msg = new byte[packet.Length - 1];
                Array.Copy(packet, 1, msg, 0, msg.Length);

                HeaderData headerData = new HeaderData();

                if (m_notifier.TryGetValue(msgType, out recvNotifier))
                {
                    Debug.Log("메시지 타입" + msgType);
                    recvNotifier(msg);
                }
                else
                {
                    Console.WriteLine("DataHandler::TryGetValue 에러 " + msgType);
                    headerData.Id = (byte)ServerPacketId.None;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void CreateAccount(string Id, string Pw)
    {
        AccountData accountData = new AccountData(Id, Pw);
        AccountPacket accountPacket = new AccountPacket(accountData);
        byte[] msg = CreatePacket(accountPacket, ClientPacketId.Create);

        clientSock.Send(msg);
    }

    public void DeleteAccount(string Id, string Pw)
    {
        AccountData accountData = new AccountData(Id, Pw);
        AccountPacket accountPacket = new AccountPacket(accountData);
        byte[] msg = CreatePacket(accountPacket, ClientPacketId.Delete);

        clientSock.Send(msg);
    }

    public void Login(string Id, string Pw)
    {
        AccountData accountData = new AccountData(Id, Pw);
        AccountPacket accountPacket = new AccountPacket(accountData);
        byte[] msg = CreatePacket(accountPacket, ClientPacketId.Login);

        clientSock.Send(msg);
    }

    public void Logout()
    {

    }

    public void GameExit()
    {
        //No Packet
        HeaderData headerData = new HeaderData();
        HeaderSerializer HeaderSerializer = new HeaderSerializer();

        headerData.Id = (int)ClientPacketId.GameClose;
        HeaderSerializer.Serialize(headerData);
        byte[] msg = HeaderSerializer.GetSerializedData();

        clientSock.Send(msg);
    }

    public void DataRequest(ClientPacketId packetId)
    {
        //No Packet
        HeaderData headerData = new HeaderData();
        HeaderSerializer HeaderSerializer = new HeaderSerializer();

        headerData.Id = (byte) packetId;
        HeaderSerializer.Serialize(headerData);
        byte[] msg = HeaderSerializer.GetSerializedData();

        clientSock.Send(msg);
    }

    void OnReceivedCreateAccountResult(byte[] msg)
    {
        if(msg[0] == 1)
        {
            uiManager.createAccountPanel.SetActive(false);
            StartCoroutine(uiManager.DialogCtrl(1.0f));
            uiManager.dialog.transform.FindChild("Text").GetComponent<Text>().text = "가입성공";
        }
        else
        {
            StartCoroutine(uiManager.DialogCtrl(1.0f));
            uiManager.dialog.transform.FindChild("Text").GetComponent<Text>().text = "가입실패";
        }
    }

    void OnReceivedDeleteAccountResult(byte[] msg)
    {
        if (msg[0] == 1)
        {
            uiManager.deleteAccountPanel.SetActive(false);
            StartCoroutine(uiManager.DialogCtrl(1.0f));
            uiManager.dialog.transform.FindChild("Text").GetComponent<Text>().text = "탈퇴성공";
        }
        else
        {
            StartCoroutine(uiManager.DialogCtrl(1.0f));
            uiManager.dialog.transform.FindChild("Text").GetComponent<Text>().text = "탈퇴실패";
        }
    }

    void OnReceivedLoginAccountResult(byte[] msg)
    {
        if (msg[0] == 1)
        {
            dataManager.SetId(uiManager.loginId.text);
            StartCoroutine(loadingManager.LoadScene((int) GameManager.Scene.Loading, 1.0f, LoadingManager.Scene.Wait));
            StartCoroutine(uiManager.DialogCtrl(1.0f));
            uiManager.dialog.transform.FindChild("Text").GetComponent<Text>().text = "로그인성공";
        }
        else
        {
            StartCoroutine(uiManager.DialogCtrl(1.0f));
            uiManager.dialog.transform.FindChild("Text").GetComponent<Text>().text = "로그인실패";
        }
    }

    void OnReceivedHeroData(byte[] msg)
    {
        HeroDataPacket heroDataPacket = new HeroDataPacket(msg);
        HeroData heroData = heroDataPacket.GetData();

        dataManager.SetHeroData(heroData);
    }

    byte[] CreateHeader<T>(IPacket<T> data, ClientPacketId Id)
    {
        byte[] msg = data.GetPacketData();

        HeaderData headerData = new HeaderData();
        HeaderSerializer headerSerializer = new HeaderSerializer();

        headerData.Id = (byte)Id;
        headerData.length = (short)msg.Length;

        headerSerializer.Serialize(headerData);
        byte[] header = headerSerializer.GetSerializedData();

        return header;
    }

    byte[] CreatePacket<T>(IPacket<T> data, ClientPacketId Id)
    {
        byte[] msg = data.GetPacketData();
        byte[] header = CreateHeader(data, Id);
        byte[] packet = CombineByte(header, msg);

        return packet;
    }

    public byte[] CombineByte(byte[] array1, byte[] array2)
    {
        byte[] array3 = new byte[array1.Length + array2.Length];
        Array.Copy(array1, 0, array3, 0, array1.Length);
        Array.Copy(array2, 0, array3, array1.Length, array2.Length);
        return array3;
    }
}

public class AsyncData
{
    public Socket clientSock;
    public byte[] msg;
    public short msgSize;
    public const int msgMaxLength = 1024;

    public AsyncData(Socket newClient)
    {
        msg = new byte[msgMaxLength];
        clientSock = newClient;
    }
}