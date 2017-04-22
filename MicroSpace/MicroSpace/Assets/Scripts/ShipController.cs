using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    public static SelectedShipArea SelectedShipArea = SelectedShipArea.None;

    public static Stat Shield = new Stat(100, 100, "ShieldSlider", "ShieldText", true, false);
    public static Stat Hull = new Stat(100, 100, "HullSlider", "HullText", true, false);
    public static Stat Fuel = new Stat(100, 100, null, "FuelText", false, true);
    public static Stat Resources = new Stat(999, 0, null, "ResourcesText", false, false);

    void Start ()
    {
		Shield.Update(null, null);
        Hull.Update(null, null);
        Fuel.Update(null, null);
        Resources.Update(null, null);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}