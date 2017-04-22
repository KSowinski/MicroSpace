using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public Sprite[] AsteroidSprites;
    private bool _rotateRight;
    private float _rotSpeed;
    
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
    }


    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed * (_rotateRight ? 1f : -1f));
    }
}
