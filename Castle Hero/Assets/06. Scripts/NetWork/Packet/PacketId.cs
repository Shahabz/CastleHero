public class HeaderData
{
    // 헤더 == [2바이트 - 패킷길이][1바이트 - ID]
    public short length; // 패킷의 길이
    public byte Id; // 패킷 ID

    public HeaderData()
    {
        length = 0;
        Id = 0;
    }
}

public enum ClientPacketId
{
    None = 0,
    Create,
    Delete,
    Login,
    Logout,
    GameClose,
    HeroDataRequest,
    SkillDataRequest,
    ItemDataRequest,    
    UnitDataRequest,
    BuildingDataRequest,
    UpgradeDataRequest,
    ResourceDataRequest,
}

public enum ServerPacketId
{
    None = 0,
    CreateResult,
    DeleteResult,
    LoginResult,
    HeroData,
    SkillData,
    ItemData,
    UnitData,
    BuildingData,
    UpgradeData,
    ResourcesData,
}

// 클라이언트-To-서버 가입데이터
public class AccountData
{
    public string Id;
    public string Pw;

    public AccountData()
    {
        Id = "";
        Pw = "";
    }

    public AccountData(string newId, string newPw)
    {
        Id = newId;
        Pw = newPw;
    }
}

public class HeroData
{
    public byte Id;
    public byte level;

    public HeroData()
    {
        Id = 1;
        level = 1;
    }

    public HeroData(byte newId, byte newLevel)
    {
        Id = newId;
        level = newLevel;
    }
}

public class SkillData
{
    public const int skillMaxNum = 15;
    public short[] skillLevel;

    public SkillData()
    {
        skillLevel = new short[skillMaxNum];

        for(int i =0; i < skillMaxNum; i++)
        {
            skillLevel[i] = 0;
        }
    }
}

public class BuildingData
{
    public const int buildingMaxNum = 5;
    public short[] buildingLevel;

    public BuildingData()
    {
        buildingLevel = new short[buildingMaxNum];

        for (int i = 0; i < buildingMaxNum; i++)
        {
            buildingLevel[i] = 0;
        }
    }
}

