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
    StateDataRequest,
    Build,
    BuildCancel,
    BuildDataRequest,
    BuildComplete,
    UnitCreate,
    UnitCreateDataRequest,
    UnitCreateComplete,
    MyPositionDataRequest,
    PlaceDataRequest,
    EnemyUnitNumRequest,
    Attack,
    EnemyUnitDataRequest,
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
    ResourceData,
    StateData,
    BuildData,
    UnitCreateData,
    MyPositionData,
    PlaceData,
    EnemyUnitNumData,
    EnemyUnitData,
}