﻿using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour, IInit
{
    public GameObject AsteroidPrefab; 
    
    public void Init()
    {
        var count = Random.Range(3, 7);
        for (var i = 1; i <= count; i++)
        {
            var r = Random.insideUnitCircle;
            var position = new Vector3(transform.position.x + r.x, transform.position.y + r.y, 0f);
            var asteroid = Instantiate(AsteroidPrefab, position, Quaternion.identity);
            asteroid.GetComponent<AsteroidController>().Init();
            asteroid.transform.parent = transform;
        }
    }
}
