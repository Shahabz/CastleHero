using System;
using UnityEngine;

class DataManager : MonoBehaviour
{
    public enum CastleState
    {
        Peace = 0,
        Famine,
        Attacked,
        BeingAttacked
    }

    public enum HeroState
    {
        Stationed = 0,
        Attack,
        Return,
        Dead,
    }

    HeroDatabase heroDatabase;
    ItemDatabase itemDatabase;
    BuildingDatabase buildingDatabase;

    public const int equipNum = 7;
    public const int invenNum = 16;
    public const int skillNum = 15;
    public const int unitNum = 5;
    public const int buildingNum = 6;

    [SerializeField] string Id;
    [SerializeField] HeroBaseData heroData;
    [SerializeField] int[] equipment;
    [SerializeField] int[] inventoryId;
    [SerializeField] int[] inventoryNum;
    [SerializeField] int[] skill;
    [SerializeField] Unit[] unit;
    [SerializeField] Unit[] createUnit;
    [SerializeField] Unit[] attackUnit;
    [SerializeField] int[] building;
    [SerializeField] int buildBuilding;
    [SerializeField] DateTime buildTime;
    [SerializeField] int[] upgrade;
    [SerializeField] int resource;
    [SerializeField] HeroState heroState;
    [SerializeField] CastleState castleState;

    public string ID { get { return Id; } }
    public HeroBaseData HeroData { get { return heroData; } }
    public int[] Equipment { get { return equipment; } }
    public int[] InventoryId { get { return inventoryId; } }
    public int[] InventoryNum { get { return inventoryNum; } }
    public int[] Skill { get { return skill; } }
    public Unit[] Unit { get { return unit; } }
    public Unit[] CreateUnit { get { return createUnit; } }
    public Unit[] AttackUnit { get { return attackUnit; } }
    public int[] Building { get { return building; } }
    public int BuildBuilding { get { return buildBuilding; } set { buildBuilding = value; } }
    public DateTime BuildTime { get { return buildTime; } set { buildTime = value; } }
    public int[] Upgrade { get { return upgrade; } }
    public int Resource { get { return resource; } }
    public HeroState HState { get { return heroState; } }
    public CastleState CState { get { return castleState; } }

    void Awake()
    {
        tag = "DataManager";
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        InitializeData();
    }

    void InitializeData()
    {
        heroDatabase = HeroDatabase.Instance;
        heroDatabase.InitializeHeroDatabase();
        itemDatabase = ItemDatabase.Instance;
        itemDatabase.InitializeItemDatabase();
        buildingDatabase = BuildingDatabase.Instance;
        buildingDatabase.InitializeBuildingDatabase();

        Id = "";
        heroData = new HeroBaseData();
        resource = 0;
        heroState = 0;
        castleState = 0;

        equipment = new int[equipNum];
        inventoryId = new int[invenNum];
        inventoryNum = new int[invenNum];
        skill = new int[skillNum];
        unit = new Unit[unitNum];
        building = new int[buildingNum];
        upgrade = new int[unitNum];

        buildBuilding = buildingNum;
    }

    public void SetId(string newId)
    {
        Id = newId;
    }

    public void SetHeroData(HeroData newHeroData)
    {
        HeroBaseData baseData = heroDatabase.GetHeroData(newHeroData.Id);
        HeroLevelData levelData = baseData.GetLevelData(newHeroData.level);

        heroData = new HeroBaseData(heroDatabase.GetHeroData(newHeroData.Id));
        heroData.Leveldata.Add(levelData);
    }

    public void SetItemData(ItemData itemData)
    {
        for (int i = 0; i < equipNum; i++)
        {
            equipment[i] = itemData.equipment[i];
        }

        for (int i = 0; i < invenNum; i++)
        {
            inventoryId[i] = itemData.inventoryId[i];
            inventoryNum[i] = itemData.inventoryNum[i];
        }
    }

    public void SetSkillData(SkillData skillData)
    {
        for (int i = 0; i < skillNum; i++)
        {
            skill[i] = skillData.skillLevel[i];
        }
    }

    public void SetUnitData(UnitData[] unitData)
    {
        unit = new Unit[unitData[0].unitKind];
        createUnit = new Unit[unitData[1].unitKind];
        attackUnit = new Unit[unitData[2].unitKind];

        for (int i = 0; i < unitData[0].unitKind; i++)
        {
            unit[i] = new Unit(unitData[0].unit[i]);
        }        

        for (int i = 0; i < unitData[1].unitKind; i++)
        {
            createUnit[i] = new Unit(unitData[1].unit[i]);
        }        

        for (int i = 0; i < unitData[2].unitKind; i++)
        {
            attackUnit[i] = new Unit(unitData[2].unit[i]);
        }
    }

    public void SetBuildingData(BuildingData buildingData)
    {
        for (int i = 0; i < buildingNum; i++)
        {
            building[i] = buildingData.building[i];         
        }
    }

    public void SetUpgradeData(UpgradeData upgradeData)
    {
        for (int i = 0; i < unitNum; i++)
        {
            upgrade[i] = upgradeData.upgrade[i];
        }
    }

    public void SetResourceData(ResourceData resourceData)
    {
        resource = resourceData.resource;
    }

    public void SetStateData(StateData stateData)
    {
        heroState = (HeroState) stateData.heroData;
        castleState = (CastleState) stateData.castleData;
    }

    public void SetBuildData(BuildData buildData)
    {
        buildBuilding = buildData.Id;

        if(buildBuilding != buildingNum)
        {
            int level = building[buildData.Id] + 1;
            buildTime = new DateTime(buildData.year, buildData.month, buildData.day, buildData.hour, buildData.minute, buildData.second) + buildingDatabase.GetBuildingData(buildBuilding).GetLevelData(level).BuildTime;
        }
        else
        {
            buildTime = DateTime.Now;
        }
		
        Debug.Log("빌드 데이터 : " + buildData.Id + " , " + buildTime.ToString());
    }
}
