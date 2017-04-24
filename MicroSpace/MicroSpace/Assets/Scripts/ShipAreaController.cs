using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipAreaController : MonoBehaviour, IPointerDownHandler
{
    public SelectedShipArea OnSelectionArea;
    public SpriteRenderer OnSelectionSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        SetActive();
    }

    public void SetActive()
    {
        On();
        var other = GameObject.FindObjectsOfType<ShipAreaController>();
        foreach (var x in other.Where(x => x.OnSelectionArea != OnSelectionArea)) x.Off();

        //Set custom UI visible/hide other
        if (OnSelectionArea == SelectedShipArea.Beam)
        {
            FindObjectOfType<BeamController>().Enable();
            HideEverythingBut(OnSelectionArea);
        }
        if (OnSelectionArea == SelectedShipArea.Comms)
        {
            //Do nothing here - comms menu enabled on planet click
            HideEverythingBut(OnSelectionArea);
        }
        if (OnSelectionArea == SelectedShipArea.Shield)
        {
            FindObjectOfType<MouseGameController>().Activate();
            HideEverythingBut(OnSelectionArea);
        }
        if (OnSelectionArea == SelectedShipArea.Repair)
        {
            FindObjectOfType<MouseGameController>().Activate();
            HideEverythingBut(OnSelectionArea);
        }
        else
        {
            HideEverythingBut(OnSelectionArea);
        }
    }

    private static void HideEverythingBut(SelectedShipArea area)
    {
        if (area != SelectedShipArea.Beam) FindObjectOfType<BeamController>().Disable();
        if (area != SelectedShipArea.Comms) FindObjectOfType<CommsController>().Hide();
        if (area != SelectedShipArea.Shield && area != SelectedShipArea.Repair) FindObjectOfType<MouseGameController>().Hide();
    }

    private void On()
    {
        ShipController.SelectedShipArea = OnSelectionArea;
        OnSelectionSprite.color = new Color(OnSelectionSprite.color.r, OnSelectionSprite.color.g, OnSelectionSprite.color.b, 1f);
        var gos = GameObject.FindGameObjectsWithTag("ShipAreaTitle");
        foreach (var go in gos) go.GetComponent<Text>().text = OnSelectionArea.ToString();
    }

    private void Off()
    {
        OnSelectionSprite.color = new Color(OnSelectionSprite.color.r, OnSelectionSprite.color.g, OnSelectionSprite.color.b, 0f);
    }
}
