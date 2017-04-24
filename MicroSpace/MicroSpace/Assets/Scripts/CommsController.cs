using UnityEngine;
using UnityEngine.UI;

public class CommsController : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Title;
    public GameObject RepairRechargeButton;
    public GameObject RefuelButton;
    public GameObject UpgradeShieldButton;
    public GameObject UpgradeHullButton;
    public GameObject UpgradeWeaponsButton;
    public GameObject UpgradeThrustersButton;
    public GameObject ScramblePortalButton;

    private PlanetServices _currentPlanet;

    public void Show(PlanetServices planetServices)
    {
        _currentPlanet = planetServices;

        Panel.SetActive(true);

        Title.SetActive(true);
        Title.transform.GetComponent<Text>().text = planetServices.PlanetName;

        SetButton(RepairRechargeButton, planetServices.RepairRecharge);
        SetButton(RefuelButton, planetServices.Refuel);
        SetButton(UpgradeShieldButton, planetServices.UpgradeShield);
        SetButton(UpgradeHullButton, planetServices.UpgradeHull);
        SetButton(UpgradeWeaponsButton, planetServices.UpgradeWeapons);
        SetButton(UpgradeThrustersButton, planetServices.UpgradeThrusters);
        SetButton(ScramblePortalButton, planetServices.ScramblePortal);
    }

    public void Hide()
    {
        _currentPlanet = null;
        Panel.SetActive(false);
        Title.SetActive(false);
        RepairRechargeButton.SetActive(false);
        RefuelButton.SetActive(false);
        UpgradeShieldButton.SetActive(false);
        UpgradeHullButton.SetActive(false);
        UpgradeWeaponsButton.SetActive(false);
        UpgradeThrustersButton.SetActive(false);
        ScramblePortalButton.SetActive(false);
    }

    private static void SetButton(GameObject button, int? value)
    {
        if (!value.HasValue)
        {
            button.SetActive(false);
            return;
        }

        button.SetActive(true);
        var priceText = button.transform.FindChild("Price");
        priceText.transform.GetComponent<Text>().text = value.ToString();
    }

    #region PURCHASE SERVICE
    public void ServiceRepairRecharge()
    {
        if (!_currentPlanet.RepairRecharge.HasValue || ShipController.Resources.Current < _currentPlanet.RepairRecharge.Value) return;
        ShipController.Resources.Update(-1 * _currentPlanet.RepairRecharge.Value, null);
        ShipController.Shield.MaxOut();
        ShipController.Hull.MaxOut();
        SoundManager.Inst().Buy();
    }

    public void ServiceRefuel()
    {
        if (!_currentPlanet.Refuel.HasValue || ShipController.Resources.Current < _currentPlanet.Refuel.Value) return;
        ShipController.Resources.Update(-1 * _currentPlanet.Refuel.Value, null);
        ShipController.Fuel.Update(10, null);
        SoundManager.Inst().Buy();
    }

    public void ServiceUpgradeShield()
    {
        if (!_currentPlanet.UpgradeShield.HasValue || ShipController.Resources.Current < _currentPlanet.UpgradeShield.Value) return;
        ShipController.Resources.Update(-1 * _currentPlanet.UpgradeShield.Value, null);
        ShipController.Shield.Update(null, 10);
        ShipController.Shield.MaxOut();
        SoundManager.Inst().Buy();
    }

    public void ServiceUpgradeHull()
    {
        if (!_currentPlanet.UpgradeHull.HasValue || ShipController.Resources.Current < _currentPlanet.UpgradeHull.Value) return;
        ShipController.Resources.Update(-1 * _currentPlanet.UpgradeHull.Value, null);
        ShipController.Hull.Update(null, 10);
        ShipController.Hull.MaxOut();
        SoundManager.Inst().Buy();
    }

    public void ServiceUpgradeWeapons()
    {
        if (!_currentPlanet.UpgradeWeapons.HasValue || ShipController.Resources.Current < _currentPlanet.UpgradeWeapons.Value) return;
        ShipController.Resources.Update(-1 * _currentPlanet.UpgradeWeapons.Value, null);
        ShipController.Weapons.Update(5, null);
        SoundManager.Inst().Buy();
    }

    public void ServiceUpgradeThrusters()
    {
        if (!_currentPlanet.UpgradeThrusters.HasValue || ShipController.Resources.Current < _currentPlanet.UpgradeThrusters.Value) return;
        ShipController.Resources.Update(-1 * _currentPlanet.UpgradeThrusters.Value, null);
        ShipController.Thrusters.Update(1, null);
        SoundManager.Inst().Buy();
    }

    public void ServiceScramblePortal()
    {
        if (!_currentPlanet.ScramblePortal.HasValue || ShipController.Resources.Current < _currentPlanet.ScramblePortal.Value) return;
        ShipController.Resources.Update(-1 * _currentPlanet.ScramblePortal.Value, null);
        FindObjectOfType<EnemysSpawner>().ScramblePortal();
        SoundManager.Inst().Buy();
    }
    #endregion
}