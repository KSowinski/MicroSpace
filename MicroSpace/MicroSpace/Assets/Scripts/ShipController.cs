using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static SelectedShipArea SelectedShipArea;

    public static Stat Shield { get; set; }
    public static Stat Hull { get; set; }
    public static Stat Fuel { get; set; }
    public static Stat Resources { get; set; }
    public static Stat Weapons { get; set; }
    public static Stat Thrusters { get; set; }

    public static Vector3 ShipPosition { get { return GameObject.FindGameObjectWithTag("Player").transform.position; } }

    void Start ()
    {
        SelectedShipArea = SelectedShipArea.None;
        Shield = new Stat(100, 100, "ShieldSlider", "ShieldText", true, false);
        Hull = new Stat(100, 100, "HullSlider", "HullText", true, false);
        Fuel = new Stat(100, 100, null, "FuelText", false, true);
        Resources = new Stat(999, 0, null, "ResourcesText", false, false);
        Weapons = new Stat(999, 1, null, null, false, false);
        Thrusters = new Stat(999, 1, null, null, false, false);

        Shield.Update(null, null);
        Hull.Update(null, null);
        Fuel.Update(null, null);
        Resources.Update(null, null);
    }
}