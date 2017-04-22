using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour, IInit
{
    public Sprite[] PlanetSprites;

    private bool _rotateRight;
    private float _rotSpeed;

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
    }


    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed * (_rotateRight ? 1f : -1f));
    }
}
