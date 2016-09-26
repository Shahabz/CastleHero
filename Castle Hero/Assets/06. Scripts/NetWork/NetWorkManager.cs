using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public const short packetLength = 2;
    public const byte packetId = 1;

    //[SerializeField]
    //string ip = "192.168.0.15";
    [SerializeField]
    int port = 3000;
    
    DataReceiver dataReceiver;

    Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    Queue<byte[]> receiveMsg = new Queue<byte[]>();
    Queue<byte[]> sendMsg = new Queue<byte[]>();
    object receiveLock = new object();
    object sendLock = new object();

    RecvNotifier recvNotifier;
    public delegate void RecvNotifier(byte[] data);
    private Dictionary<int, RecvNotifier> m_notifier = new Dictionary<int, RecvNotifier>();

    [SerializeField] UIManager uiManager;
    [SerializeField] DataManager dataManager;
    [SerializeField] LoadingManager loadingManager;

    void Awake()
    {
        tag = "NetworkManager";
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start ()
    {
        StartCoroutine(ServerConect());        
    }

    IEnumerator ServerConect()
    {
        int count = 0;
        while (count < 100)
        {   
            try
            {
                clientSock.Connect(IPAddress.Loopback, port);
                break;
            }
            catch
            {
                Debug.Log("서버와 연결이 끊겼습니다. 실패 : " + count + "회");
            }
            count++;

            yield return new WaitForSeconds(5f);
        }

        if (clientSock.Connected)
        {
            dataReceiver = new DataReceiver(clientSock, receiveMsg, receiveLock);
            dataReceiver.StartDataReceive();
            SetServerConnection();
        }
        else
        {
            Debug.Log("서버와 연결이 되지않아 클라이언트를 종료합니다.");
            Application.Quit();
        }
    }

    public void ManagerInitialize()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        loadingManager = GameObject.FindGameObjectWithTag("LoadingManager").GetComponent<LoadingManager>();
    }

    void SetServerConnection()
    {
        m_notifier.Clear();
        m_notifier.Add((int)ServerPacketId.CreateResult, OnReceivedCreateAccountResult);
        m_notifier.Add((int)ServerPacketId.DeleteResult, OnReceivedDeleteAccountResult);
        m_notifier.Add((int)ServerPacketId.LoginResult, OnReceivedLoginResult);
        m_notifier.Add((int)ServerPacketId.HeroData, OnReceivedHeroData);
        m_notifier.Add((int)ServerPacketId.SkillData, OnReceivedSkillData);
        m_notifier.Add((int)ServerPacketId.ItemData, OnReceivedItemData);
        m_notifier.Add((int)ServerPacketId.UnitData, OnReceivedUnitData);
        m_notifier.Add((int)ServerPacketId.BuildingData, OnReceivedBuildingData);
        m_notifier.Add((int)ServerPacketId.UpgradeData, OnReceivedUpgradeData);
        m_notifier.Add((int)ServerPacketId.ResourceData, OnReceivedResourceData);
        m_notifier.Add((int)ServerPacketId.StateData, OnReceivedStateData);
        m_notifier.Add((int)ServerPacketId.BuildData, OnReceivedBuildData);
        m_notifier.Add((int)ServerPacketId.UnitCreateData, OnReceivedUnitCreateData);
        m_notifier.Add((int)ServerPacketId.PositionData, OnReceivedPositionData);
    }

    public void GameExit()
    {
        //No Packet
        Debug.Log("게임종료");
        HeaderData headerData = new HeaderData();
        HeaderSerializer HeaderSerializer = new HeaderSerializer();

        headerData.Id = (int)ClientPacketId.GameClose;
        HeaderSerializer.Serialize(headerData);
        byte[] msg = HeaderSerializer.GetSerializedData();

        try
        {
            clientSock.Send(msg);
        }
        catch
        {
            Debug.Log("서버와의 연결이 끊겼습니다.");
        }
        
    }

    public void DataHandle()
    {
        if (receiveMsg.Count > 0)
        {
            byte[] packet;

            lock (receiveLock)
            {
                packet = receiveMsg.Dequeue();
            }

            byte msgType = packet[0];
            byte[] msg = new byte[packet.Length - 1];
            Array.Copy(packet, 1, msg, 0, msg.Length);

            HeaderData headerData = new HeaderData();

            if (m_notifier.TryGetValue(msgType, out recvNotifier))
            {
                recvNotifier(msg);
            }
            else
            {
                Debug.Log("DataHandler::TryGetValue 에러 " + msgType);
                headerData.Id = (byte)ServerPacketId.None;
            }
        }
    }

    public void DataSend()
    {
        if (sendMsg.Count > 0)
        {
            lock (sendLock)
            {
                byte[] msg = sendMsg.Dequeue();
                try
                {
                    clientSock.Send(msg);
                }
                catch
                {
                    Debug.Log("서버의 응답이 없습니다.");
                }                
            }
        }
    }

    void OnReceivedCreateAccountResult(byte[] msg)
    {
        if (msg[0] == 1)
        {
            uiManager.createAccountPanel.SetActive(false);
            StartCoroutine(UIManager.DialogCtrl(1.0f, "가입성공"));
        }
        else
        {
            StartCoroutine(UIManager.DialogCtrl(1.0f, "가입실패"));
        }
    }

    void OnReceivedDeleteAccountResult(byte[] msg)
    {
        if (msg[0] == 1)
        {
            uiManager.deleteAccountPanel.SetActive(false);
            StartCoroutine(UIManager.DialogCtrl(1.0f, "탈퇴성공"));
        }
        else
        {
            StartCoroutine(UIManager.DialogCtrl(1.0f, "탈퇴실패"));
        }
    }

    void OnReceivedLoginResult(byte[] msg)
    {
        if (msg[0] == 1)
        {
            dataManager.SetId(uiManager.loginId.text);
            StartCoroutine(loadingManager.LoadScene(GameManager.Scene.Login, GameManager.Scene.Wait, 1.0f));
            StartCoroutine(UIManager.DialogCtrl(1.0f, "로그인성공"));
        }
        else
        {
            StartCoroutine(UIManager.DialogCtrl(1.0f, "로그인실패"));
        }
    }

    void OnReceivedHeroData(byte[] msg)
    {
        HeroDataPacket heroDataPacket = new HeroDataPacket(msg);
        HeroData heroData = heroDataPacket.GetData();

        dataManager.SetHeroData(heroData);
        loadingManager.dataCheck[(int)ServerPacketId.HeroData - 4] = true;
    }

    void OnReceivedItemData(byte[] msg)
    {
        ItemDataPacket itemDataPacket = new ItemDataPacket(msg);
        ItemData itemData = itemDataPacket.GetData();

        dataManager.SetItemData(itemData);
        loadingManager.dataCheck[(int)ServerPacketId.ItemData - 4] = true;
    }

    void OnReceivedSkillData(byte[] msg)
    {
        SkillDataPacket skillDataPacket = new SkillDataPacket(msg);
        SkillData skillData = skillDataPacket.GetData();

        dataManager.SetSkillData(skillData);
        loadingManager.dataCheck[(int)ServerPacketId.SkillData - 4] = true;
    }

    void OnReceivedUnitData(byte[] msg)
    {
        UnitDataPacket unitDataPacket = new UnitDataPacket(msg);
        UnitData unitData = unitDataPacket.GetData();

        dataManager.SetUnitData(unitData);

        if (loadingManager.CurrentScene == GameManager.Scene.Loading)
        {
            loadingManager.dataCheck[(int)ServerPacketId.UnitData - 4] = true;
        }
        else if (loadingManager.CurrentScene == GameManager.Scene.Wait)
        {
            uiManager.SetUnitScrollView();
        }        
    }

    void OnReceivedBuildingData(byte[] msg)
    {
        BuildingDataPacket buildingDataPacket = new BuildingDataPacket(msg);
        BuildingData buildingData = buildingDataPacket.GetData();

        dataManager.SetBuildingData(buildingData);
        if(loadingManager.CurrentScene == GameManager.Scene.Loading)
        {
            loadingManager.dataCheck[(int)ServerPacketId.BuildingData - 4] = true;
        }
        else if(loadingManager.CurrentScene == GameManager.Scene.Wait)
        {
            uiManager.BuildingUIManager.SetBuilding();
        }
    }

    void OnReceivedUpgradeData(byte[] msg)
    {
        UpgradeDataPacket upgradeDataPacket = new UpgradeDataPacket(msg);
        UpgradeData upgradeData = upgradeDataPacket.GetData();

        dataManager.SetUpgradeData(upgradeData);
        loadingManager.dataCheck[(int)ServerPacketId.UpgradeData - 4] = true;
    }

    void OnReceivedResourceData(byte[] msg)
    {
        ResourceDataPacket resourceDataPacket = new ResourceDataPacket(msg);
        ResourceData resourceData = resourceDataPacket.GetData();

        dataManager.SetResourceData(resourceData);
        loadingManager.dataCheck[(int)ServerPacketId.ResourceData - 4] = true;
    }

    void OnReceivedStateData(byte[] msg)
    {
        StateDataPacket stateDataPacket = new StateDataPacket(msg);
        StateData stateData = stateDataPacket.GetData();

        dataManager.SetStateData(stateData);
        loadingManager.dataCheck[(int)ServerPacketId.StateData - 4] = true;
    }

    void OnReceivedBuildData(byte[] msg)
    {
        BuildDataPacket buildDataPacket = new BuildDataPacket(msg);
        BuildData buildData = buildDataPacket.GetData();

        dataManager.SetBuildData(buildData);

        if (loadingManager.CurrentScene == GameManager.Scene.Loading)
        {
            loadingManager.dataCheck[(int)ServerPacketId.BuildData - 4] = true;
        }
        else if (loadingManager.CurrentScene == GameManager.Scene.Wait)
        {
            StartCoroutine(uiManager.BuildTimeCheck());
        }
    }

    void OnReceivedUnitCreateData(byte[] msg)
    {
        UnitCreateDataPacket unitCreateDataPacket = new UnitCreateDataPacket(msg);
        UnitCreateData unitCreateData = unitCreateDataPacket.GetData();
        Debug.Log(" 생산유닛 : " + unitCreateData.unit.Id + " 생산개수 : " + unitCreateData.unit.num);
        Debug.Log("시간 : " + unitCreateData.hour.ToString() + ":" + unitCreateData.minute.ToString() + ":" + unitCreateData.second.ToString());

        dataManager.SetUnitCreateData(unitCreateData);

        if(loadingManager.CurrentScene == GameManager.Scene.Loading)
        {
            loadingManager.dataCheck[(int)ServerPacketId.UnitCreateData - 4] = true;
        }
        else if (loadingManager.CurrentScene == GameManager.Scene.Wait)
        {
            StartCoroutine(uiManager.UnitCreateTimeCheck());
        }
    }

    void OnReceivedPositionData(byte[] msg)
    {
        PositionDataPacket positionDataPacket = new PositionDataPacket(msg);
        PositionData positionData = positionDataPacket.GetData();

        dataManager.SetPositionData(positionData);

        loadingManager.dataCheck[(int)ServerPacketId.PositionData - 4] = true;
    }

    public void CreateAccount(string Id, string Pw)
    {
        Debug.Log("회원가입");
        AccountData accountData = new AccountData(Id, Pw);
        AccountPacket accountPacket = new AccountPacket(accountData);
        byte[] msg = CreatePacket(accountPacket, ClientPacketId.Create);
        sendMsg.Enqueue(msg);
    }

    public void DeleteAccount(string Id, string Pw)
    {
        AccountData accountData = new AccountData(Id, Pw);
        AccountPacket accountPacket = new AccountPacket(accountData);
        byte[] msg = CreatePacket(accountPacket, ClientPacketId.Delete);

        sendMsg.Enqueue(msg);
    }

    public void Login(string Id, string Pw)
    {
        Debug.Log("로그인");
        AccountData accountData = new AccountData(Id, Pw);
        AccountPacket accountPacket = new AccountPacket(accountData);
        byte[] msg = CreatePacket(accountPacket, ClientPacketId.Login);

        sendMsg.Enqueue(msg);
    }

    public void Logout()
    {
        //No Packet
        Debug.Log("로그아웃");
        HeaderData headerData = new HeaderData();
        HeaderSerializer HeaderSerializer = new HeaderSerializer();

        headerData.Id = (byte)ClientPacketId.Logout;
        HeaderSerializer.Serialize(headerData);
        byte[] msg = HeaderSerializer.GetSerializedData();

        sendMsg.Enqueue(msg);

        StartCoroutine(loadingManager.LoadScene(GameManager.Scene.Wait, GameManager.Scene.Login, 1.0f));
    }

    public void BuildBuilding(BuildingId Id)
    {
        Build build = new Build(Id);
        BuildPacket buildPacket = new BuildPacket(build);
        byte[] msg = CreatePacket(buildPacket, ClientPacketId.Build);

        sendMsg.Enqueue(msg);
    }

    public void BuildComplete()
    {
        DataRequest(ClientPacketId.BuildComplete);
        DataRequest(ClientPacketId.BuildingDataRequest);
    }

    public void UnitCreate(int unitId, int unitNum)
    {
        UnitCreate unitCreate = new UnitCreate(unitId, unitNum);
        UnitCreatePacket unitCreatePacket = new UnitCreatePacket(unitCreate);
        byte[] msg = CreatePacket(unitCreatePacket, ClientPacketId.UnitCreate);

        sendMsg.Enqueue(msg);
    }

    public void UnitCreateComplete()
    {
        DataRequest(ClientPacketId.UnitCreateComplete);
        DataRequest(ClientPacketId.UnitDataRequest);
        DataRequest(ClientPacketId.UnitCreateDataRequest);
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

    public void DataRequest(ClientPacketId Id)
    {
        //No Packet
        HeaderData headerData = new HeaderData();
        HeaderSerializer HeaderSerializer = new HeaderSerializer();

        headerData.Id = (byte)Id;
        HeaderSerializer.Serialize(headerData);
        byte[] msg = HeaderSerializer.GetSerializedData();

        sendMsg.Enqueue(msg);
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