using UnityEngine;
using UnityEngine.UI;

public class BuildingUIManager
{
    DataManager dataManager;
    NetworkManager networkManager;

    //패널
    public GameObject buildingPanel;
    public GameObject buildPanel;

    //이미지
    public GameObject buildingImage;
    public GameObject buildingState;

    //버튼
    public Button buildButton;
    public Button buildYesButton;
    public Button buildNoButton;

    //텍스트
    public Text castleLevel;
    public Text mineLevel;
    public Text storageLevel;
    public Text barracksLevel;
    public Text wallLevel;
    public Text laboratoryLevel;

    public Text buildingName;
    public Text buildingExplanation;
    public Text buildingLevel;
    public Text buildingLevelExplanation;
    public Text buildCost;
    public Text buildingTime;

    public Text currentBuildingLevel;
    public Text nextBuildingLevel;
    public Text currentBuildingExplanation;
    public Text nextBuildingExplanation;

    BuildingId currentBuilding;
    public BuildingId CurrentBuilding { get { return currentBuilding; } set { currentBuilding = value; } }
    
    public void ManagerInitialize()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    public void OnClickAddListener()
    {
        buildButton.onClick.AddListener(() => OnClickBuildButton());
        buildYesButton.onClick.AddListener(() => OnClickBuildYesButton());
        buildNoButton.onClick.AddListener(() => OnClickBuildNoButton());
    }

    public void SetUIObject()
    {
        buildingPanel = GameObject.Find("BuildingPanel");
        buildPanel = GameObject.Find("BuildPanel");

        buildingImage = GameObject.Find("BuildingImage");
        buildingState = GameObject.Find("BuildingState");

        buildButton = GameObject.Find("BuildButton").GetComponent<Button>();
        buildYesButton = GameObject.Find("BuildYesButton").GetComponent<Button>();
        buildNoButton = GameObject.Find("BuildNoButton").GetComponent<Button>();

        castleLevel = GameObject.Find("CastleLevel").GetComponent<Text>();
        mineLevel = GameObject.Find("MineLevel").GetComponent<Text>();
        storageLevel = GameObject.Find("StorageLevel").GetComponent<Text>();
        barracksLevel = GameObject.Find("BarracksLevel").GetComponent<Text>();
        wallLevel = GameObject.Find("WallLevel").GetComponent<Text>();
        laboratoryLevel = GameObject.Find("LaboratoryLevel").GetComponent<Text>();
        buildingName = GameObject.Find("BuildingName").GetComponent<Text>();
        buildingExplanation = GameObject.Find("BuildingExplanation").GetComponent<Text>();
        buildingLevel = GameObject.Find("BuildingLevel").GetComponent<Text>();
        buildingLevelExplanation = GameObject.Find("BuildingLevelExplanation").GetComponent<Text>();
        buildCost = GameObject.Find("BuildCost").GetComponent<Text>();
        buildingTime = GameObject.Find("BuildingTime").GetComponent<Text>();
        currentBuildingLevel = GameObject.Find("CurrentBuildingLevel").GetComponent<Text>();
        nextBuildingLevel = GameObject.Find("NextBuildingLevel").GetComponent<Text>();
        currentBuildingExplanation = GameObject.Find("CurrentBuildingExplanation").GetComponent<Text>();
        nextBuildingExplanation = GameObject.Find("NextBuildingExplanation").GetComponent<Text>();

        buildPanel.SetActive(false);
        buildingState.SetActive(false);
        buildingPanel.SetActive(false);
    }

    //건물 UI 셋팅
    public void SetBuilding()
    {
        castleLevel.text = dataManager.Building[(int)BuildingId.Castle].ToString();
        mineLevel.text = dataManager.Building[(int)BuildingId.Mine].ToString();
        storageLevel.text = dataManager.Building[(int)BuildingId.Storage].ToString();
        barracksLevel.text = dataManager.Building[(int)BuildingId.Barracks].ToString();
        wallLevel.text = dataManager.Building[(int)BuildingId.Wall].ToString();
        laboratoryLevel.text = dataManager.Building[(int)BuildingId.Laboratory].ToString();
        currentBuilding = BuildingId.None;
    }

    //건물 설명 창
    public void SetBuildingState(BuildingId Id)
    {
        buildingState.SetActive(true);
        Building building = BuildingDatabase.Instance.GetBuildingData((int)Id);
        buildingName.text = building.Name;
        buildingExplanation.text = building.Explanation;

        buildingLevel.text = dataManager.Building[(int)Id].ToString();
        buildingLevelExplanation.text = BuildingDatabase.Instance.buildingData[(int)Id].BuildingData[dataManager.Building[(int)Id]].Explanation;
        buildingTime.text = BuildingDatabase.Instance.buildingData[(int)Id].BuildingData[dataManager.Building[(int)Id]].BuildTime.ToString();
        buildCost.text = BuildingDatabase.Instance.buildingData[(int)Id].BuildingData[dataManager.Building[(int)Id]].Cost.ToString();
        currentBuilding = Id;
    }

    //건설 창
    public void SetBuildPanel()
    {
        currentBuildingLevel.text = dataManager.Building[(int)currentBuilding].ToString();
        nextBuildingLevel.text = (dataManager.Building[(int)currentBuilding] + 1).ToString();
        currentBuildingExplanation.text = BuildingDatabase.Instance.buildingData[(int)currentBuilding].BuildingData[dataManager.Building[(int)currentBuilding]].Explanation;
        nextBuildingExplanation.text = BuildingDatabase.Instance.buildingData[(int)currentBuilding].BuildingData[dataManager.Building[(int)currentBuilding] + 1].Explanation;
    }

    //건설 버튼
    public void OnClickBuildButton()
    {
        if (dataManager.BuildBuilding == (int) BuildingId.None)
        {
            if (dataManager.Building[(int)currentBuilding] < BuildingDatabase.Instance.buildingData[(int)currentBuilding].MaxLevel)
            {
                buildPanel.SetActive(true);
                SetBuildPanel();
            }
            else
            {
                Debug.Log("최대레벨입니다.");
            }
        }
        else
        {
            Debug.Log("건설중입니다.");
        }
    }

    //건설 취소 버튼
    public void OnClickBuildNoButton()
    {
        buildPanel.SetActive(false);
    }

    //건설 확인 버튼
    public void OnClickBuildYesButton()
    {
        networkManager.BuildBuilding(currentBuilding);
        networkManager.DataRequest(ClientPacketId.BuildDataRequest);
        buildPanel.SetActive(false);
    }
}
