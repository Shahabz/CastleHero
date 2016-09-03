using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class Database
{
    Hashtable userData;
    FileStream userDataFs;
    BinaryFormatter bin;
    
    public Hashtable UserData { get { return userData; } }
    public const string userDataFile = "UserData.data";

    /*
    유저 데이터 파일 : UserData.data
    맵 데이터 파일 : MapData.data
    
    */

    public Database()
    {
        userDataFs = new FileStream(userDataFile, FileMode.OpenOrCreate);
        bin = new BinaryFormatter();

        if(userDataFs.Length > 0)
        {
            userData = (Hashtable) bin.Deserialize(userDataFs);
        }
        else
        {
            userData = new Hashtable();
        }
        
        //worldMap Data
    }

    public bool AddUserData(string Id, string Pw)
    {
        try
        {
            UserData newUserData = new UserData(Id, Pw);
            if (!userData.Contains(Id))
            {
                userData.Add(Id, newUserData);
                FileSave(userDataFile);
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

    public bool DeleteUserData(string Id, string Pw)
    {
        try
        {
            if (UserData.Contains(Id))
            {
                if (((UserData)userData[Id]).PW == Pw)
                {
                    userData.Remove(Id);
                    FileSave(userDataFile);
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

    public bool ChangeUserData(string Id, int level)
    {
        try
        {

        }
        catch
        {

        }
        return true;
    }

    public bool FileSave(string path)
    {
        try
        {
            userDataFs.Close();
            userDataFs = new FileStream(path, FileMode.Create);
        }
        catch
        {
            Console.WriteLine("Database::FileSave.FileMode.Create 에러");
            return false;
        }
        
        try
        {
            bin.Serialize(userDataFs, userData);
        }
        catch (Exception e)
        {
            Console.WriteLine("Database::FileSaveSerialize 에러" + e.Message);
            return false;
        }
        
        try
        {
            userDataFs.Close();
            userDataFs = new FileStream(path, FileMode.Open);
        }
        catch
        {
            Console.WriteLine("Database::FileSave.FileMode.Open 에러");
            return false;
        }

        return true;
    }
}