using UnityEngine;

public class MoonController : MonoBehaviour
{
    public Sprite[] MoonSprites;
    
    private float _rotSpeed;
    private bool _orbitRight;
    private float _radius;
    public Transform CenterPlanet;
    
    public void Init(bool isSecondMoon, Transform centerPlanet)
    {
        //Random sprite
        transform.GetComponent<SpriteRenderer>().sprite = MoonSprites[Random.Range(0, MoonSprites.Length)];

        //Minimum 0.2 upto 0.6 = (4 * 0.05) to (12 * 0.05)
        var sizeInt = Random.Range(4, 13);
        var sizeF = 0.05f * sizeInt;

        //Set size
        transform.localScale = new Vector3(sizeF, sizeF, 1f);

        //Smaller the obj - faster rotation - Biggest = 10f >> Smallest = 30f
        _rotSpeed = Random.Range(20f, 40f);

        //Random rotation - 50/50
        //RotateRight = Random.Range(1, 101) >= 51;
        _orbitRight = Random.Range(1, 101) >= 51;

        //Add collider
        var cc = transform.gameObject.AddComponent<CircleCollider2D>();
        cc.radius = 0.55f;

        //Set orbit radious
        _radius = (centerPlanet.localScale.x / 2f) + (sizeF / 2f) + 0.1f + (isSecondMoon ? 0.65f : 0f);
        
        //Set position
        var offset = (transform.position - centerPlanet.position).normalized;
        if (offset == Vector3.zero)
        {
            transform.position = new Vector3(centerPlanet.position.x + _radius, centerPlanet.position.y + _radius, 0f);
        }
        else
        {
            transform.position = offset * _radius + centerPlanet.position;
        }
        

        //Set parent
        transform.SetParent(centerPlanet);

        CenterPlanet = centerPlanet;
    }


    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed * 1.5f * (_orbitRight ? -1f : 1f));
        transform.RotateAround(CenterPlanet.position, Vector3.forward, (_orbitRight ? 1f : -1f) *_rotSpeed * Time.deltaTime);
    }
}
