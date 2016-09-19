using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingUI : MonoBehaviour, IPointerDownHandler
{
    BuildingUIManager buildingUIManager;
    public BuildingId Id;

    void Start()
    {
        buildingUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().BuildingUIManager;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        buildingUIManager.SetBuildingState(Id);
    }
}