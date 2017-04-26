using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemysSpawner : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;

    public GameObject TimesText1;
    public GameObject TimesText2;
    public GameObject AttachmentRing;
    public Canvas TimerCanvas;

    private float _time;
    private int _wave;
    private float _waveDelay = 60f;
    private bool _alreadySpawned;

    // Use this for initialization
    void Start ()
    {
        _wave = 1;
        ResetTimer();
    }

    // Update is called once per frame

    void Update ()
    {
        //Keep rotating with portal
        TimerCanvas.transform.rotation = AttachmentRing.transform.rotation;

        //Timer
        if (_time > 0f)
        {
            UpdateTimer(Time.deltaTime);
        }
        //New Wave
        else if (_time <= 0f)
        {
            if (!_alreadySpawned)
            {

                TimesText1.SetActive(false);
                TimesText2.SetActive(false);
                SpawnEnemies();
                _wave++;
                _alreadySpawned = true;
            }

            //Restart once there are no enemies
            if (GameObject.FindObjectsOfType<EnemyShipController>().Length == 0)
            {
                ResetTimer();
            }
        }
    }

    private void ResetTimer()
    {
        _alreadySpawned = false;
        TimesText1.SetActive(true);
        TimesText2.SetActive(true);
        _time = Mathf.Max(_waveDelay - ((_wave - 1)*5f), 10f);
        UpdateText();
    }

    private void UpdateTimer(float deltaTime)
    {
        _time -= deltaTime;
        if (_time < 0f) _time = 0f;
        UpdateText();
    }

    private void UpdateText()
    {
        var txt = _time.ToString("0.00");
        TimesText1.GetComponent<Text>().text = txt;
        TimesText2.GetComponent<Text>().text = txt;
    }

    private void SpawnEnemies()
    {
        for (var i = 1; i <= _wave + 1; i++)
        {
            var rndPos = RandomCircle(transform.position, new Vector2(0.75f, 0.75f));
            var rndEnemy = Random.Range(1, 4);
            GameObject enemy = null;
            if (rndEnemy == 1) enemy = Instantiate(Enemy1, rndPos, Quaternion.identity);
            if (rndEnemy == 2) enemy = Instantiate(Enemy2, rndPos, Quaternion.identity);
            if (rndEnemy == 3) enemy = Instantiate(Enemy3, rndPos, Quaternion.identity);
            if (enemy != null) enemy.GetComponent<EnemyShipController>().Init(_wave);
        }
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

    public void ScramblePortal()
    {
        _time += 10f;
    }
}
