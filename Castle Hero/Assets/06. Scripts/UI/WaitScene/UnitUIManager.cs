using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager
{
    DataManager dataManager;
    NetworkManager networkManager;

    GameObject unitPanel;
    public GameObject unitImage;
    public GameObject unitState;

    public Button unitCreateButton;

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
    public Text unitCreateTime;
    public Text unitCreateNum;

    UnitId currentUnit;
    public UnitId CurrentUnit { get { return currentUnit; } set { currentUnit = value; } }

    public void ManagerInitialize()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    public void OnClickAddListener()
    {
        unitCreateButton.onClick.AddListener(() => OnClickUnitCreateButton());
    }

    public void SetUIObject()
    {
        unitPanel = GameObject.Find("UnitPanel");
        unitImage = GameObject.Find("UnitImage");
        unitState = GameObject.Find("UnitState");

        unitCreateButton = GameObject.Find("UnitCreateButton").GetComponent<Button>();

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
        unitCreateNum = GameObject.Find("UnitCreateNum").GetComponent<Text>();

        unitState.SetActive(false);
        unitPanel.SetActive(false);
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
        currentUnit = newId;
    }

    public void OnClickUnitCreateButton()
    {
        int unitNum = int.Parse(unitCreateNum.text);

        if (unitNum <= 0)
        {
            Debug.Log("1이상의 숫자를 입력하세요");
        }
        else if(unitNum > 99)
        {
            Debug.Log("99이하의 숫자를 입력하세요");
        }
        else
        {
            Debug.Log("생성 유닛 : " + currentUnit + "생성숫자 : "  + unitNum);
            networkManager.UnitCreate((int)currentUnit, unitNum);
            networkManager.DataRequest(ClientPacketId.UnitCreateDataRequest);

            unitNum = 0;
        }
    }
}
