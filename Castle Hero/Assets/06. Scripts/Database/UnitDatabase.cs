using System;
using System.Collections.Generic;

public enum UnitId
{
    Gladiator,
    Archer,
    Paladin,
    Mage,
    Knight,
    None,
}

class UnitDatabase
{
    private static UnitDatabase instance;

    public static UnitDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UnitDatabase();
            }

            return instance;
        }
    }

    public List<UnitBaseData> unitData;

    public void InitializeUnitDatabase()
    {
        unitData = new List<UnitBaseData>();

        unitData.Add(new UnitBaseData(UnitId.Gladiator, "Gladiator", "검과 방패를 쓰는 검투사이다. 이동속도는 느리지만 근접전투에 강하다.", new TimeSpan(0, 0, 45), 100, 18));
        unitData[(int)UnitId.Gladiator].AddLevelData(1, 2, 0, 0, 10, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(2, 3, 0, 0, 14, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(3, 4, 0, 0, 18, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(4, 5, 0, 0, 22, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(5, 6, 0, 0, 26, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(6, 7, 0, 0, 30, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(7, 8, 0, 0, 34, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(8, 9, 0, 0, 38, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(9, 10, 0, 0, 42, 0, 4, 0.6f, 0, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(10, 11, 0, 0, 46, 0, 4, 0.6f, 1, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(11, 12, 0, 0, 50, 0, 5, 0.8f, 1, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(12, 13, 1, 1, 54, 0, 5, 0.8f, 1, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(13, 14, 1, 1, 58, 0, 5, 0.8f, 1, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(14, 15, 1, 1, 62, 0, 5, 0.8f, 1, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(15, 16, 1, 1, 66, 0, 5, 0.8f, 1, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(16, 17, 1, 1, 70, 0, 5, 0.8f, 1, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(17, 18, 1, 1, 74, 0, 5, 0.8f, 1, 0);
        unitData[(int)UnitId.Gladiator].AddLevelData(18, 19, 1, 1, 78, 0, 5, 0.8f, 1, 0);

        unitData.Add(new UnitBaseData(UnitId.Archer, "Archer", "활을 쏘는 궁수이다. 원거리 전투에 효과적이다.", new TimeSpan(0, 1, 30), 250, 18));
        unitData[(int)UnitId.Archer].AddLevelData(1, 3, 0, 0, 12, 0, 3, 0.6f, 0, 0);
        unitData[(int)UnitId.Archer].AddLevelData(2, 4, 0, 0, 15, 0, 3, 0.6f, 0, 0);
        unitData[(int)UnitId.Archer].AddLevelData(3, 4, 0, 0, 18, 0, 3, 0.6f, 0, 0);
        unitData[(int)UnitId.Archer].AddLevelData(4, 5, 0, 0, 21, 0, 3, 0.6f, 0, 0);
        unitData[(int)UnitId.Archer].AddLevelData(5, 5, 0, 0, 24, 0, 3, 0.6f, 0, 0);
        unitData[(int)UnitId.Archer].AddLevelData(6, 6, 0, 0, 27, 0, 3, 0.6f, 0, 0);
        unitData[(int)UnitId.Archer].AddLevelData(7, 6, 0, 0, 30, 0, 3, 0.8f, 0, 0);
        unitData[(int)UnitId.Archer].AddLevelData(8, 7, 0, 0, 33, 0, 3, 0.8f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(9, 7, 0, 0, 36, 0, 4, 0.8f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(10, 8, 0, 0, 39, 0, 4, 0.8f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(11, 9, 0, 0, 42, 0, 4, 1f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(12, 10, 0, 0, 45, 0, 4, 1f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(13, 11, 0, 0, 48, 0, 4, 1f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(14, 12, 0, 0, 51, 0, 4, 1f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(15, 13, 0, 0, 54, 0, 4, 1f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(16, 14, 0, 0, 57, 0, 4, 1f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(17, 15, 1, 0, 60, 0, 4, 1f, 1, 0);
        unitData[(int)UnitId.Archer].AddLevelData(18, 16, 1, 0, 63, 0, 4, 1f, 1, 0);

        unitData.Add(new UnitBaseData(UnitId.Paladin, "Paladin", "망치를 들고있는 팔라딘이다. 마법 방어력이 뛰어나다", new TimeSpan(0, 5, 0), 500, 18));
        unitData[(int)UnitId.Paladin].AddLevelData(1, 3, 0, 0, 30, 0, 3, 0.8f, 0, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(2, 4, 1, 0, 33, 0, 3, 0.8f, 0, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(3, 4, 1, 0, 36, 0, 3, 0.8f, 0, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(4, 5, 1, 0, 39, 0, 3, 0.8f, 0, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(5, 5, 1, 0, 42, 0, 3, 0.8f, 0, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(6, 6, 1, 0, 45, 0, 3, 0.8f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(7, 6, 2, 0, 48, 0, 4, 0.8f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(8, 7, 2, 0, 52, 0, 4, 0.8f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(9, 7, 2, 0, 56, 0, 4, 0.8f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(10, 8, 2, 0, 60, 0, 4, 0.8f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(11, 9, 2, 0, 64, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(12, 11, 3, 1, 68, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(13, 13, 3, 1, 73, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(14, 15, 3, 1, 78, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(15, 17, 3, 1, 83, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(16, 19, 3, 1, 88, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(17, 21, 4, 2, 93, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Paladin].AddLevelData(18, 23, 4, 2, 98, 0, 5, 1f, 1, 0);

        unitData.Add(new UnitBaseData(UnitId.Mage, "Mage", "마법을 사용하는 마법사이다. 강력한 화염마법을 구사한다.", new TimeSpan(0, 10, 0), 750, 18));
        unitData[(int)UnitId.Mage].AddLevelData(1, 5, 0, 0, 16, 0, 2, 0.6f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(2, 7, 0, 0, 18, 0, 2, 0.6f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(3, 9, 0, 0, 20, 0, 2, 0.6f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(4, 11, 0, 0, 22, 0, 2, 0.6f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(5, 13, 0, 0, 24, 0, 2, 0.6f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(6, 15, 0, 0, 26, 0, 2, 0.6f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(7, 17, 0, 0, 28, 0, 2, 0.8f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(8, 19, 0, 0, 30, 0, 2, 0.8f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(9, 21, 0, 0, 32, 0, 2, 0.8f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(10, 23, 0, 0, 34, 0, 2, 0.8f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(11, 25, 0, 1, 36, 0, 2, 1f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(12, 28, 0, 1, 38, 0, 2, 1f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(13, 31, 0, 1, 40, 0, 2, 1f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(14, 34, 0, 2, 42, 0, 2, 1f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(15, 37, 0, 2, 44, 0, 2, 1f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(16, 40, 0, 2, 46, 0, 2, 1f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(17, 43, 0, 2, 48, 0, 2, 1.4f, 0, 0);
        unitData[(int)UnitId.Mage].AddLevelData(18, 46, 0, 3, 50, 0, 2, 1.4f, 0, 0);

        unitData.Add(new UnitBaseData(UnitId.Knight, "Knight", "기동력이 뛰어난 기사이다. 근접 전투와 체력이 뛰어나다.", new TimeSpan(0, 15, 0), 1000, 18));
        unitData[(int)UnitId.Knight].AddLevelData(1, 8, 0, 0, 46, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(2, 10, 0, 0, 51, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(3, 12, 0, 0, 56, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(4, 14, 0, 0, 61, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(5, 16, 0, 0, 66, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(6, 18, 0, 0, 71, 0, 5, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(7, 20, 2, 0, 76, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(8, 22, 2, 0, 81, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(9, 24, 4, 0, 86, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(10, 26, 4, 0, 91, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(11, 28, 6, 1, 96, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(12, 30, 6, 1, 105, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(13, 32, 8, 1, 114, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(14, 34, 8, 2, 123, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(15, 36, 10, 2, 132, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(16, 38, 10, 2, 141, 0, 6, 1f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(17, 40, 12, 2, 150, 0, 6, 1.2f, 1, 0);
        unitData[(int)UnitId.Knight].AddLevelData(18, 42, 12, 3, 159, 0, 6, 1.2f, 1, 0);
    }

    public UnitBaseData GetUnitData(int Id)
    {
        for (int i = 0; i < unitData.Count; i++)
        {
            if ((int)unitData[i].ID == Id)
            {
                return unitData[i];
            }
        }

        return null;
    }
}

public class UnitBaseData
{
    UnitId Id;
    string name;
    string explanation;
    int maxLevel;
    TimeSpan createTime;
    int cost;
    List<UnitLevelData> unitData;

    public UnitId ID { get { return Id; } }
    public string Name { get { return name; } }
    public string Explanation { get { return explanation; } }
    public int MaxLevel { get { return maxLevel; } }
    public TimeSpan CreateTime { get { return createTime; } }
    public int Cost { get { return cost; } }
    public List<UnitLevelData> UnitData { get { return unitData; } }

    public UnitBaseData()
    {
        Id = UnitId.None;
        name = "";
        explanation = "";
        maxLevel = 0;
        createTime = new TimeSpan();
        cost = 0;
        unitData = new List<UnitLevelData>();
    }

    public UnitBaseData(UnitId newId, string newName, string newExplanation, TimeSpan newCreateTime, int newCost, int newMaxLevel)
    {
        Id = newId;
        name = newName;
        explanation = newExplanation;
        maxLevel = newMaxLevel;
        createTime = newCreateTime;
        cost = newCost;
        unitData = new List<UnitLevelData>();
    }

    public void AddLevelData(int newLevel, int newAttack, int newDefense, int newMagicDefense, int newHealth,
        int newMana, float newMoveSpeed, float newAttackSpeed, int newHealthRegeneration, int newManaRegeneration)
    {
        unitData.Add(new UnitLevelData(newLevel, newAttack, newDefense, newMagicDefense, newHealth, newMana, newMoveSpeed,
            newAttackSpeed, newHealthRegeneration, newManaRegeneration));
    }

    public UnitLevelData GetLevelData(int newLevel)
    {
        for (int i = 0; i < unitData.Count; i++)
        {
            if (unitData[i].Level == newLevel)
            {
                return unitData[i];
            }
        }
        return null;
    }
}

public class UnitLevelData
{
    int level;
    int attack;
    int defense;
    int magicDefense;
    int health;
    int mana;
    float moveSpeed;
    float attackSpeed;
    int healthRegeneration;
    int manaRegeneration;

    public int Level { get { return level; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int MagicDefense { get { return magicDefense; } }
    public int Health { get { return health; } }
    public int Mana { get { return mana; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public int HealthRegeneration { get { return healthRegeneration; } }
    public int ManaRegeneration { get { return manaRegeneration; } }

    public UnitLevelData()
    {
        level = 0;
        attack = 0;
        defense = 0;
        magicDefense = 0;
        health = 0;
        mana = 0;
        moveSpeed = 0;
        attackSpeed = 0;
        healthRegeneration = 0;
        manaRegeneration = 0;
    }

    public UnitLevelData(int newLevel, int newAttack, int newDefense, int newMagicDefense, int newHealth,
        int newMana, float newMoveSpeed, float newAttackSpeed, int newHealthRegeneration, int newManaRegeneration)
    {
        level = newLevel;
        attack = newAttack;
        defense = newDefense;
        magicDefense = newMagicDefense;
        health = newHealth;
        mana = newMana;
        moveSpeed = newMoveSpeed;
        attackSpeed = newAttackSpeed;
        healthRegeneration = newHealthRegeneration;
        manaRegeneration = newManaRegeneration;
    }
}