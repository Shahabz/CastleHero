using System.Collections.Generic;

public enum UnitID
{
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
    List<HeroBaseData> HeroData;

    public HeroDatabase()
    {
        HeroData = new List<HeroBaseData>();

        HeroData.Add(new HeroBaseData((int)UnitID.Unity, 1.0f, 1.0f));
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