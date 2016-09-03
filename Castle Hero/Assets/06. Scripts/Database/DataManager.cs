using System;
using System.Collections.Generic;
using UnityEngine;

class DataManager : MonoBehaviour
{
    HeroDatabase heroDatabase;

    string Id;
    int heroId;
    float attackRange;
    float colliderSize;

    int level;
    int currentExperience;
    int maxExperience;
    int attack;
    int defense;
    int magicDefense;
    int currentHealth;
    int maxHealth;
    int currentMana;
    int maxMana;
    float moveSpeed;
    float attackSpeed;
    float rotateSpeed;
    int healthRegeneration;
    int manaRegeneration;

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

    public DataManager()
    {
        heroDatabase = new HeroDatabase();
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
    }
}
