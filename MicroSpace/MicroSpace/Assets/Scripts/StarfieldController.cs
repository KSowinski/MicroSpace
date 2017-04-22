using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldController : MonoBehaviour
{
    public Sprite[] BgSprites;

    void Start ()
    {
        //Random sprite
        transform.GetComponent<SpriteRenderer>().sprite = BgSprites[Random.Range(0, BgSprites.Length)];
    }
}
