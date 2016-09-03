using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

public class DataHandler
{
	public Queue<TcpPacket> receiveMsgs;
	public Queue<TcpPacket> sendMsgs;
	public Database database;

	public Hashtable LoginUser;

	object receiveLock;
	object sendLock;

	TcpPacket tcpPacket;
    
	byte[] msg = new byte[1024];

    RecvNotifier recvNotifier;
    public delegate ServerPacketId RecvNotifier(byte[] data);
	private Dictionary<int, RecvNotifier> m_notifier = new Dictionary<int, RecvNotifier>();

	public DataHandler (Queue<TcpPacket> receiveQueue, Queue<TcpPacket> sendQueue, object newReceiveLock, object newSendLock, Hashtable newHashtable)
	{
		database = new Database ();
		receiveMsgs = receiveQueue;
		sendMsgs = sendQueue;
		receiveLock = newReceiveLock;
		sendLock = newSendLock;
		LoginUser = newHashtable;

		m_notifier.Add((int) ClientPacketId.Create, CreateAccount);
		m_notifier.Add((int) ClientPacketId.Delete, DeleteAccount);
		m_notifier.Add((int) ClientPacketId.Login, Login);
		m_notifier.Add((int) ClientPacketId.Logout, Logout);
		m_notifier.Add((int) ClientPacketId.GameClose, GameClose);

        Thread handleThread = new Thread(new ThreadStart(DataHandle));
        handleThread.Start();
    }
    
	public void DataHandle ()
	{
        while (true)
        {
            if (receiveMsgs.Count != 0)
            {
                //패킷을 Dequeue 한다 패킷 : 메시지 타입 + 메시지 내용
                tcpPacket = receiveMsgs.Dequeue();

                //타입과 내용을 분리한다
                byte Id = tcpPacket.msg[0];
                msg = new byte[tcpPacket.msg.Length - 1];
                Array.Copy(tcpPacket.msg, 1, msg, 0, msg.Length);

                HeaderData headerData = new HeaderData();

                //Dictionary에 등록된 델리게이트형 메소드에서 msg를 반환받는다.
                if (m_notifier.TryGetValue(Id, out recvNotifier))
                {
                    //send 할 id를 반환받음
                    if (recvNotifier(msg) == ServerPacketId.None)
                        return;
                }
                else
                {
                    Console.WriteLine("DataHandler::TryGetValue 에러 " + Id);
                    headerData.Id = (byte)ServerPacketId.None;
                }

                tcpPacket = new TcpPacket(msg, tcpPacket.client);
                sendMsgs.Enqueue(tcpPacket);
            }
        }
	}

	public ServerPacketId CreateAccount (byte[] data)
	{
		Console.WriteLine (tcpPacket.client.RemoteEndPoint.ToString() + " 가입요청");

		AccountPacket accountPacket = new AccountPacket (data);
		AccountData accountData = accountPacket.GetData ();

		Console.WriteLine ("아이디 : " + accountData.Id + "패스워드 : " + accountData.password);

        msg = new byte[1024];

		try
		{
			if (database.AddUserData (accountData.Id, accountData.password))
			{
                msg[0] = (byte) UnityServer.Result.Success;
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
			Console.WriteLine ("DataHandler::AddPlayerData 에러");
            Console.WriteLine("가입 실패");
            msg[0] = (byte)UnityServer.Result.Fail;
        }

        Array.Resize(ref msg, 1);

        return ServerPacketId.CreateResult;
	}

	public ServerPacketId DeleteAccount (byte[] data)
	{
		Console.WriteLine (tcpPacket.client.RemoteEndPoint.ToString() + " 탈퇴요청");

		AccountPacket accountPacket = new AccountPacket (data);
		AccountData accountData = accountPacket.GetData ();

		Console.WriteLine ("아이디 : " + accountData.Id + "패스워드 : " + accountData.Id);

		try
		{
			if (database.DeleteUserData (accountData.Id, accountData.password))
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
			Console.WriteLine ("DataHandler::RemovePlayerData 에러");
            Console.WriteLine("탈퇴 실패");
            msg[0] = (byte)UnityServer.Result.Fail;
        }

        Array.Resize(ref msg, 1);

        return ServerPacketId.DeleteResult;
	}

	public ServerPacketId Login (byte[] data)
	{
		Console.WriteLine (tcpPacket.client.RemoteEndPoint.ToString() + " 로그인요청");

		AccountPacket accountPacket = new AccountPacket (data);
		AccountData accountData = accountPacket.GetData ();

		Console.WriteLine ("아이디 : " + accountData.Id + "비밀번호 : " + accountData.password);

		try
		{
			if (database.UserData.ContainsKey (accountData.Id))
			{
				if (((UserData)database.UserData[accountData.Id]).PW == accountData.password)
				{
					if(!LoginUser.ContainsValue(accountData.Id))
					{
                        msg[0] = (byte)UnityServer.Result.Success;
                        Console.WriteLine ("로그인 성공");
						((TcpClient)LoginUser[tcpPacket.client]).Id = accountData.Id;
					}
					else
					{
						Console.WriteLine ("현재 접속중인 아이디입니다.");
                        msg[0] = (byte)UnityServer.Result.Fail;
                    }
				}
				else
				{
					Console.WriteLine ("패스워드가 맞지 않습니다.");
                    msg[0] = (byte)UnityServer.Result.Fail;
                }
			}
			else
			{
				Console.WriteLine ("존재하지 않는 아이디입니다.");
                msg[0] = (byte)UnityServer.Result.Fail;
            }
		}
		catch
		{
			Console.WriteLine ("DataHandler::PlayerData.Contains 에러");
            msg[0] = (byte)UnityServer.Result.Fail;
        }
        Array.Resize(ref msg, 1);

        return ServerPacketId.LoginResult;
	}

	public ServerPacketId Logout (byte[] data)
	{
		Console.WriteLine (tcpPacket.client.RemoteEndPoint.ToString() + " 로그아웃요청");

		string id = ((TcpClient)LoginUser[tcpPacket.client]).Id;

		try
		{
			if (LoginUser.Contains (id))
			{
				LoginUser.Remove(id);
				Console.WriteLine(id + "로그아웃");
                msg[0] = (byte)UnityServer.Result.Success;
            }
			else
			{
				Console.WriteLine ("로그인되어있지 않은 아이디입니다.");
                msg[0] = (byte)UnityServer.Result.Fail;
            }
		}
		catch
		{
			Console.WriteLine ("DataHandler::PlayerData.Contains 에러");
            msg[0] = (byte)UnityServer.Result.Fail;
        }

        Array.Resize(ref msg, 1);

        return ServerPacketId.None;
	}

	public ServerPacketId GameClose (byte[] data)
	{
		try
		{
            Console.WriteLine(tcpPacket.client.RemoteEndPoint.ToString() + "가 접속을 종료했습니다.");
            tcpPacket.client.Close();			
		}
		catch
		{
			Console.WriteLine ("DataHandler::LoginUser.Close 에러");
		}

		return ServerPacketId.None;
	}

    public ServerPacketId RequestHeroData(byte[] data)
    {
        string Id = ((TcpClient)LoginUser[tcpPacket.client]).Id;
        int heroId = ((UserData)database.UserData[Id]).HeroId;
        int level = ((UserData)database.UserData[Id]).HeroLevel;

        HeroData heroData = new HeroData(heroId, level);
        HeroDataPacket heroDataPacket = new HeroDataPacket(heroData);

        msg = CreatePacket(heroDataPacket, ServerPacketId.HeroData);        

        return ServerPacketId.HeroData;
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