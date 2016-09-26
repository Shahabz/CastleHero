using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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
    public const string worldMapDataFile = "worldMapData.data";

    BuildingDatabase buildingDatabase;
    UnitDatabase unitDatabase;

    /*
    아이디, 비밀번호 파일 : AccountData.data
    유저 데이터 파일 : UserData.data
    맵 데이터 파일 : MapData.data
    */

    public Database()
    {
        bin = new BinaryFormatter();
        fs = new FileStream(accountDataFile, FileMode.OpenOrCreate);

        if (fs.Length > 0)
        {
            accountData = (Hashtable)bin.Deserialize(fs);
        }
        else
        {
            accountData = new Hashtable();
        }

        userData = new Hashtable();
        worldMapData = new Hashtable();

        buildingDatabase = BuildingDatabase.Instance;
        buildingDatabase.InitializeBuildingDatabase();
        unitDatabase = UnitDatabase.Instance;
        unitDatabase.InitializeUnitDatabase();
    }

    public bool AddAccountData(string Id, string Pw)
    {
        try
        {
            if (!accountData.Contains(Id))
            {
                accountData.Add(Id, new LoginData(Id, Pw));
                FileSave(accountDataFile, accountData);
                FileSave(Id + ".data", new UserData(Id));

                while (true)
                {
                    GetAccountData(Id).SetPosition();
                    try
                    {
                        AddPlaceData(PlaceType.Castle, GetAccountData(Id).XPos, GetAccountData(Id).YPos, 0, Id);
                        Console.WriteLine(GetAccountData(Id).XPos + ", " + GetAccountData(Id).YPos);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("같은 위치에 생성되었습니다");
                    }                
                }                

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

    public void AddPlaceData(PlaceType placeType, int xPos, int yPos, int level, string Id)
    {
        try
        {
            worldMapData.Add(new Position(xPos, yPos), new Place(placeType, new Position(xPos, yPos), level, Id));
        }
        catch
        {
            Console.WriteLine("이미 있는 위치에 생성합니다.");
        }
        
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
    Deungeon,
    None,
}

[Serializable]
public class Place
{
    public PlaceType placeType;
    Position position;
    int level;
    string Id;

    public Place()
    {
        placeType = PlaceType.Castle;
        position = new Position(0, 0);
        level = 0;
    }

    public Place(PlaceType newPlaceType, Position newPos, int newLevel, string newId)
    {
        placeType = newPlaceType;
        position = new Position(newPos);
        level = newLevel;
        Id = newId;
    }
}

[Serializable]
public class Position
{
    int x;
    int y;

    public int X { get { return x; } }
    public int Y { get { return y; } }

    public Position(Position position)
    {
        x = position.x;
        y = position.y;
    }

    public Position(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}