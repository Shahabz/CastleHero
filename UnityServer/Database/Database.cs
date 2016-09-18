using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class Database
{
    Hashtable accountData;
    Hashtable userData;
    FileStream fs;
    BinaryFormatter bin;
    
    public Hashtable AccountData { get { return accountData; } }
    public Hashtable UserData { get { return userData;}}
    public const string accountDataFile = "AccountData.data";

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
        //worldMap Data
    }

    public bool AddAccountData(string Id, string Pw)
    {
        try
        {
            if (!accountData.Contains(Id))
            {
                accountData.Add(Id, new LoginData(Id, Pw));
                FileSave(accountDataFile, accountData);
                FileSave(Id + ".data", new UserData(Id, Pw));
                
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
                if (((UserData)accountData[Id]).PW == Pw)
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
            Console.WriteLine("Database::AddUserData.Add 에러");
            return false;
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

    public UserData GetAccountData(string Id)
    {
        if (userData.Contains(Id))
        {
            Console.WriteLine("기존 데이터 로드");
            return (UserData) userData[Id];
        }
        else
        {
            Console.WriteLine("새로운 데이터 로드");
            fs.Close();
            try
            {
                fs = new FileStream(Id + ".data", FileMode.Open);
            }
            catch
            {
                Console.WriteLine("Database::GetAccountData.FileOpen 에러");
                return null;
            }            

            if (fs.Length > 0)
            {
                UserData newUserData = (UserData)bin.Deserialize(fs);
                userData.Add(Id, newUserData);
                return newUserData;
            }
            else
            {
                return null;
            }
        }        
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