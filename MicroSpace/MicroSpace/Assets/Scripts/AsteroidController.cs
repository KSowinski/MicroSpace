using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class AsteroidController : MonoBehaviour, IPointerDownHandler
{
    public Sprite[] AsteroidSprites;
    public GameObject AsteroidExplosionPrefab;

    private bool _rotateRight;
    private float _rotSpeed;
    private Resource _resources;
    
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

        //Set Asteroid Resource
        var resourceType = Random.Range(1, 101) >= 51 ? ResourceType.Fuel : ResourceType.Titanium;
        var resourceAmount = sizeInt*2;
        _resources = new Resource() { Amount = resourceAmount, Type = resourceType, SourceDestroyed = false };
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
            if (Vector3.Distance(transform.position, ShipController.ShipPosition) <= 1f)
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
        if (_resources.Amount > extractedAmount)
        {
            _resources.Amount -= extractedAmount;
            return new Resource() { Amount = extractedAmount, Type = _resources.Type, SourceDestroyed = false };
        }

        DestroySelf();
        return new Resource() { Amount = _resources.Amount, Type = _resources.Type, SourceDestroyed = true};
    }

    private void DestroySelf()
    {
        Instantiate(AsteroidExplosionPrefab, transform.position, Quaternion.identity);
        DestroyObject(this.transform.gameObject);
    }
}