using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class Database
{
    Hashtable accountData;
    Hashtable userData;
    Hashtable worldMapData;
    FileStream fs;
    BinaryFormatter bin;
    
    public Hashtable AccountData { get { return accountData; } }
    public Hashtable UserData { get { return userData;}}
    public const string accountDataFile = "AccountData.data";
    public const string worldMapDataFile = "WorldMapData.data";

    BuildingDatabase buildingDatabase;
    UnitDatabase unitDatabase;

    public Database()
    {
        bin = new BinaryFormatter();
        fs = new FileStream(accountDataFile, FileMode.OpenOrCreate);

        accountData = GetData(accountDataFile);
        worldMapData = GetData(worldMapDataFile);

        userData = new Hashtable();

        buildingDatabase = BuildingDatabase.Instance;
        buildingDatabase.InitializeBuildingDatabase();
        unitDatabase = UnitDatabase.Instance;
        unitDatabase.InitializeUnitDatabase();
        InitializeWorldMapData();
    }

    public Hashtable GetData(string path)
    {
        fs.Close();
        fs = new FileStream(path, FileMode.OpenOrCreate);

        try
        {
            if (fs.Length > 0)
            {
                return (Hashtable)bin.Deserialize(fs);
            }
            else
            {
                return new Hashtable();
            }
        }
        catch
        {
            Console.WriteLine("Database::GetData 에러");
            return new Hashtable();
        }
    }

    public bool AddAccountData(string Id, string Pw)
    {
        try
        {
            if (!accountData.Contains(Id))
            {
                accountData.Add(Id, new LoginData(Id, Pw));                

                while (true)
                {
                    GetAccountData(Id).SetPosition();

                    try
                    {
                        AddPlaceData((int)PlaceType.Castle, GetAccountData(Id).XPos, GetAccountData(Id).YPos, 0, Id);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("같은 위치에 생성되었습니다");
                    }
                }

                FileSave(accountDataFile, accountData);
                FileSave(Id + ".data", new UserData(Id));
                FileSave(worldMapDataFile, worldMapData);
                return true;
            }
            else
            {
                Console.WriteLine("이미 존재하는 아이디");
                return false;
            }
        }
        catch
        {
            Console.WriteLine("Database::AddUserData.Add 에러");
            return false;
        }
    }

    public bool DeleteAccountData(string Id, string Pw)
    {
        try
        {
            if (accountData.Contains(Id))
            {
                if (((LoginData)accountData[Id]).PW == Pw)
                {
                    accountData.Remove(Id);
                    FileSave(accountDataFile, accountData);

                    FileInfo file = new FileInfo(Id + ".data");
                    file.Delete();

                    return true;
                }
                else
                {
                    Console.WriteLine("잘못된 비밀번호");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("존재하지 않는 아이디");
                return false;
            }            
        }
        catch
        {
            Console.WriteLine("Database::DeleteAccountData.Contains 에러");
            return false;
        }
    }
    
    public UserData GetAccountData(string Id)
    {
        if (userData.Contains(Id))
        {
            return (UserData) userData[Id];
        }
        else
        {
            return AddUserData(Id);
        }
    }

    public UserData AddUserData(string Id)
    {
        fs.Close();
        //파일이 있으면 가져오고 없으면 새로 만듬
        try
        {
            fs = new FileStream(Id + ".data", FileMode.OpenOrCreate);
        }
        catch
        {
            Console.WriteLine("Database::GetAccountData.FileOpenOrCreate 에러");
        }

        UserData newUserData;

        //원래 있는 경우에는 그 파일의 데이터를 가져오고
        if (fs.Length > 0)
        {
            newUserData = (UserData)bin.Deserialize(fs);
        }
        //없을 경우에는 새로 만들어서 가져옴
        else
        {
            newUserData = new UserData(Id);
        }

        //데이터를 유저리스트 테이블에 추가한 뒤 반환
        userData.Add(Id, newUserData);
        return newUserData;
    }

    public void AddPlaceData(int placeType, short xPos, short yPos, int level, string Id)
    {
        if (!worldMapData.ContainsKey(xPos * 1000 + yPos))
        {
            worldMapData.Add(xPos * 1000 + yPos, new Place(placeType, new Position(xPos, yPos), level, Id));
        }
        else
        {
            Console.WriteLine("이미 있는 위치에 생성합니다.");
        }
    }

    public Place GetPlaceData(int xPos, int yPos)
    {
        return (Place) worldMapData[xPos * 100 + yPos];
    }

    public Place[] GetWorldMapData()
    {
        Place[] worldMap = new Place[worldMapData.Count];
        int i = 0;

        foreach (DictionaryEntry place in worldMapData)
        {
            worldMap[i++] = (Place) place.Value;
        }

        return worldMap;
    }

    public Place GetPlaceData(int placeNum)
    {
        if (worldMapData.Contains(placeNum))
        {
            return (Place)worldMapData[placeNum];
        }
        else
        {
            return null;
        }
    }

    public void InitializeWorldMapData()
    {
        AddPlaceData((int)PlaceType.Castle, 0, 0, 1, "Monster");
        AddPlaceData((int)PlaceType.Dungeon, 10, 0, 5, "StrongMonster");
        FileSave(worldMapDataFile, worldMapData);
    }

    public bool FileSave(string path, object data)
    {
        try
        {
            fs.Close();
            fs = new FileStream(path, FileMode.Create);
        }
        catch
        {
            Console.WriteLine("Database::FileSave.FileMode.Create 에러");
            return false;
        }

        try
        {
            bin.Serialize(fs, data);
        }
        catch (Exception e)
        {
            Console.WriteLine("Database::FileSaveSerialize 에러" + e.Message);
            return false;
        }

        return true;
    }
}

[Serializable]
public class LoginData
{
    string Id;
    string Pw;

    public string ID { get { return Id; } }
    public string PW { get { return Pw; } }

    public LoginData(string newId, string newPw)
    {
        Id = newId;
        Pw = newPw;
    }
}

public enum PlaceType
{
    Castle,
    Resources,
    Dungeon,
    None,
}

[Serializable]
public class Place
{
    byte type;
    Position position;
    byte level;
    string Id;

    public byte Type { get { return type; } }
    public Position Position { get { return position; } }
    public byte Level { get { return level; } }
    public string ID { get { return Id; } }

    public Place()
    {
        type = (int) PlaceType.Castle;
        position = new Position(0, 0);
        level = 0;
    }

    public Place(int newPlaceType, Position newPos, int newLevel, string newId)
    {
        type = (byte)newPlaceType;
        position = new Position(newPos);
        level = (byte)newLevel;
        Id = newId;
    }
}

[Serializable]
public class Position
{
    short x;
    short y;

    public short X { get { return x; } }
    public short Y { get { return y; } }

    public Position(Position position)
    {
        x = position.x;
        y = position.y;
    }

    public Position(short _x, short _y)
    {
        x = _x;
        y = _y;
    }
}