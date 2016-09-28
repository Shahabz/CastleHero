using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private const int spawnLength = 10;
    DataManager dataManager;

    GameObject[] homeSpawnPoint = new GameObject[spawnLength];
    GameObject[] awaySpawnPoint = new GameObject[spawnLength];

    Unit[] homeUnitData;
    Unit[] awayUnitData;

    GameObject[] homeUnit = new GameObject[spawnLength];
    GameObject[] awayUnit = new GameObject[spawnLength];

    short attackXPos;
    short attackYPos;

    void Awake()
    {
        tag = "BattleManager";
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ManagerInitialize()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }

    public void SetAttackPos(short x, short y)
    {
        attackXPos = x;
        attackYPos = y;
    }

    public Position GetAttackPos()
    {
        return new Position(attackXPos, attackYPos);
    }

    public void SetHomeUnit(Unit[] unit)
    {
        homeUnitData = unit;
    }

    public void SetAwayUnit(Unit[] unit)
    {
        awayUnitData = unit;
    }

    public void SetSpawnPoint()
    {
        homeSpawnPoint = GameObject.FindGameObjectsWithTag("HomeSpawnPoint");
        awaySpawnPoint = GameObject.FindGameObjectsWithTag("AwaySpawnPoint");
    }

    public void SetBattleStage()
    {
        SetHomeUnit(dataManager.Unit);
        StartCoroutine(SpawnUnit(homeUnitData, homeUnit, homeSpawnPoint));
        StartCoroutine(SpawnUnit(awayUnitData, awayUnit, awaySpawnPoint));
    }

    public IEnumerator SpawnUnit(Unit[] unitData, GameObject[] unit, GameObject[] spawnPoint)
    {
        //생성하기 전 유닛의 총 숫자를 체크
        int unitNum = CountUnitData(unitData);

        int unitSpawnIndex = 0;
        int spawnIndex = 0;

        //총 숫자가 0이 아니면
        while (unitNum != 0)
        {
            //시작할 때 체크하고
            unitNum = CountUnitData(unitData);

            //소환할 유닛이 하나라도 있는지
            if (unitNum != 0)
            {
                int unitIndex = FindEmptySlot(unit);

                //소환 최대 개수보다 적은지
                if (unitIndex != -1)
                {
                    if(unitData[unitSpawnIndex].num > 0)
                    {
                        //소환
                        Unit spawnUnitData = unitData[unitSpawnIndex];

                        unit[unitIndex] = Spawn(spawnUnitData, spawnPoint[spawnIndex]);

                        if (spawnPoint[spawnIndex].tag == "HomeSpawnPoint")
                        {
                            unit[unitIndex].tag = "HomeUnit";
                        }
                        else if (spawnPoint[spawnIndex].tag == "AwaySpawnPoint")
                        {
                            unit[unitIndex].tag = "AwayUnit";
                        }

                        unit[unitIndex].GetComponent<UnitAI>().InitializeUnit(UnitDatabase.Instance.unitData[spawnUnitData.Id].Name, spawnUnitData.Id, dataManager.Upgrade[spawnUnitData.Id]);

                        unitData[unitSpawnIndex].num--;
                    }

                    if (++unitSpawnIndex >= unitData.Length)
                        unitSpawnIndex = 0;

                    if (++spawnIndex >= spawnPoint.Length)
                        spawnIndex = 0;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public int FindEmptySlot(GameObject[] unit)
    {
        for (int i = 0; i < unit.Length; i++)
        {
            if(unit[i] == null)
            {
                return i;
            }
        }

        return -1;
    }

    public int CountUnitData(Unit[] unitData)
    {
        int count = 0;

        for(int i =0; i< unitData.Length; i++)
        {
            count += unitData[i].num;
        }

        return count;
    }

    public GameObject Spawn(Unit unit, GameObject spawnPoint)
    {
        string unitName = UnitDatabase.Instance.unitData[unit.Id].Name;
        GameObject newUnit = Instantiate(Resources.Load("Prefabs/Unit/" + unitName) as GameObject);
        newUnit.transform.position = spawnPoint.transform.position;
        return newUnit;
    }
}