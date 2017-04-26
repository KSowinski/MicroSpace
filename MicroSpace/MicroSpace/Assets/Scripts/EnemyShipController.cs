using System.Linq;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    public GameObject DamageEffect;
    public GameObject LaserPrefab;
    public GameObject[] FirePoints;

    private float _movingSpeed = 0.005f;
    private float _fireDelay = 0.5f;
    private int _hp;

    public void Init(int wave)
    {
        transform.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        _hp = 5 + (wave * 2);
    }

    void Update()
    {
        MoveOrAttack();
    }

    private void MoveOrAttack()
    {
        //Always rotate
        var angle = (Mathf.Atan2(ShipController.ShipPosition.y - transform.position.y,
                ShipController.ShipPosition.x - transform.position.x) * Mathf.Rad2Deg);
        angle = (angle < 0f) ? angle + 360f : angle;
        var quaternion = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), 5f * Time.deltaTime);
        transform.rotation = quaternion;

        //Move or shoot
        if (Vector2.Distance(transform.position, ShipController.ShipPosition) >= 2f)
        {
            transform.position = Vector2.Lerp(transform.position, ShipController.ShipPosition, _movingSpeed);
        }
        //Shoot
        else
        {
            if (_fireDelay > 0f)
            {
                _fireDelay -= Time.deltaTime;
                return;
            }

            var rndFirePoint = FirePoints[Random.Range(0, FirePoints.Length)];
            var laser = Instantiate(LaserPrefab, rndFirePoint.transform.position, transform.localRotation);
            laser.GetComponent<SpriteRenderer>().color = transform.GetComponent<SpriteRenderer>().color;
            laser.name = "EnemyBullet";
            _fireDelay = 0.5f;
            SoundManager.Inst().Laser();
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.name.Contains("PlayerBullet"))
        {
            Instantiate(DamageEffect, coll.contacts.First().point, Quaternion.identity);
            _hp -= ShipController.Weapons.Current * 2;
            SoundManager.Inst().Damage();
            Destroy(coll.collider);

            if (_hp <= 0)
            {
                DestroyShip();
            }
        }
    }

    private void DestroyShip()
    {
        var rndExplosions = Random.Range(5, 11);
        for (var i = 1; i <= rndExplosions; i++)
        {
            var rndPos = RandomCircle(transform.position, new Vector2(0.15f, 0.15f));
            Instantiate(DamageEffect, rndPos, Quaternion.identity);
            SoundManager.Inst().Explosion();
        }

        FindObjectOfType<ScoreController>().AddScore(250);

        Destroy(this.transform.gameObject);
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
