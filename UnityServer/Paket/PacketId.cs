public class HeaderData  {
	// 헤더 == [2바이트 - 패킷길이][1바이트 - ID]
	public short length; // 패킷의 길이
	public byte Id; // 패킷 ID
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
    ItemDataRequest,
    SkillDataRequest,
    UnitDataRequest,
    BuildingDataRequest,
    UpgradeDataRequest,
    ResourcesDataRequest,
}

public enum ServerPacketId
{
	None = 0,
	CreateResult,
	DeleteResult,
	LoginResult,
    HeroData,
    ItemData,
    SkillData,
    UnitData,
    BuildingData,
    UpgradeData,
    ResourcesData,
}

// 클라이언트-To-서버 가입데이터
public struct AccountData
{
	public string Id;
	public string password;
}

public struct ResultData
{
	public string result;
	const string Fail = "Fail";
	const string Success = "Success";
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

    public HeroData(int newId, int newLevel)
    {
        Id = (byte) newId;
        level = (byte) newLevel;
    }
}