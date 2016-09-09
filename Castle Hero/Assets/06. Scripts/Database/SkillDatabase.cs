using UnityEngine;
using System.Collections.Generic;

public class SkillDatabase
{
    private static SkillDatabase instance;

    public static SkillDatabase Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SkillDatabase();
            }
            return instance;
        }
    }

    public Dictionary<int, Skill> database;
}

public class Skill
{
    public enum SkillType
    {
        None = 0,
        Active,
        Passive,
        Aura,
    }

    protected int Id;
    protected int manaCost;
    protected Transform target;
    protected SkillType type;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}

public class ActiveSkill : Skill
{
    float readyTime;
    float afterActionTime;
    float castRange;
    int damage;

    
}