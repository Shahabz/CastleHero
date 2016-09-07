using System.Collections.Generic;

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

