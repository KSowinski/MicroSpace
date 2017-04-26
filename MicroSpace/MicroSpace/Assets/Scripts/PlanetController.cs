using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetController : MonoBehaviour, IInit, IPointerDownHandler
{
    public Sprite[] PlanetSprites;

    private bool _rotateRight;
    private float _rotSpeed;
    private PlanetServices _planetServices;

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed * (_rotateRight ? 1f : -1f));
    }

    public void Init()
    {
        //Random sprite
        transform.GetComponent<SpriteRenderer>().sprite = PlanetSprites[Random.Range(0, PlanetSprites.Length)];

        //Minimum 0.75 upto 1.25 = (15 * 0.05) to (25 * 0.05)
        var sizeInt = Random.Range(15, 26);
        var sizeF = 0.05f*sizeInt;

        //Set size
        transform.localScale = new Vector3(sizeF, sizeF, 1f);

        //Smaller the obj - faster rotation - Biggest = 10f >> Smallest = 30f
        _rotSpeed = 55f - (sizeInt*2);

        //Random rotation - 50/50
        _rotateRight = Random.Range(1, 101) >= 51;

        //Add collider
        var cc = transform.gameObject.AddComponent<CircleCollider2D>();
        cc.radius = 0.55f;
        

        //Init Planet services
        _planetServices = InitPlanetServices();
    }

    private static PlanetServices InitPlanetServices()
    {
        var name = GetRandomName();
        var rr = Random.Range(1, 101) <= 75 ? Random.Range(5, 16) : default(int?);  //75% chance to exsit with price between 5-15
        var re = Random.Range(3, 11);
        var us = Random.Range(1, 101) <= 75 ? Random.Range(10, 26) : default(int?);
        var uh = Random.Range(1, 101) <= 75 ? Random.Range(10, 26) : default(int?);
        var uw = Random.Range(1, 101) <= 60 ? Random.Range(25, 51) : default(int?);
        var ut = Random.Range(1, 101) <= 60 ? Random.Range(15, 31) : default(int?);
        var sp = Random.Range(1, 101) <= 25 ? Random.Range(10, 26) : default(int?);
        
        return new PlanetServices()
        {
            PlanetName = name,
            RepairRecharge = rr,
            Refuel = re,
            UpgradeShield = us,
            UpgradeHull = uh,
            UpgradeWeapons = uw,
            UpgradeThrusters = ut,
            ScramblePortal = sp
        };
    }

    private static string GetRandomName()
    {
        const string letters = "QWERTYUIOPASDFGHJKLZXCVBNM";
        const string numbers = "1234567890";
        var locations = new List<string>() { "Alpha", "Beta", "Gamma", "Delta", "Zeta", "Sigma", "Omega" };

        //Letter 1
        var r = Random.Range(0, letters.Length);
        var letter1 = letters[r];

        //Letter 2
        r = Random.Range(0, letters.Length);
        var letter2 = letters[r];

        //Number
        r = Random.Range(0, numbers.Length);
        var number = numbers[r];

        //Location
        r = Random.Range(0, locations.Count);
        var location = locations[r];

        //Location Number
        r = Random.Range(0, numbers.Length);
        var locationNumber = numbers[r];

        return string.Format("{0}{1}-{2} {3}-{4}", letter1, letter2, number, location, locationNumber);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Beam controll
        if (ShipController.SelectedShipArea == SelectedShipArea.Comms)
        {
            if (Vector3.Distance(transform.position, ShipController.ShipPosition) <= 1.5f)
            {
                if (Inpt.GetMouseButton(0))
                {
                    FindObjectOfType<CommsController>().Show(_planetServices);
                }
            }
            else
            {
                Log.Info("Too Far from planet");
            }
        }
    }
}
