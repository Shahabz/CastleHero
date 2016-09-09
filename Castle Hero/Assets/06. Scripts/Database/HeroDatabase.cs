using System.Collections.Generic;

public enum UnitID
{
    None,
    Unity,

}

public enum State
{
    Idle = 1,
    Move,
    Attack,
    Die,
    Skill1,
    Skill2,
    Skill3,
}

public class HeroDatabase
{
    private static HeroDatabase instance;

    public static HeroDatabase Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new HeroDatabase();
            }

            return instance;
        }
    }

    List<HeroBaseData> HeroData;

    public void InitializeHeroDatabase()
    {
        HeroData = new List<HeroBaseData>();
        
        //이름, 공격범위, 충돌범위 
        HeroData.Add(new HeroBaseData((int)UnitID.None, 1.0f, 1.0f));
        HeroData.Add(new HeroBaseData((int)UnitID.Unity, 1.0f, 1.0f));

        //레벨, 경험치, 공격력, 방어력, 마법방어력, 체력, 마나, 이동속도, 공격속도, 체력리젠, 마나리젠
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(1, 100, 5, 0, 0, 40, 5, 5, 0.8f, 1, 0, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(2, 200, 7, 0, 0, 45, 7, 5, 0.85f, 1, 0, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(3, 400, 9, 0, 0, 50, 9, 5, 0.9f, 1, 0, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(4, 750, 11, 0, 0, 55, 11, 5, 0.95f, 1, 0, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(5, 1300, 13, 0, 0, 60, 13, 5, 1f, 1, 1, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(6, 2100, 15, 0, 0, 65, 15, 5, 1.05f, 1, 1, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(7, 3200, 17, 1, 0, 71, 17, 5, 1.1f, 1, 1, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(8, 4650, 19, 2, 0, 77, 19, 5, 1.15f, 1, 1, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(9, 6500, 21, 3, 0, 83, 21, 5, 1.2f, 1, 2, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(10, 8800, 23, 4, 0, 89, 23, 5, 1.25f, 1, 2, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(11, 11600, 25, 5, 0, 95, 26, 5, 1.3f, 1, 2, 0));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(12, 14950, 27, 6, 1, 102, 29, 5, 1.35f, 1, 2, 1));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(13, 18900, 29, 7, 2, 109, 32, 5, 1.4f, 1, 3, 1));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(14, 23500, 31, 8, 3, 116, 35, 5, 1.45f, 1, 3, 1));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(15, 28800, 33, 9, 4, 123, 38, 5, 1.5f, 1, 3, 1));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(16, 34850, 35, 10, 5, 130, 41, 5, 1.55f, 1, 3, 1));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(17, 41700, 37, 12, 6, 138, 44, 5, 1.6f, 1, 4, 1));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(18, 49400, 39, 14, 7, 146, 47, 5, 1.65f, 1, 4, 1));
        HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(19, 58000, 41, 16, 9, 154, 50, 5, 1.7f, 1, 4, 1));
    }

    public HeroBaseData GetHeroData(int Id)
    {
        if(HeroData.Count > Id)
        {
            for(int i = 0; i< HeroData.Count; i++)
            {
                if(HeroData[i].ID == Id)
                {
                    return HeroData[i];
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }
}

public class HeroBaseData
{
    int Id;
    float attackRange;
    float colliderSize;

    List<HeroLevelData> levelData;

    public int ID { get { return Id; } }
    public float AttackRange { get { return attackRange; } }
    public float ColliderSize { get { return colliderSize; } }
    public List<HeroLevelData> Leveldata { get { return levelData; } }

    public HeroBaseData()
    {
        Id = 0;
        attackRange = 0;
        colliderSize = 0;
        levelData = new List<HeroLevelData>();
    }

    public HeroBaseData(int newId, float newAttackRange, float newColliderSize)
    {
        Id = newId;
        attackRange = newAttackRange;
        colliderSize = newColliderSize;
        levelData = new List<HeroLevelData>();
    }

    public HeroBaseData(HeroBaseData newBaseData)
    {
        Id = newBaseData.Id;
        attackRange = newBaseData.attackRange;
        colliderSize = newBaseData.colliderSize;
        levelData = new List<HeroLevelData>();
    }

    public HeroLevelData GetLevelData(int level)
    {
        if (levelData.Count >= level)
        {
            return levelData[level];
        }

        return null;
    }

    public bool AddLevelData(HeroLevelData newHeroLevelData)
    {
        try
        {
            levelData.Add(newHeroLevelData);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveLevelData(int level)
    {
        int index = FindLevelDataIndex(level);

        if (index != -1)
        {
            try
            {
                levelData.Remove(levelData[index]);
                return true;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool ChangeLevelData(HeroLevelData heroLevelData)
    {
        int index = FindLevelDataIndex(heroLevelData.Level);

        if (index != -1)
        {
            try
            {
                levelData[index] = heroLevelData;
                return true;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public int FindLevelDataIndex(int level)
    {
        for (int i = 0; i < levelData.Count; i++)
        {
            if (levelData[i].Level == level)
            {
                return i;
            }
        }

        return -1;
    }
}

public class HeroLevelData
{
    int level;
    int experience;
    int attack;
    int defense;
    int magicDefense;
    int health;
    int mana;
    float moveSpeed;
    float attackSpeed;
    float rotateSpeed;
    int healthRegeneration;
    int manaRegeneration;

    public int Level { get { return level; } }
    public int Experience { get { return experience; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int MagicDefense { get { return magicDefense; } }
    public int Health { get { return health; } }
    public int Mana { get { return mana; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float RotateSpeed { get { return rotateSpeed; } }
    public int HealthRegeneration { get { return healthRegeneration; } }
    public int ManaRegeneration { get { return manaRegeneration; } }

    public HeroLevelData()
    {
        level = 0;
        experience = 0;
        attack = 0;
        defense = 0;
        magicDefense = 0;
        health = 0;
        mana = 0;
        moveSpeed = 0;
        attackSpeed = 0;
        rotateSpeed = 0;
        healthRegeneration = 0;
        manaRegeneration = 0;
    }

    public HeroLevelData(int newLevel, int newExperience, int newAttack, int newDefense, int newMagicDefense, int newHealth,
        int newMana, float newMoveSpeed, float newAttackSpeed, float newRotateSpeed, int newHealthRegeneration, int newManaRegeneration)
    {
        level = newLevel;
        experience = newExperience;
        attack = newAttack;
        defense = newDefense;
        magicDefense = newMagicDefense;
        health = newHealth;
        mana = newMana;
        moveSpeed = newMoveSpeed;
        attackSpeed = newAttackSpeed;
        rotateSpeed = newRotateSpeed;
        healthRegeneration = newHealthRegeneration;
        manaRegeneration = newManaRegeneration;
    }

    public HeroLevelData(HeroLevelData heroLevelData)
    {
        level = heroLevelData.level;
        experience = heroLevelData.experience;
        attack = heroLevelData.attack;
        defense = heroLevelData.defense;
        magicDefense = heroLevelData.magicDefense;
        health = heroLevelData.health;
        mana = heroLevelData.mana;
        moveSpeed = heroLevelData.moveSpeed;
        attackSpeed = heroLevelData.attackSpeed;
        rotateSpeed = heroLevelData.rotateSpeed;
        healthRegeneration = heroLevelData.healthRegeneration;
        manaRegeneration = heroLevelData.manaRegeneration;
    }
}

