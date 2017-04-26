using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class AsteroidController : MonoBehaviour, IPointerDownHandler
{
    public Sprite[] AsteroidSprites;
    public GameObject AsteroidExplosionPrefab;

    private bool _rotateRight;
    private float _rotSpeed;
    private List<Resource> _resources;
    
    public void Init()
    {
        //Random sprite
        transform.GetComponent<SpriteRenderer>().sprite = AsteroidSprites[Random.Range(0, AsteroidSprites.Length)];

        //Minimum 0.4 upto 0.65 = (4 * 0.05) to (13 * 0.05)
        var sizeInt = Random.Range(4, 14);
        var sizeF = 0.05f * sizeInt;

        //Set size
        transform.localScale = new Vector3(sizeF, sizeF, 1f);

        //Smaller the obj - faster rotation - Biggest = 10f >> Smallest = 30f
        _rotSpeed = 55f - (sizeInt * 2);

        //Random rotation - 50/50
        _rotateRight = Random.Range(1, 101) >= 51;
        
        //Add collider
        transform.gameObject.AddComponent<PolygonCollider2D>();

        //Set Asteroid Resources
        _resources = new List<Resource>();
        var fuelFirst = Random.Range(1, 101) >= 51;

        if (fuelFirst)
        {
            var fuelAmount = Random.Range(sizeInt * 2, sizeInt * 3 + 1);
            _resources.Add(new Resource() { Amount = fuelAmount, Type = ResourceType.Fuel, SourceDestroyed = false });

            var titaniumAmount = Random.Range(sizeInt * 2, sizeInt * 3 + 1);
            _resources.Add(new Resource() { Amount = titaniumAmount, Type = ResourceType.Titanium, SourceDestroyed = false });
        }
        else
        {
            var titaniumAmount = Random.Range(sizeInt * 2, sizeInt * 3 + 1);
            _resources.Add(new Resource() { Amount = titaniumAmount, Type = ResourceType.Titanium, SourceDestroyed = false });

            var fuelAmount = Random.Range(sizeInt * 2, sizeInt * 3 + 1);
            _resources.Add(new Resource() { Amount = fuelAmount, Type = ResourceType.Fuel, SourceDestroyed = false });
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed * (_rotateRight ? 1f : -1f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Beam controll
        if (ShipController.SelectedShipArea == SelectedShipArea.Beam)
        {
            if (Vector3.Distance(transform.position, ShipController.ShipPosition) <= 1.25f)
            {
                if (Inpt.GetMouseButton(0))
                {
                    FindObjectOfType<BeamController>().Activate(this.gameObject);
                }
            }
            else
            {
                Log.Info("Too Far from asteroid");
            }
        }
    }

    public Resource ExtractResource()
    {
        var extractedAmount = Random.Range(5, 16); //5-15
        var resource = _resources[0].Amount > 0 ? _resources[0] : _resources[1];

        if (resource.Amount > extractedAmount)
        {
            resource.Amount -= extractedAmount;
            return new Resource() { Amount = extractedAmount, Type = resource.Type, SourceDestroyed = false };
        }
        
        extractedAmount = resource.Amount;
        resource.Amount = 0;

        var anyLeft = _resources.Any(x => x.Amount > 0);
        if (!anyLeft) DestroySelf();

        return new Resource() { Amount = extractedAmount, Type = resource.Type, SourceDestroyed = !anyLeft };
    }

    private void DestroySelf()
    {
        Instantiate(AsteroidExplosionPrefab, transform.position, Quaternion.identity);
        DestroyObject(this.transform.gameObject);
    }
}