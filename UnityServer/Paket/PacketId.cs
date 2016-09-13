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
}