using UnityEngine;
using UnityEngine.UI;

public class WorldMapUIManager : MonoBehaviour
{
    DataManager dataManager;
    NetworkManager networkManager;
    LoadingManager loadingManager;
    BattleManager battleManager;

    //패널
    GameObject worldMapPanel;

    //이미지
    GameObject worldMapImage;
    public GameObject worldMapToolTip;
    GameObject worldMapExplanation;

    //버튼
    public Button worldMapQuitButton;

    public Button attackButton;
    public Button attackCancelButton;

    //텍스트
    public Text toolTipPlaceName;
    public Text placeName;
    public Text ownerName;
    public Text unitNum;
    public Text distance;

    public GameObject currentPlace;

    public void ManagerInitialize()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        loadingManager = GameObject.FindGameObjectWithTag("LoadingManager").GetComponent<LoadingManager>();
        battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
    }

    public void OnClickAddListener()
    {
        worldMapQuitButton.onClick.AddListener(() => OnClickWorldMapQuitButton());
        attackButton.onClick.AddListener(() => OnClickAttackButton());
        attackCancelButton.onClick.AddListener(() => OnClickAttackCancelButton());
    }

    public void SetUIObject()
    {
        worldMapPanel = GameObject.Find("WorldMapPanel");

        worldMapImage = GameObject.Find("WorldMapImage");
        worldMapToolTip = GameObject.Find("WorldMapToolTip");
        worldMapExplanation = GameObject.Find("WorldMapExplanation");

        worldMapQuitButton = GameObject.Find("WorldMapQuitButton").GetComponent<Button>();
        attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        attackCancelButton = GameObject.Find("AttackCancelButton").GetComponent<Button>();

        toolTipPlaceName = GameObject.Find("ToolTipPlaceName").GetComponent<Text>();
        placeName = GameObject.Find("PlaceName").GetComponent<Text>();
        ownerName = GameObject.Find("OwnerName").GetComponent<Text>();
        unitNum = GameObject.Find("UnitNum").GetComponent<Text>();
        distance = GameObject.Find("Distance").GetComponent<Text>();

        worldMapExplanation.SetActive(false);
        worldMapToolTip.SetActive(false);
        worldMapPanel.SetActive(false);
    }

    //월드맵 UI 셋팅
    public void SetWorldMap()
    {
        for (int i = 0; i < dataManager.WorldMap.Length; i++)
        {
            Place placeData = dataManager.WorldMap[i];
            GameObject place = InstantiatePlace(placeData.Type);

            place.name = placeData.ID;
            place.transform.SetParent(worldMapImage.transform);
            place.GetComponent<RectTransform>().localPosition = new Vector3((placeData.Position.X * 3) - 1500, (placeData.Position.Y * 3) - 1500, 0);
            place.GetComponent<RectTransform>().localScale = Vector3.one;
            place.GetComponent<WorldMapUI>().SetPlace(placeData.ID, placeData.Level, placeData.Position.X, placeData.Position.Y, placeData.Type);

            if (placeData.ID == dataManager.ID)
            {
                worldMapImage.GetComponent<RectTransform>().localPosition = new Vector3(1500 - (placeData.Position.X * 3), 1500 - (placeData.Position.Y * 3), 0);
            }
        }
    }

    //월드맵 프리팹 생성
    public GameObject InstantiatePlace(int placeType)
    {
        if (placeType == (int)PlaceType.Castle)
        {
            return (Instantiate(Resources.Load("Prefabs/WorldMap/Castle")) as GameObject);
        }
        else if (placeType == (int)PlaceType.Dungeon)
        {
            return (Instantiate(Resources.Load("Prefabs/WorldMap/Dungeon")) as GameObject);
        }
        else if (placeType == (int)PlaceType.Resources)
        {
            return (Instantiate(Resources.Load("Prefabs/WorldMap/Resources")) as GameObject);
        }

        return null;
    }

    //월드맵 장소 설명 셋팅
    public void SetPlaceExplanation(string newPlaceName, string newOwnerName, int level, int x, int y, int placeType)
    {
        worldMapExplanation.SetActive(true);
        placeName.text = newPlaceName;
        ownerName.text = newOwnerName;
        distance.text = new Vector2(dataManager.XPos - x, dataManager.YPos - y).magnitude.ToString("###0");

        int unit = 0;

        if (placeType == (int)PlaceType.Castle)
        {
            networkManager.DataRequest(ClientPacketId.EnemyUnitNumRequest);
        }
        else if (placeType == (int)PlaceType.Dungeon)
        {
            unit = level * 20;
            unitNum.text = "몇명 없음";
            SetUnitNum(unit);
        }
        else if (placeType == (int)PlaceType.Resources)
        {
            unit = level * 15;
            unitNum.text = "몇명 없음";
            SetUnitNum(unit);
        }        
    }

    //UnitNum 텍스트 설정
    public void SetUnitNum(int num)
    {
        if (num <= 20)
        {
            unitNum.text = "거의 없음";
        }
        else if (num <= 50)
        {
            unitNum.text = "몇명 없음";
        }
        else if (num <= 100)
        {
            unitNum.text = "조금 있음";
        }
        else if (num <= 200)
        {
            unitNum.text = "어느정도 있음";
        }
        else if (num <= 500)
        {
            unitNum.text = "많음";
        }
        else if (num <= 1000)
        {
            unitNum.text = "엄청 많음";
        }
        else
        {
            unitNum.text = "셀 수 없이 많음";
        }
    }

    //월드맵 x 버튼
    public void OnClickWorldMapQuitButton()
    {
        worldMapExplanation.SetActive(false);
        worldMapToolTip.SetActive(false);
        worldMapPanel.SetActive(false);
    }

    //공격 하기 버튼
    public void OnClickAttackButton()
    {
        battleManager.SetAttackPos(currentPlace.GetComponent<WorldMapUI>().X, currentPlace.GetComponent<WorldMapUI>().Y);
        StartCoroutine(loadingManager.LoadScene(GameManager.Scene.Wait, GameManager.Scene.Battle, 1.0f));
    }

    //공격 취소 버튼
    public void OnClickAttackCancelButton()
    {
        worldMapExplanation.SetActive(false);
    }
}