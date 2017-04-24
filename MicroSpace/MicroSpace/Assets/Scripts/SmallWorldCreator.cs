using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SmallWorldCreator : MonoBehaviour
{
    public GameObject WormholeInPrefab;
    public GameObject WormholeOutPrefab;
    public GameObject PortalPrefab;
    public GameObject PlanetPrefab;
    public GameObject MoonPrefab;
    public GameObject AsteroidsPrefab;
    public GameObject AnomalyPrefab;
    public GameObject PlayerShipPrefab;

    //Minimum number of objects:
    //2 Wormholes
    //1 Portal
    //1 Asteroid Field
    //1 Planet
    //1 Anomaly
    private const int MinObjects = 2 + 1 + 1 + 1 + 1;

    //Maximum number of objects:
    //2 Wormholes
    //1 Portal
    //2 Asteroid Field
    //3 Anomaly
    //3 Planet
    private const int MaxObjects = 2 + 1 + 2 + 3 + 3;

    public void Create()
    {
        var positions = GetPositionSlots();

        var player = 1;
        var wormholeIn = 1;
        var wormholeOut = 1;
        var portal = 1;
        var asteroids = 2;
        var anomaly = 3;
        var planets = 3;

        while (positions.Count > 0)
        {
            var randomPosition = Random.Range(0, positions.Count);
            var position = positions[randomPosition];
            positions.RemoveAt(randomPosition);

            //Player
            if (player > 0)
            {
                SpawnObject<PlayerShipController>(PlayerShipPrefab, position);
                player--;
            }
            //Wormholes
            else if (wormholeIn > 0)
            {
                SpawnObject<WormholeController>(WormholeInPrefab, position);
                wormholeIn--;
            }
            else if(wormholeOut > 0)
            {
                SpawnObject<WormholeController>(WormholeOutPrefab, position);
                wormholeOut--;
            }
            //Portal
            else if (portal > 0)
            {
                SpawnObject<PortalController>(PortalPrefab, position);
                portal--;
            }
            //Asteroids
            else if (asteroids > 1) //minimum required minus 1
            {
                SpawnObject<AsteroidsSpawner>(AsteroidsPrefab, position);
                asteroids--;
            }
            //Anomaly
            else if (anomaly > 2) //minimum required minus 1
            {
                SpawnObject<AnomalyController>(AnomalyPrefab, position);
                anomaly--;
            }
            //Planets
            else if (planets > 2) //minimum required minus 1
            {
                SpawnPlanet(PlanetPrefab, MoonPrefab, position);
                Log.Info("Planet");
                planets--;
            }
            //Other - once we've fulfilled all min object creation we will randomize the rest
            else
            {
                var created = false;

                while (!created)
                {
                    var randomChance = Random.Range(1, 101);

                    //55% for a planet
                    if (planets > 0 && randomChance >= 1 && randomChance <= 55)
                    {
                        SpawnPlanet(PlanetPrefab, MoonPrefab, position);
                        planets--;
                        created = true;
                    }
                    //30% for a anomaly
                    else if (anomaly > 0 && randomChance >= 56 && randomChance <= 85)
                    {
                        SpawnObject<AnomalyController>(AnomalyPrefab, position);
                        anomaly--;
                        created = true;
                    }
                    //15% for a asteroids
                    else if (asteroids > 0 &&randomChance >= 86 && randomChance <= 100)
                    {
                        SpawnObject<AsteroidsSpawner>(AsteroidsPrefab, position);
                        asteroids--;
                        created = true;
                    }
                }
            }
        }
    }

    private void SpawnPlanet(GameObject planetPrefab, GameObject moonPrefab, Vector3 position)
    {
        var planet = SpawnObject<PlanetController>(planetPrefab, position);

        var moon1Chance = Random.Range(1, 101) > 50;
        if (moon1Chance)
        {
            var rndRot = Random.rotation.z;
            var moon1 = Instantiate(moonPrefab, position, new Quaternion(Quaternion.identity.x, Quaternion.identity.y, rndRot, Quaternion.identity.w));
            moon1.GetComponent<MoonController>().Init(false, planet.transform);
        }

        var moon2Chance = Random.Range(1, 101) > 90;
        if (moon2Chance)
        {
            var rndRot = Random.rotation.z;
            var moon2 = Instantiate(moonPrefab, position, new Quaternion(Quaternion.identity.x, Quaternion.identity.y, rndRot, Quaternion.identity.w));
            moon2.GetComponent<MoonController>().Init(true, planet.transform);
        }
    }

    private static GameObject SpawnObject<T>(GameObject go, Vector3 position) where T: IInit
    {
        var rndRot = Random.rotation.z;
        var obj = Instantiate(go, position, new Quaternion(Quaternion.identity.x, Quaternion.identity.y, rndRot, Quaternion.identity.w));
        var component = obj.GetComponent<T>();
        if(component != null) component.Init();
        return obj;
    }

    private List<Vector3> GetPositionSlots()
    {
        const int xDiv = 7;
        const int yDiv = 5;

        var list = new List<Vector3>();
        var partX = Screen.width / xDiv;
        
        var partY = Screen.height / yDiv;

        for (var x = 0; x < xDiv; x++)
        {
            for (var y = 0; y < yDiv; y++)
            {
                var posX = x * partX + (partX / 2f);
                var posY = y * partY + (partY / 2f);
                var position = new Vector3(posX, posY, 0f);
                //20% of x/y part so it won't go too close to edge of "slot"
                var rndPos = RandomCircle(position, new Vector2( (partX / 2f) * 0.20f, (partY / 2f) * 0.20f));
                var worldPos = Camera.main.ScreenToWorldPoint(rndPos);
                list.Add(new Vector3(worldPos.x, worldPos.y, 0f));
            }
        }

        //Remove random positions
        var currentObjectCount = Random.Range(MinObjects, MaxObjects + 2); //1 extra for player start point
        while (list.Count > currentObjectCount) list.RemoveAt(Random.Range(0, list.Count));

        return list;
    }

    private static Vector3 RandomCircle(Vector3 center, Vector2 radius)
    {
        var ang = Random.value * 360f;
        Vector3 pos;
        pos.x = center.x + radius.x * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius.y * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

}

