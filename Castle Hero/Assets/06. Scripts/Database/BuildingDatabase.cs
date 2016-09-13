using System;
using System.Collections.Generic;

public enum BuildingId
{
    None = 0,
    Castle,
    Mine,
    Storage,
    Barracks,
    Wall,
    Laboratory,
}

class BuildingDatabase
{
    private static BuildingDatabase instance;

    public static BuildingDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuildingDatabase();
            }

            return instance;
        }
    }

    public List<Building> buildingData;

    public void InitializeBuildingDatabase()
    {
        buildingData = new List<Building>();

        buildingData.Add(new Building());

        buildingData.Add(new Building(BuildingId.Castle, "Castle", "영웅이 거주하는 성이다. 다른 건물들을 업그레이드 할 수 있다.", 10));
        buildingData[(int)BuildingId.Castle].AddLevelData(1, new TimeSpan(0, 3, 0), 500, "광산, 저장소, 병영, 성벽, 연구소 1레벨 건설 가능");
        buildingData[(int)BuildingId.Castle].AddLevelData(2, new TimeSpan(0, 15, 0), 2000, "광산, 저장소, 병영, 성벽, 연구소 2레벨 건설 가능");
        buildingData[(int)BuildingId.Castle].AddLevelData(3, new TimeSpan(0, 30, 0), 8000, "광산, 저장소, 연구소 3레벨 건설 가능");
        buildingData[(int)BuildingId.Castle].AddLevelData(4, new TimeSpan(1, 0, 0), 32000, "광산, 저장소, 병영, 성벽, 연구소 3레벨 건설 가능");
        buildingData[(int)BuildingId.Castle].AddLevelData(5, new TimeSpan(2, 0, 0), 64000, "광산, 저장소, 연구소 5레벨 건설 가능");
        buildingData[(int)BuildingId.Castle].AddLevelData(6, new TimeSpan(3, 0, 0), 128000, "광산, 저장소, 연구소 6레벨 건설 가능. 병영, 성벽 4레벨 건성 가능.");
        buildingData[(int)BuildingId.Castle].AddLevelData(7, new TimeSpan(6, 0, 0), 256000, "광산, 저장소, 연구소 7레벨 건설 가능");
        buildingData[(int)BuildingId.Castle].AddLevelData(8, new TimeSpan(12, 0, 0), 512000, "광산, 저장소, 연구소 8레벨 건설 가능. 병영, 성벽 5레벨 건성 가능.");
        buildingData[(int)BuildingId.Castle].AddLevelData(9, new TimeSpan(24, 0, 0), 1024000, "광산, 저장소, 연구소 9레벨 건설 가능");
        buildingData[(int)BuildingId.Castle].AddLevelData(10, new TimeSpan(72, 0, 0), 2048000, "광산, 저장소, 연구소 10레벨 건설 가능. 병영, 성벽 6레벨 건성 가능.");

        buildingData.Add(new Building(BuildingId.Mine, "Mine", "건물의 업그레이드 밑 영웅의 장비를 구입할 수 있는 금을 얻을 수 있는 광산이다.", 10));
        buildingData[(int)BuildingId.Mine].AddLevelData(1, new TimeSpan(0, 1, 0), 100, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(2,new TimeSpan(0, 5, 0), 400, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(3, new TimeSpan(0, 10, 0), 1600, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(4, new TimeSpan(0, 20, 0), 6400, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(5, new TimeSpan(0, 30, 0), 12800, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(6, new TimeSpan(1, 0, 0), 25600, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(7, new TimeSpan(2, 0, 0), 51200, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(8, new TimeSpan(4, 0, 0), 102400, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(9, new TimeSpan(8, 0, 0), 204800, "자원 획득량 1 증가");
        buildingData[(int)BuildingId.Mine].AddLevelData(10, new TimeSpan(24, 0, 0), 409600, "자원 획득량 1 증가");

        buildingData.Add(new Building(BuildingId.Storage, "Storage", "광산에서 캐낸 금을 저장하는 저장소이다.", 10));
        buildingData[(int)BuildingId.Storage].AddLevelData(1, new TimeSpan(0, 2, 0), 150, "자원 저장량 4배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(2, new TimeSpan(0, 10, 0), 600, "자원 저장량 4배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(3, new TimeSpan(0, 20, 0), 2400, "자원 저장량 4배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(4, new TimeSpan(0, 40, 0), 9600, "자원 저장량 2배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(5, new TimeSpan(1, 20, 0), 19200, "자원 저장량 2배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(6, new TimeSpan(2, 0, 0), 38400, "자원 저장량 2배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(7, new TimeSpan(4, 0, 0), 76800, "자원 저장량 2배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(8, new TimeSpan(8, 0, 0), 153600, "자원 저장량 2배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(9, new TimeSpan(16, 0, 0), 307200, "자원 저장량 2배 증가");
        buildingData[(int)BuildingId.Storage].AddLevelData(10, new TimeSpan(48, 0, 0), 614400, "자원 저장량 2배 증가");

        buildingData.Add(new Building(BuildingId.Barracks, "Barracks", "병력을 생산할 수 있는 병영이다.", 5));
        buildingData[(int)BuildingId.Barracks].AddLevelData(1, new TimeSpan(0, 2, 0), 150, "검투사 모집 가능");
        buildingData[(int)BuildingId.Barracks].AddLevelData(2, new TimeSpan(0, 20, 0), 1200, "궁수 모집 가능");
        buildingData[(int)BuildingId.Barracks].AddLevelData(3, new TimeSpan(1, 0, 0), 9600, "팔라딘 모집 가능");
        buildingData[(int)BuildingId.Barracks].AddLevelData(4, new TimeSpan(3, 0, 0), 76800, "마법사 모집 가능");
        buildingData[(int)BuildingId.Barracks].AddLevelData(5, new TimeSpan(12, 0, 0), 614400, "기사 모집 가능");

        buildingData.Add(new Building(BuildingId.Wall, "Wall", "적의 공격을 효율적으로 방어할 수 있는 성벽이다.", 5));
        buildingData[(int)BuildingId.Wall].AddLevelData(1, new TimeSpan(0, 2, 0), 150, "바리케이트 설치 가능");
        buildingData[(int)BuildingId.Wall].AddLevelData(2, new TimeSpan(0, 20, 0), 1200, "성벽에 궁수 추가 가능");
        buildingData[(int)BuildingId.Wall].AddLevelData(3, new TimeSpan(1, 0, 0), 9600, "성벽에 대형 석궁 설치 가능");
        buildingData[(int)BuildingId.Wall].AddLevelData(4, new TimeSpan(3, 0, 0), 76800, "지휘북 설치 가능");
        buildingData[(int)BuildingId.Wall].AddLevelData(5, new TimeSpan(12, 0, 0), 614400, "지휘깃발 설치 가능");

        buildingData.Add(new Building(BuildingId.Laboratory, "Laboratory", "병력을 더욱 강하게 만들 수 있는 연구소이다.", 10));
        buildingData[(int)BuildingId.Laboratory].AddLevelData(1, new TimeSpan(0, 2, 0), 300, "병력 1레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(2, new TimeSpan(0, 10, 0), 6120, "병력 2레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(3, new TimeSpan(0, 20, 0), 4800, "병력 3레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(4, new TimeSpan(0, 40, 0), 19200, "병력 4레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(5, new TimeSpan(1, 20, 0), 38400, "병력 5레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(6, new TimeSpan(4, 0, 0), 76800, "병력 6레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(7, new TimeSpan(8, 0, 0), 153600, "병력 7레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(8, new TimeSpan(16, 0, 0), 307200, "병력 8레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(9, new TimeSpan(48, 0, 0), 614400, "병력 9레벨 강화 가능");
        buildingData[(int)BuildingId.Laboratory].AddLevelData(10, new TimeSpan(2, 0, 0), 38400, "병력 10레벨 강화 가능");
    }
}

public class Building
{
    BuildingId Id;
    string name;
    string explanation;
    int maxLevel;
    List<BuildingLevelData> buildingData;
    
    public BuildingId ID { get { return Id; } }
    public string Name { get { return name; } }
    public string Explanation { get { return explanation; } }
    public int MaxLevel { get { return maxLevel; } }
    public List<BuildingLevelData> BuildingData { get { return buildingData; } }

    public Building()
    {
        Id = BuildingId.None;
        name = "";
        explanation = "";
        maxLevel = 0;
        buildingData = new List<BuildingLevelData>();
    }

    public Building(BuildingId newId, string newName, string newExplanation, int newMaxLevel)
    {
        Id = newId;
        name = newName;
        explanation = newExplanation;
        maxLevel = newMaxLevel;
        buildingData = new List<BuildingLevelData>();
    }

    public void AddLevelData(int newLevel, TimeSpan newBuildTime, int newCost, string newNextLevel)
    {
        buildingData.Add(new BuildingLevelData(newLevel, newBuildTime, newCost, newNextLevel));
    }
}

public class BuildingLevelData
{
    int level;
    TimeSpan buildTime;
    int cost;
    string nextLevel;

    public int Level { get { return level; } }
    public TimeSpan BuildTime { get { return buildTime; } }
    public int Cost { get { return cost; } }
    public string NextLevel { get { return nextLevel; } }

    public BuildingLevelData()
    {
        level = 0;
        buildTime = new TimeSpan();
        cost = 0;
        nextLevel = "";
    }

    public BuildingLevelData(int newLevel, TimeSpan newBuildTime, int newCost, string newNextLevel)
    {
        level = newLevel;
        buildTime = newBuildTime;
        cost = newCost;
        nextLevel = newNextLevel;
    }
}