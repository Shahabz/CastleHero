using UnityEngine;

public class BattleManager : MonoBehaviour
{
    DataManager dataManager;

    GameObject[] homeSpawnPoint;
    GameObject[] awaySpawnPoint;

    Unit[] homeUnit;
    Unit[] awayUnit;

    void Awake()
    {
        tag = "BattleManager";
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ManagerInitialize()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }

    public void SetHomeUnit(Unit[] unit)
    {
        homeUnit = unit;
    }

    public void SetAwayUnit(Unit[] unit)
    {
        awayUnit = unit;
    }

    public void SetSpawnPoint()
    {
        homeSpawnPoint = GameObject.FindGameObjectsWithTag("HomeSpawnPoint");
        homeSpawnPoint = GameObject.FindGameObjectsWithTag("AwaySpawnPoint");
    }

    public void SetBattleStage()
    {
        SetHomeUnit(dataManager.Unit);
    }
}