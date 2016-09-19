using UnityEngine;
using UnityEngine.EventSystems;

public class UnitUI : MonoBehaviour, IPointerDownHandler
{
    UnitUIManager unitUIManager;
    public UnitId Id;

    void Start()
    {
        unitUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UnitUIManager;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        unitUIManager.SetUnitData(Id);
    }
}