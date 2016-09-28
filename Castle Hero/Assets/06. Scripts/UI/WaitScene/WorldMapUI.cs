using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMapUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    WorldMapUIManager worldMapUIManager;

    string placeName;
    int level;
    short x;
    short y;
    int placeType;

    public short X { get { return x; } }
    public short Y { get { return y; } }

    void Start()
    {
        worldMapUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().WorldMapUIManager;
    }

    public void SetPlace(string newName, int newLevel, short newX, short newY, int newPlaceType)
    {
        placeName = newName;
        level = newLevel;
        x = newX;
        y = newY;
        placeType = newPlaceType;
    }

    //월드맵 툴팁 셋팅
    public void SetWorldMapToolTip(string placeName, bool isOn)
    {
        if (isOn)
        {
            worldMapUIManager.worldMapToolTip.SetActive(true);
            worldMapUIManager.worldMapToolTip.GetComponent<RectTransform>().position = transform.position + (Vector3.up * 50);
            if (placeType == (int)PlaceType.Castle)
            {
                worldMapUIManager.toolTipPlaceName.text = placeName + "의 성";
            }
            else if (placeType == (int)PlaceType.Dungeon)
            {
                worldMapUIManager.toolTipPlaceName.text = "던전";
            }
            else if (placeType == (int)PlaceType.Resources)
            {
                worldMapUIManager.toolTipPlaceName.text = "자원지";
            }
        }
        else
        {
            worldMapUIManager.worldMapToolTip.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventdata)
    {        
        SetWorldMapToolTip(placeName, true);
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        SetWorldMapToolTip(placeName, false);
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        worldMapUIManager.currentPlace = gameObject;
        worldMapUIManager.SetPlaceExplanation(placeName + "의 성", placeName, level, x, y, placeType);
    }
}
