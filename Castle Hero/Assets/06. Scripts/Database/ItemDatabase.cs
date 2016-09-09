using System.Collections.Generic;

public enum ItemType
{
    None = 0,
    Weapon,
    Helmet,
    Armor,
    Gloves,
    Shoes,
    Ring,
    Necklace,
}

public class ItemDatabase
{
    private static ItemDatabase instance;

    public static ItemDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemDatabase();
            }

            return instance;
        }        
    }

    public Dictionary<int, Item> database;

    public void InitializeItemDatabase()
    {
        database = new Dictionary<int, Item>();

        database.Add(0, new Item(0, ItemType.None, "빈칸", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        database.Add(1, new Item(1, ItemType.Weapon, "작은검", 100, 2, 0, 0, 0, 0, 0, 0, 0, 0));
        database.Add(2, new Item(2, ItemType.Weapon, "검", 200, 3, 0, 0, 0, 0, 0, 0, 0, 0));
        database.Add(3, new Item(3, ItemType.Weapon, "대검", 300, 5, 0, 0, 0, 0, 0, 0, 0, 0));
        database.Add(4, new Item(4, ItemType.Weapon, "화염의 검", 500, 8, 0, 0, 0, 0, 0, 0, 0, 0));
        database.Add(5, new Item(5, ItemType.Weapon, "드래곤뼈 검", 1000, 11, 0, 0, 0, 0, 0, 0, 0, 0));
        database.Add(6, new Item(6, ItemType.Helmet, "가죽 모자", 100, 0, 0, 0, 3, 0, 0, 0, 0, 0));
        database.Add(7, new Item(7, ItemType.Helmet, "녹슨 투구", 200, 0, 0, 0, 6, 0, 0, 0, 0, 0));
        database.Add(8, new Item(8, ItemType.Helmet, "강철 투구", 300, 0, 0, 0, 10, 0, 0, 0, 0, 0));
        database.Add(9, new Item(9, ItemType.Helmet, "미스릴 투구", 500, 0, 0, 0, 15, 0, 0, 0, 0, 0));
        database.Add(10, new Item(10, ItemType.Helmet, "드래곤비늘 투구", 1000, 0, 0, 0, 20, 0, 0, 0, 0, 0));
        database.Add(11, new Item(11, ItemType.Armor, "가죽 갑옷", 100, 0, 1, 0, 0, 0, 0, 0, 0, 0));
        database.Add(12, new Item(12, ItemType.Armor, "녹슨 갑옷", 200, 0, 2, 0, 0, 0, 0, 0, 0, 0));
        database.Add(13, new Item(13, ItemType.Armor, "체인 갑옷", 300, 0, 3, 0, 0, 0, 0, 0, 0, 0));
        database.Add(14, new Item(14, ItemType.Armor, "미스릴 갑옷", 500, 0, 4, 0, 0, 0, 0, 0, 0, 0));
        database.Add(15, new Item(15, ItemType.Armor, "드래곤비늘 갑옷", 1000, 0, 5, 0, 0, 0, 0, 0, 0, 0));
        database.Add(16, new Item(16, ItemType.Gloves, "천 장갑", 100, 0, 0, 0, 0, 0, 0, 0.1f, 0, 0));
        database.Add(17, new Item(17, ItemType.Gloves, "가죽 장갑", 200, 0, 0, 0, 0, 0, 0, 0.15f, 0, 0));
        database.Add(18, new Item(18, ItemType.Gloves, "건틀릿", 300, 0, 0, 0, 0, 0, 0, 0.2f, 0, 0));
        database.Add(19, new Item(19, ItemType.Gloves, "강철 건틀릿", 500, 0, 0, 0, 0, 0, 0, 0.25f, 0, 0));
        database.Add(20, new Item(20, ItemType.Gloves, "드래곤비늘 건틀릿", 1000, 0, 0, 0, 0, 0, 0, 0.3f, 0, 0));
        database.Add(21, new Item(21, ItemType.Shoes, "천 신발", 100, 0, 0, 0, 0, 0, 0.1f, 0, 0, 0));
        database.Add(22, new Item(22, ItemType.Shoes, "가죽 신발", 200, 0, 0, 0, 0, 0, 0.15f, 0, 0, 0));
        database.Add(23, new Item(23, ItemType.Shoes, "털 신발", 300, 0, 0, 0, 0, 0, 0.2f, 0, 0, 0));
        database.Add(24, new Item(24, ItemType.Shoes, "강철 신발", 500, 0, 0, 0, 0, 0, 0.25f, 0, 0, 0));
        database.Add(25, new Item(25, ItemType.Shoes, "드래곤비늘 신발", 1000, 0, 0, 0, 0, 0, 0.3f, 0, 0, 0));
        database.Add(26, new Item(26, ItemType.Ring, "돌 반지", 100, 0, 0, 1, 0, 0, 0, 0, 0, 0));
        database.Add(27, new Item(27, ItemType.Ring, "은 반지", 200, 0, 0, 2, 0, 0, 0, 0, 0, 0));
        database.Add(28, new Item(28, ItemType.Ring, "금 반지", 300, 0, 0, 3, 0, 0, 0, 0, 0, 0));
        database.Add(29, new Item(29, ItemType.Ring, "다이아몬드 반지", 500, 0, 0, 4, 0, 0, 0, 0, 0, 0));
        database.Add(30, new Item(30, ItemType.Ring, "마법 깃든 반지", 1000, 0, 0, 5, 0, 0, 0, 0, 0, 0));
        database.Add(31, new Item(31, ItemType.Necklace, "돌 목걸이", 100, 0, 0, 0, 0, 0, 0, 0, 1, 0));
        database.Add(32, new Item(32, ItemType.Necklace, "은 목걸이", 200, 0, 0, 0, 0, 0, 0, 0, 0, 1));
        database.Add(33, new Item(33, ItemType.Necklace, "금 목걸이", 300, 0, 0, 0, 0, 0, 0, 0, 1, 2));
        database.Add(34, new Item(34, ItemType.Necklace, "다이아몬드 목걸이", 500, 0, 0, 0, 0, 0, 0, 0, 1, 3));
        database.Add(35, new Item(35, ItemType.Necklace, "마법 깃든 목걸이", 1000, 0, 0, 0, 0, 0, 0, 0, 2, 3));
    }
}

public class Item
{
    int Id;
    ItemType type;
    string name;
    int gold;
    int attack;
    int defense;
    int magicDefense;
    int health;
    int mana;
    float moveSpeed;
    float attackSpeed;
    int healthRegeneration;
    int manaRegeneration;

    public int ID { get { return Id; } }
    public ItemType Type { get { return type; } }
    public string Name { get { return name; } }
    public int Gold { get { return gold; } }
    public int Defense { get { return defense; } }
    public int MagicDefense { get { return magicDefense; } }
    public int Health { get { return health; } }
    public int Mana { get { return mana; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public int HealthRegeneration { get { return healthRegeneration; } }
    public int ManaRegeneration { get { return manaRegeneration; } }

    public Item()
    {
        Id = 0;
        type = ItemType.None;
        gold = 0;
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

    public Item(int newId, ItemType newType, string newName, int newGold, int newAttack, int newDefense, int newMagicDefense, int newHealth, int newMana,
        float newMoveSpeed, float newAttackSpeed, int newHealthRegeneration, int newManaRegeneration)
    {
        Id = newId;
        type = newType;
        name = newName;
        gold = newGold;
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
