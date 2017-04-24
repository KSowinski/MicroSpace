using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject ExplosionPrefab1;
    public GameObject ExplosionPrefab2;
    public GameObject ExplosionPrefab3;

    private static bool _gameOver = false;
    private static float _explosionDelay;

    public static void ShhhOnlyDreamsNow()
    {
        Inpt.IsEnabled = false;
        _gameOver = true;
        _explosionDelay = 0f;
    }

    public void Update()
    {
        if(!_gameOver) return;

        if (_explosionDelay > 0f)
        {
            _explosionDelay -= Time.deltaTime;
            return;
        }

        SoundManager.Inst().Explosion();

        var rndCount = Random.Range(1, 4);
        for (int i = 1; i <= rndCount; i++)
        {
            var rndPrefab = Random.Range(1, 4);
            if (rndPrefab == 1) CreateExplosion(ExplosionPrefab1);
            if (rndPrefab == 2) CreateExplosion(ExplosionPrefab2);
            if (rndPrefab == 3) CreateExplosion(ExplosionPrefab3);
        }
        
        _explosionDelay = 0.25f;

        StartCoroutine(DelayedRestart());
    }
    
    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(5);
        RestartLevel();
    }

    private static void CreateExplosion(GameObject prefab)
    {
        var rndPoint = RandomCircle(ShipController.ShipPosition, new Vector2(0.25f, 0.25f));
        Instantiate(prefab, rndPoint, Quaternion.identity);
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

    private void RestartLevel()
    {
        _gameOver = false;  
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
