using UnityEngine;

public class PortalController : MonoBehaviour, IInit
{
    public bool RotateRight;
    private float _rotSpeed;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        _rotSpeed = Random.Range(10f, 30f);

        //Random rotation
        var rot = Random.rotation;
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, rot.z, transform.rotation.w);
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed * (RotateRight ? 1f : -1f));
    }
}
