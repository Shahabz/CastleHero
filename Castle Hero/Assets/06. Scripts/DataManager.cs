using UnityEngine;

class DataManager : MonoBehaviour
{
    HeroDatabase heroDatabase;
    public const int equipNum = 7;
    public const int invenNum = 16;
    public const int skillNum = 15;
    public const int unitNum = 5;
    public const int buildingNum = 7;

    [SerializeField] string Id;
    [SerializeField] int heroId;
    [SerializeField] float attackRange;
    [SerializeField] float colliderSize;

    [SerializeField] int level;
    [SerializeField] int currentExperience;
    [SerializeField] int maxExperience;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int magicDefense;
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] int currentMana;
    [SerializeField] int maxMana;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] int healthRegeneration;
    [SerializeField] int manaRegeneration;

    [SerializeField] Item[] equipment;
    [SerializeField] Item[] inventory;
    [SerializeField] int[] skill;
    [SerializeField] Unit[] unit;
    [SerializeField] int[] building;
    [SerializeField] int[] upgrade;
    [SerializeField] int gold;

    public string ID { get { return Id; } }
    public int HeroId { get { return heroId; } }
    public float AttackRange { get { return attackRange; } }
    public float ColliderSize { get { return colliderSize; } }

    public int Level { get { return level; } }
    public int CurrentExperience { get { return currentExperience; } }
    public int MaxExperience { get { return maxExperience; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return Defense; } }
    public int MagicDefens { get { return magicDefense; } }
    public int CurrnetHealth { get { return currentHealth; } }
    public int MaxHealth { get { return maxHealth; } }
    public int CurrentMana { get { return currentMana; } }
    public int MaxMana { get { return maxMana; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float RotateSpeed { get { return rotateSpeed; } }
    public int HealthRegeneration { get { return healthRegeneration; } }
    public int ManaRegeneration { get { return manaRegeneration; } }

    public Item[] Equipment { get { return equipment; } }
    public Item[] Inventory { get { return inventory; } }
    public int[] Skill { get { return skill; } }
    public Unit[] Unit { get { return unit; } }
    public int[] Building { get { return building; } }
    public int[] Upgrade { get { return upgrade; } }
    public int Gold { get { return gold; } }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        heroDatabase = new HeroDatabase();
        heroId = 1;
        level = 1;

        equipment = new Item[equipNum];
        inventory = new Item[invenNum];
        skill = new int[skillNum];
        unit = new Unit[unitNum];
        building = new int[buildingNum];
        upgrade = new int[unitNum];
    }

    public void SetId(string newId)
    {
        Id = newId;
    }

    public void SetHeroData(HeroData newHeroData)
    {
        HeroBaseData baseData = heroDatabase.GetHeroData(newHeroData.Id);
        HeroLevelData levelData = baseData.GetLevelData(newHeroData.level);
        

        level = newHeroData.level;
        heroId = newHeroData.Id;
        attackRange = baseData.AttackRange;
        colliderSize = baseData.ColliderSize;

        maxExperience = levelData.Experience;
        attack = levelData.Attack;
        defense = levelData.Defense;
        magicDefense = levelData.MagicDefens;
        maxHealth = levelData.Health;
        maxMana = levelData.Mana;
        moveSpeed = levelData.MoveSpeed;
        attackSpeed = levelData.AttackSpeed;
        rotateSpeed = levelData.RotateSpeed;
        healthRegeneration = levelData.HealthRegeneration;
        manaRegeneration = levelData.ManaRegeneration;

        currentExperience = 0;
        currentHealth = 0;
        currentMana = 0;

        equipment = new Item[equipNum];
        inventory = new Item[invenNum];
        skill = new int[skillNum];
        unit = new Unit[unitNum];
        building = new int[buildingNum];
        upgrade = new int[unitNum];
    }

    public void SetItemData(ItemData itemData)
    {
        for (int i = 0; i < equipNum; i++)
        {
            equipment[i] = itemData.equipment[i];
        }

        for (int i = 0; i < invenNum; i++)
        {
            inventory[i] = itemData.inventory[i];
        }
    }

    public void SetSkillData(SkillData skillData)
    {
        for (int i = 0; i < skillNum; i++)
        {
            Skill[i] = skillData.skillLevel[i];
        }
    }

    public void SetUnitData(UnitData unitData)
    {
        unit = new Unit[unitData.unitKind];

        for (int i = 0; i < unitData.unitKind; i++)
        {
            unit[i] = new Unit(unitData.unit[i]);
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
        gold = resourceData.gold;
    }
}
