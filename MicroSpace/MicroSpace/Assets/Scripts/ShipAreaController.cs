using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipAreaController : MonoBehaviour, IPointerDownHandler
{
    public SelectedShipArea OnSelectionArea;
    public SpriteRenderer OnSelectionSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        On();
        var other = GameObject.FindObjectsOfType<ShipAreaController>();
        foreach (var x in other.Where(x => x.OnSelectionArea != OnSelectionArea)) x.Off();
    }

    public void On()
    {
        ShipController.SelectedShipArea = OnSelectionArea;
        OnSelectionSprite.color = new Color(OnSelectionSprite.color.r, OnSelectionSprite.color.g, OnSelectionSprite.color.b, 1f);
    }

    public void Off()
    {
        OnSelectionSprite.color = new Color(OnSelectionSprite.color.r, OnSelectionSprite.color.g, OnSelectionSprite.color.b, 0f);
    }
}
