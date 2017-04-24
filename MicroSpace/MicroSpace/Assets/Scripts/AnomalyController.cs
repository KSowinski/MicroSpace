using UnityEngine;

public class AnomalyController : MonoBehaviour, IInit
{
    public Sprite[] AnomalySprites;

    private readonly float _minimum = 0.1f;
    private readonly float _maximum = 0.75f;
    private readonly float _duration = 5f;
    private float _startTime;
    private bool _fadeIn = true;

    public void Init()
    {
        //Init time
        _startTime = Time.time;

        //Random sprite
        transform.GetComponent<SpriteRenderer>().sprite = AnomalySprites[Random.Range(0, AnomalySprites.Length)];

        //Random rotation
        var rot = Random.rotation;
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, rot.z, transform.rotation.w);

        //Random initial alpah
        var sprite = transform.gameObject.GetComponent<SpriteRenderer>();
        sprite.color = new Color(1f, 1f, 1f, Random.Range(_minimum, _maximum));
    }

    void Update()
    {
        var t = (Time.time - _startTime) / _duration;
        var sprite = transform.gameObject.GetComponent<SpriteRenderer>();
        if (_fadeIn)
        {
            sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(_minimum, _maximum, t));
            if (sprite.color.a >= _maximum)
            {
                _fadeIn = false;
                _startTime = Time.time;
            }
        }
        else
        {
            sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(_maximum, _minimum, t));
            if (sprite.color.a <= _minimum)
            {
                _fadeIn = true;
                _startTime = Time.time;
            }
        }
    }
}
