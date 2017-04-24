using UnityEngine;

public class CursorController : MonoBehaviour
{

    private float _currentTime;
    private float _targetAngle = -180f;

    // Use this for initialization
    void Start ()
	{
	    _currentTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Time.time < _currentTime + 2f) return;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _targetAngle), 3f * Time.deltaTime);
        if (_targetAngle < 0f && transform.eulerAngles.z >= 179f)
        {
            _currentTime = Time.time;
            _targetAngle = 0f;
        }
        else if (_targetAngle >= 0f && transform.eulerAngles.z <= 1f)
        {
            _currentTime = Time.time;
            _targetAngle = -180;
        }
    }
}
