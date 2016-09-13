using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingUI : MonoBehaviour, IPointerDownHandler
{
    UIManager uiManager;
    public BuildingId Id;

    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        uiManager.SetBuildingState(Id);
    }
}