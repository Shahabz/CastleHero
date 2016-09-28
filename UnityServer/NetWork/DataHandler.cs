using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

public class DataHandler
{
    public Queue<TcpPacket> receiveMsgs;
    public Queue<TcpPacket> sendMsgs;
    public Database database;

    public Dictionary<Socket, string> LoginUser;

    object receiveLock;
    object sendLock;

    TcpPacket tcpPacket;

    byte[] msg = new byte[1024];

    RecvNotifier recvNotifier;
    public delegate ServerPacketId RecvNotifier(byte[] data);
    private Dictionary<int, RecvNotifier> m_notifier = new Dictionary<int, RecvNotifier>();

    public DataHandler(Queue<TcpPacket> receiveQueue, Queue<TcpPacket> sendQueue, object newReceiveLock, object newSendLock)
    {
        database = new Database();
        receiveMsgs = receiveQueue;
        sendMsgs = sendQueue;
        receiveLock = newReceiveLock;
        sendLock = newSendLock;
        LoginUser = new Dictionary<Socket, string>();

        m_notifier.Add((int)ClientPacketId.Create, CreateAccount);
        m_notifier.Add((int)ClientPacketId.Delete, DeleteAccount);
        m_notifier.Add((int)ClientPacketId.Login, Login);
        m_notifier.Add((int)ClientPacketId.Logout, Logout);
        m_notifier.Add((int)ClientPacketId.GameClose, GameClose);
        m_notifier.Add((int)ClientPacketId.HeroDataRequest, HeroDataRequest);
        m_notifier.Add((int)ClientPacketId.ItemDataRequest, ItemDataRequest);
        m_notifier.Add((int)ClientPacketId.SkillDataRequest, SkillDataRequest);
        m_notifier.Add((int)ClientPacketId.UnitDataRequest, UnitDataRequest);
        m_notifier.Add((int)ClientPacketId.BuildingDataRequest, BuildingDataRequest);
        m_notifier.Add((int)ClientPacketId.UpgradeDataRequest, UpgradeDataRequest);
        m_notifier.Add((int)ClientPacketId.ResourceDataRequest, ResourceDataRequest);
        m_notifier.Add((int)ClientPacketId.StateDataRequest, StateDataRequest);
        m_notifier.Add((int)ClientPacketId.Build, BuildBuilding);
        m_notifier.Add((int)ClientPacketId.BuildCancel, BuildCancel);
        m_notifier.Add((int)ClientPacketId.BuildDataRequest, BuildDataRequest);
        m_notifier.Add((int)ClientPacketId.BuildComplete, BuildComplete);
        m_notifier.Add((int)ClientPacketId.UnitCreate, UnitCreate);
        m_notifier.Add((int)ClientPacketId.UnitCreateDataRequest, UnitCreateDataRequest);
        m_notifier.Add((int)ClientPacketId.UnitCreateComplete, UnitCreateComplete);
        m_notifier.Add((int)ClientPacketId.MyPositionDataRequest, MyPositionDataRequest);
        m_notifier.Add((int)ClientPacketId.PlaceDataRequest, PlaceDataRequest);
        m_notifier.Add((int)ClientPacketId.EnemyUnitNumRequest, EnemyUnitNumRequest);
        m_notifier.Add((int)ClientPacketId.EnemyUnitDataRequest, EnemyUnitDataRequest);

        Thread handleThread = new Thread(new ThreadStart(DataHandle));
        handleThread.Start();
        Thread buildCheckThread = new Thread(new ThreadStart(BuildChecker));
        buildCheckThread.Start();
    }

    public void DataHandle()
    {
        while (true)
        {
            if (receiveMsgs.Count != 0)
            {
                //패킷을 Dequeue 한다 패킷 : 메시지 타입 + 메시지 내용
                lock (receiveLock)
                {
                    tcpPacket = receiveMsgs.Dequeue();
                }

                //타입과 내용을 분리한다
                byte Id = tcpPacket.msg[0];
                msg = new byte[tcpPacket.msg.Length - 1];
                Array.Copy(tcpPacket.msg, 1, msg, 0, msg.Length);

                HeaderData headerData = new HeaderData();

                //Dictionary에 등록된 델리게이트형 메소드에서 msg를 반환받는다.
                if (m_notifier.TryGetValue(Id, out recvNotifier))
                {
                    ServerPacketId packetId = recvNotifier(msg);
                    //send 할 id를 반환받음
                    if (packetId != ServerPacketId.None)
                    {
                        tcpPacket = new TcpPacket(msg, tcpPacket.client);
                        sendMsgs.Enqueue(tcpPacket);
                    }
                }
                else
                {
                    Console.WriteLine("DataHandler::TryGetValue 에러 " + Id);
                    headerData.Id = (byte)ServerPacketId.None;
                }
            }
        }
    }

    public ServerPacketId CreateAccount(byte[] data)
    {
        Console.WriteLine(tcpPacket.client.RemoteEndPoint.ToString() + " 가입요청");

        AccountPacket accountPacket = new AccountPacket(data);
        AccountData accountData = accountPacket.GetData();

        Console.WriteLine("아이디 : " + accountData.Id + "패스워드 : " + accountData.password);

        try
        {
            if (database.AddAccountData(accountData.Id, accountData.password))
            {
                msg[0] = (byte)UnityServer.Result.Success;
                Console.WriteLine("가입 성공");
            }
            else
            {
                msg[0] = (byte)UnityServer.Result.Fail;
                Console.WriteLine("가입 실패");
            }
        }
        catch
        {
            Console.WriteLine("DataHandler::AddPlayerData 에러");
            Console.WriteLine("가입 실패");
            msg[0] = (byte)UnityServer.Result.Fail;
        }

        Array.Resize(ref msg, 1);
        msg = CreateResultPacket(msg, ServerPacketId.CreateResult);

        return ServerPacketId.CreateResult;
    }

    public ServerPacketId DeleteAccount(byte[] data)
    {
        Console.WriteLine(tcpPacket.client.RemoteEndPoint.ToString() + " 탈퇴요청");

        AccountPacket accountPacket = new AccountPacket(data);
        AccountData accountData = accountPacket.GetData();

        Console.WriteLine("아이디 : " + accountData.Id + "패스워드 : " + accountData.Id);

        try
        {
            if (database.DeleteAccountData(accountData.Id, accountData.password))
            {
                msg[0] = (byte)UnityServer.Result.Success;
                Console.WriteLine("탈퇴 성공");
            }
            else
            {
                msg[0] = (byte)UnityServer.Result.Fail;
                Console.WriteLine("탈퇴 실패");
            }
        }
        catch
        {
            Console.WriteLine("DataHandler::RemovePlayerData 에러");
            Console.WriteLine("탈퇴 실패");
            msg[0] = (byte)UnityServer.Result.Fail;
        }

        Array.Resize(ref msg, 1);
        msg = CreateResultPacket(msg, ServerPacketId.DeleteResult);

        return ServerPacketId.DeleteResult;
    }

    public ServerPacketId Login(byte[] data)
    {
        Console.WriteLine(tcpPacket.client.RemoteEndPoint.ToString() + " 로그인요청");

        AccountPacket accountPacket = new AccountPacket(data);
        AccountData accountData = accountPacket.GetData();

        Console.WriteLine("아이디 : " + accountData.Id + "비밀번호 : " + accountData.password);

        try
        {
            if (database.AccountData.Contains(accountData.Id))
            {
                if (((LoginData)database.AccountData[accountData.Id]).PW == accountData.password)
                {
                    if (!LoginUser.ContainsValue(accountData.Id))
                    {
                        msg[0] = (byte)UnityServer.Result.Success;
                        Console.WriteLine("로그인 성공");
                        LoginUser.Add(tcpPacket.client, accountData.Id);
                    }
                    else
                    {
                        Console.WriteLine("현재 접속중인 아이디입니다.");

                        if (CompareIP(GetSocket(accountData.Id).RemoteEndPoint.ToString(), tcpPacket.client.RemoteEndPoint.ToString()))
                        {
                            LoginUser.Remove(GetSocket(accountData.Id));
                            Console.WriteLine("현재 접속중 해제");
                        }                        
                        msg[0] = (byte)UnityServer.Result.Fail;
                    }
                }
                else
                {
                    Console.WriteLine("패스워드가 맞지 않습니다.");
                    msg[0] = (byte)UnityServer.Result.Fail;
                }
            }
            else
            {
                Console.WriteLine("존재하지 않는 아이디입니다.");
                msg[0] = (byte)UnityServer.Result.Fail;
            }
        }
        catch
        {
            Console.WriteLine("DataHandler::PlayerData.Contains 에러");
            msg[0] = (byte)UnityServer.Result.Fail;
        }

        Array.Resize(ref msg, 1);

        msg = CreateResultPacket(msg, ServerPacketId.LoginResult);

        return ServerPacketId.LoginResult;
    }

    public ServerPacketId Logout(byte[] data)
    {
        Console.WriteLine(tcpPacket.client.RemoteEndPoint.ToString() + " 로그아웃요청");

        string id = LoginUser[tcpPacket.client];

        msg = new byte[1];

        try
        {
            if (LoginUser.ContainsValue(id))
            {
                LoginUser.Remove(tcpPacket.client);
                Console.WriteLine(id + "로그아웃");
                msg[0] = (byte)UnityServer.Result.Success;
            }
            else
            {
                Console.WriteLine("로그인되어있지 않은 아이디입니다. : " + id);
                msg[0] = (byte)UnityServer.Result.Fail;
            }
        }
        catch
        {
            Console.WriteLine("DataHandler::PlayerData.Contains 에러");
            msg[0] = (byte)UnityServer.Result.Fail;
        }

        Array.Resize(ref msg, 1);

        msg = CreateResultPacket(msg, ServerPacketId.LoginResult);

        return ServerPacketId.None;
    }

    public ServerPacketId GameClose(byte[] data)
    {
        Console.WriteLine("게임종료");

        try
        {
            Console.WriteLine(tcpPacket.client.RemoteEndPoint.ToString() + "가 접속을 종료했습니다.");

            if (LoginUser.ContainsKey(tcpPacket.client))
            {
                string Id = LoginUser[tcpPacket.client];
                database.FileSave(Id + ".data", database.GetAccountData(Id));
                database.UserData.Remove(Id);

                LoginUser.Remove(tcpPacket.client);
            }
                        
            tcpPacket.client.Close();
        }
        catch
        {
            Console.WriteLine("DataHandler::LoginUser.Close 에러");
        }

        return ServerPacketId.None;
    }

    public ServerPacketId HeroDataRequest(byte[] data)
    {        
        string Id = LoginUser[tcpPacket.client];
        Console.WriteLine("유저" + Id + "영웅 데이터 요청");
        int heroId = database.GetAccountData(Id).HeroId;
        int level = database.GetAccountData(Id).HeroLevel;

        HeroData heroData = new HeroData(heroId, level);
        HeroDataPacket heroDataPacket = new HeroDataPacket(heroData);

        msg = CreatePacket(heroDataPacket, ServerPacketId.HeroData);

        return ServerPacketId.HeroData;
    }

    public ServerPacketId ItemDataRequest(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];
        Console.WriteLine("유저" + Id + "아이템 데이터 요청");
        int[] equipment = database.GetAccountData(Id).Equipment;
        int[] inventory = database.GetAccountData(Id).InventoryId;
        int[] inventoryNum = database.GetAccountData(Id).InventoryNum;

        ItemData itemData = new ItemData(equipment, inventory, inventoryNum);
        ItemDataPacket itemDataPacket = new ItemDataPacket(itemData);

        msg = CreatePacket(itemDataPacket, ServerPacketId.ItemData);

        return ServerPacketId.ItemData;
    }

    public ServerPacketId SkillDataRequest(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];
        Console.WriteLine("유저" + Id + "스킬 데이터 요청");
        int[] skill = database.GetAccountData(Id).Skill;

        SkillData skillData = new SkillData(skill);
        SkillDataPacket skillDataPacket = new SkillDataPacket(skillData);

        msg = CreatePacket(skillDataPacket, ServerPacketId.SkillData);

        return ServerPacketId.SkillData;
    }

    public ServerPacketId UnitDataRequest(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];
        Console.WriteLine("유저" + Id + "유닛 데이터 요청");

        int unitKind = database.GetAccountData(Id).UnitKind;
        Unit[] unit = database.GetAccountData(Id).Unit;

        Console.WriteLine("유닛 종류 : " + unitKind);
        for (int i = 0; i < unitKind; i++)
        {
            Console.WriteLine("유닛 아이디" + unit[i].Id);
            Console.WriteLine("유닛 숫자" + unit[i].num);
        }

        UnitData unitData = new UnitData(unitKind, unit);
        UnitDataPacket unitDataPacket = new UnitDataPacket(unitData);
        msg = CreatePacket(unitDataPacket, ServerPacketId.UnitData);

        return ServerPacketId.UnitData;
    }

    public ServerPacketId BuildingDataRequest(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];
        Console.WriteLine("유저" + Id + "빌딩 데이터 요청");

        int[] building = database.GetAccountData(Id).Building;

        BuildingData buildingData = new BuildingData(building);
        BuildingDataPacket buildingDataPacket = new BuildingDataPacket(buildingData);

        msg = CreatePacket(buildingDataPacket, ServerPacketId.BuildingData);

        return ServerPacketId.SkillData;
    }

    public ServerPacketId UpgradeDataRequest(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];
        Console.WriteLine("유저" + Id + "업그레이드 데이터 요청");

        int[] upgrade = database.GetAccountData(Id).Upgrade;

        UpgradeData upgradeData = new UpgradeData(upgrade);
        UpgradeDataPacket upgradeDataPacket = new UpgradeDataPacket(upgradeData);

        msg = CreatePacket(upgradeDataPacket, ServerPacketId.UpgradeData);

        return ServerPacketId.UpgradeData;
    }

    public ServerPacketId ResourceDataRequest(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];
        Console.WriteLine("유저" + Id + "자원 데이터 요청");

        int resource = database.GetAccountData(Id).Resource;

        ResourceData resourceData = new ResourceData(resource);
        ResourceDataPacket resourceDataPacket = new ResourceDataPacket(resourceData);

        msg = CreatePacket(resourceDataPacket, ServerPacketId.ResourceData);

        return ServerPacketId.ResourceData;
    }

    public ServerPacketId StateDataRequest(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];
        Console.WriteLine("유저" + Id + "성 상태 데이터 요청");

        byte heroState = (byte)database.GetAccountData(Id).HState;
        byte CastleState = (byte)database.GetAccountData(Id).CState;

        StateData stateData = new StateData();
        StateDataPacket stateDataPacket = new StateDataPacket(stateData);

        msg = CreatePacket(stateDataPacket, ServerPacketId.StateData);

        return ServerPacketId.StateData;
    }

    public ServerPacketId BuildBuilding(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];

        BuildPacket buildPacket = new BuildPacket(data);
        Build buildData = buildPacket.GetData();

        database.GetAccountData(Id).Build(buildData.Id);
        database.FileSave(Id + ".data", database.GetAccountData(Id));

        Console.WriteLine("아이디 : " + buildData.Id);
        Console.WriteLine("짓는건물 : " + database.GetAccountData(Id).BuildBuilding);

        return ServerPacketId.None;
    }

    public ServerPacketId BuildCancel(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];

        database.GetAccountData(Id).BuildCancel();

        return ServerPacketId.None;
    }

    public ServerPacketId BuildDataRequest(byte[] data)
    {
        Console.WriteLine("건설 데이터 요청");
        string Id = LoginUser[tcpPacket.client];

        UserData newUserData = database.GetAccountData(Id);
        int buildBuilding = newUserData.BuildBuilding;
        DateTime time;

        if (buildBuilding != UserData.buildingNum)
        {
            time = newUserData.BuildTime;
        }
        else
        {
            time = DateTime.Now;
        }

        BuildData buildData = new BuildData(buildBuilding, time);
        BuildDataPacket buildDataPacket = new BuildDataPacket(buildData);

        Console.WriteLine("요청아이디 : " + Id);
        Console.WriteLine("빌딩 : " + buildData.Id);
        Console.WriteLine("시간 : " + time.ToString());

        msg = CreatePacket(buildDataPacket, ServerPacketId.BuildData);

        return ServerPacketId.BuildData;
    }

    public ServerPacketId BuildComplete(byte[] data)
    {        
        string Id = LoginUser[tcpPacket.client];

        Console.WriteLine("아이디 : " + Id);
        Console.WriteLine("건설 완료 : " + database.GetAccountData(Id).BuildBuilding);
        database.GetAccountData(Id).Buildcomplete();
        database.FileSave(Id + ".data", database.GetAccountData(Id));

        return ServerPacketId.None;
    }

    public ServerPacketId UnitCreate(byte[] data)
    {
        Console.Write("유닛 생산 ");
        string Id = LoginUser[tcpPacket.client];

        UnitCreatePacket unitCreatePacket = new UnitCreatePacket(data);
        UnitCreate unitCreate = unitCreatePacket.GetData();

        UserData newUserData = database.GetAccountData(Id);
        newUserData.UnitCreate(unitCreate);
        database.FileSave(Id + ".data", database.GetAccountData(Id));

        return ServerPacketId.None;
    }

    public ServerPacketId UnitCreateDataRequest(byte[] data)
    {
        Console.WriteLine("유닛생산 데이터 요청");
        string Id = LoginUser[tcpPacket.client];

        UserData newUserData = database.GetAccountData(Id);
        DateTime time;

        if (newUserData.CreateUnit.num != 0)
        {
            time = newUserData.UnitCreateTime;
        }
        else
        {
            time = DateTime.Now;
        }

        Unit unit = newUserData.CreateUnit;

        UnitCreateData unitCreateData = new UnitCreateData(time, unit);
        UnitCreateDataPacket unitCreateDataPacket = new UnitCreateDataPacket(unitCreateData);

        msg = CreatePacket(unitCreateDataPacket, ServerPacketId.UnitCreateData);

        return ServerPacketId.UnitCreateData;
    }

    public ServerPacketId UnitCreateComplete(byte[] data)
    {
        string Id = LoginUser[tcpPacket.client];

        UserData newUserData = database.GetAccountData(Id);

        database.GetAccountData(Id).UnitCreateComplete();

        database.FileSave(Id + ".data", database.GetAccountData(Id));

        return ServerPacketId.None;
    }

    public ServerPacketId MyPositionDataRequest(byte[] data)
    {
        Console.WriteLine("자기성 위치 데이터 요청");

        string Id = LoginUser[tcpPacket.client];

        UserData newUserData = database.GetAccountData(Id);

        Position positionData = new Position(newUserData.XPos, newUserData.YPos);
        PositionDataPacket positionDataPacket = new PositionDataPacket(positionData);

        msg = CreatePacket(positionDataPacket, ServerPacketId.MyPositionData);

        return ServerPacketId.MyPositionData;
    }

    public ServerPacketId PlaceDataRequest(byte[] data)
    {
        Console.WriteLine("성 위치 데이터 요청");
        string Id = LoginUser[tcpPacket.client];

        UserData newUserData = database.GetAccountData(Id);

        Place[] placeData = database.GetWorldMapData();
        PlaceDataPacket positionDataPacket = new PlaceDataPacket(placeData);

        msg = CreatePacket(positionDataPacket, ServerPacketId.PlaceData);

        return ServerPacketId.PlaceData;
    }

    public ServerPacketId EnemyUnitNumRequest(byte[] data)
    {
        Console.WriteLine("적 유닛 숫자 데이터 요청");

        string Id = Encoding.Unicode.GetString(data);

        UserData newUserData = database.GetAccountData(Id);
        
        msg = CreateResultPacket(BitConverter.GetBytes(newUserData.GetUnitNum()), ServerPacketId.EnemyUnitNumData);

        return ServerPacketId.EnemyUnitNumData;
    }

    public ServerPacketId EnemyUnitDataRequest(byte[] data)
    {
        Console.WriteLine("적 유닛 데이터 요청");

        PositionDataPacket enemyUnitDataRequestPacket = new PositionDataPacket(data);
        Position position = enemyUnitDataRequestPacket.GetData();

        UnitData unitData = new UnitData();

        int mapIndex = position.X * 1000 + position.Y;

        Place place = database.GetPlaceData(mapIndex);

        if (place.Type == (int)PlaceType.Castle)
        {
            UserData newUserData = database.GetAccountData(place.ID);
            unitData = new UnitData(newUserData.UnitKind, newUserData.Unit);
        }

        UnitDataPacket unitDataPacket = new UnitDataPacket(unitData);

        msg = CreatePacket(unitDataPacket, ServerPacketId.EnemyUnitData);

        Console.WriteLine(msg[2]);
        return ServerPacketId.EnemyUnitData;
    }

    public void BuildChecker()
    {
        while (true)
        {
            Thread.Sleep(1800000);

            foreach (KeyValuePair<Socket, string> client in LoginUser)
            {
                Console.WriteLine(database.GetAccountData(client.Value).BuildBuilding);
                if(database.GetAccountData(client.Value).BuildBuilding != (int) BuildingId.None)
                {
                    Console.WriteLine(database.GetAccountData(client.Value).BuildTime);
                    if (database.GetAccountData(client.Value).BuildTime < DateTime.Now)
                    {
                        Console.WriteLine(client.Value + "자동 건설 완료 : " + database.GetAccountData(client.Value).BuildBuilding);
                        database.GetAccountData(client.Value).Buildcomplete();
                        database.FileSave(client.Value + ".data", database.GetAccountData(client.Value));
                    }
                }
            }
        }
    }

    byte[] CreateHeader<T>(IPacket<T> data, ServerPacketId Id)
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

    byte[] CreatePacket<T>(IPacket<T> data, ServerPacketId Id)
    {
        byte[] msg = data.GetPacketData();
        byte[] header = CreateHeader(data, Id);
        byte[] packet = CombineByte(header, msg);

        return packet;
    }

    byte[] CreateResultPacket(byte[] msg, ServerPacketId Id)
    {
        HeaderData headerData = new HeaderData();
        HeaderSerializer HeaderSerializer = new HeaderSerializer();

        headerData.Id = (byte)Id;
        headerData.length = (short)msg.Length;
        HeaderSerializer.Serialize(headerData);
        msg = CombineByte(HeaderSerializer.GetSerializedData(), msg);
        return msg;
    }

    bool CompareIP(string ip1, string ip2)
    {
        if(ip1.Substring(0, ip1.IndexOf(":")) == ip2.Substring(0, ip2.IndexOf(":")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Socket GetSocket(string Id)
    {
        foreach (KeyValuePair<Socket, string> client in LoginUser)
        {
            if (client.Value == Id)
            {
                return client.Key;
            }
        }

        return null;
    }

    public static byte[] CombineByte (byte[] array1, byte[] array2)
	{
		byte[] array3 = new byte[array1.Length + array2.Length];
		Array.Copy (array1, 0, array3, 0, array1.Length);
		Array.Copy (array2, 0, array3, array1.Length, array2.Length);
		return array3;
	}

	public static byte[] CombineByte (byte[] array1, byte[] array2, byte[] array3)
	{
		byte[] array4 = CombineByte (CombineByte (array1, array2), array3);;
		return array4;
	}
}

[Serializable]
public class TcpClient
{
	public Socket client;
	public string Id;

	public TcpClient (Socket newClient)
	{
		client = newClient;
		Id = "";
	}
}

public class HeaderData
{
    // 헤더 == [2바이트 - 패킷길이][1바이트 - ID]
    public short length; // 패킷의 길이
    public byte Id; // 패킷 ID
}