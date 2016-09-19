using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager
{
    DataManager dataManager;

    public GameObject unitImage;
    public GameObject unitState;

    public Text gladiatorLevel;
    public Text archerLevel;
    public Text paladinLevel;
    public Text mageLevel;
    public Text knightLevel;
    public Text unitName;
    public Text unitExplanation;
    public Text unitLevel;
    public Text unitStatus;
    public Text unitCreateCost;
    public Text unitCreateName;
    public Text unitCreateNum;
    public Text unitCreateTime;

    public void ManagerInitialize()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }

    public void SetUIObject()
    {
        unitImage = GameObject.Find("UnitImage");
        unitState = GameObject.Find("UnitState");

        gladiatorLevel = GameObject.Find("GladiatorLevel").GetComponent<Text>();
        archerLevel = GameObject.Find("ArcherLevel").GetComponent<Text>();
        paladinLevel = GameObject.Find("PaladinLevel").GetComponent<Text>();
        mageLevel = GameObject.Find("MageLevel").GetComponent<Text>();
        knightLevel = GameObject.Find("KnightLevel").GetComponent<Text>();
        unitName = GameObject.Find("UnitName").GetComponent<Text>();
        unitExplanation = GameObject.Find("UnitExplanation").GetComponent<Text>();
        unitLevel = GameObject.Find("UnitCurrentLevel").GetComponent<Text>();
        unitStatus = GameObject.Find("UnitStatus").GetComponent<Text>();
        unitCreateCost = GameObject.Find("UnitCreateCost").GetComponent<Text>();
        unitCreateTime = GameObject.Find("UnitCreateTime").GetComponent<Text>();

        unitState.SetActive(false);
    }

    public void SetUnitLevel()
    {
        gladiatorLevel.text = dataManager.Upgrade[(int)UnitId.Gladiator].ToString();
        archerLevel.text = dataManager.Upgrade[(int)UnitId.Archer].ToString();
        paladinLevel.text = dataManager.Upgrade[(int)UnitId.Paladin].ToString();
        mageLevel.text = dataManager.Upgrade[(int)UnitId.Mage].ToString();
        knightLevel.text = dataManager.Upgrade[(int)UnitId.Knight].ToString();
    }

    public void SetUnitData(UnitId newId)
    {
        unitState.SetActive(true);
        UnitBaseData unitData = UnitDatabase.Instance.unitData[(int)newId];
        int level = dataManager.Upgrade[(int)newId];
        UnitLevelData unitLevelData = unitData.GetLevelData(level);
        
        unitName.text = unitData.Name;
        unitExplanation.text = unitData.Explanation;
        unitLevel.text = level.ToString();

        unitStatus.text = "공격력 : " + unitLevelData.Attack.ToString() + " 방어력 : " + unitLevelData.Defense.ToString() + " 마법방어력 : " + unitLevelData.MagicDefense.ToString()
                + " 체력 : " + unitLevelData.Health.ToString() + " 이동속도 : " + unitLevelData.MoveSpeed.ToString() + " 공격속도 : " + unitLevelData.AttackSpeed.ToString();
        unitCreateCost.text = unitData.Cost.ToString();
        unitCreateTime.text = unitData.CreateTime.Hours.ToString("00") + ":" + unitData.CreateTime.Minutes.ToString("00") + ":" + unitData.CreateTime.Seconds.ToString("00");
    }
}
