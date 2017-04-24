using UnityEngine;

public class LaserController : MonoBehaviour
{
    private Vector3 _initialPosition;

    public void Start()
    {
        _initialPosition = transform.position;
    }

    void Update ()
	{
        transform.Translate(Vector3.right * Time.deltaTime * 3f);
        if (Vector2.Distance(_initialPosition, transform.position) > 3f) { Destroy(transform.gameObject); }
    }
}
