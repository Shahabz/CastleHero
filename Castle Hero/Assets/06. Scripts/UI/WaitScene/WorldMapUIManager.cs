using UnityEngine;
using UnityEngine.UI;

public class WorldMapUIManager : MonoBehaviour
{
    DataManager dataManager;
    NetworkManager networkManager;

    //패널
    GameObject worldMapPanel;

    //버튼
    Button worldMapQuitButton;

    //이미지
    GameObject worldMapImage;

    public void ManagerInitialize()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    public void OnClickAddListener()
    {
        worldMapQuitButton.onClick.AddListener(() => OnClickWorldMapQuitButton());
    }

    public void SetUIObject()
    {
        worldMapPanel = GameObject.Find("WorldMapPanel");

        worldMapQuitButton = GameObject.Find("WorldMapQuitButton").GetComponent<Button>();

        worldMapImage = GameObject.Find("WorldMapImage");
    }

    //월드맵 UI 셋팅
    public void SetWorldMap()
    {
        GameObject myCastle = Instantiate(Resources.Load("Prefabs/Castle")) as GameObject;
        myCastle.transform.SetParent(worldMapImage.transform);
        myCastle.GetComponent<RectTransform>().localPosition = new Vector3((dataManager.XPos * 3) - 1500, (dataManager.YPos * 3) - 1500, 0);
        myCastle.GetComponent<RectTransform>().localScale = Vector3.one;
        myCastle.name = "MyCastle";
    }

    public void OnClickWorldMapQuitButton()
    {
        worldMapPanel.SetActive(false);
    }
}
