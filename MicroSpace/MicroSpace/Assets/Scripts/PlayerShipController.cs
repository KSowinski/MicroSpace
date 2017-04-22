using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour, IInit
{

    private Vector3 _mousePosition;
    private float _moveSpeed = 0.001f;
    private float _rotationSpeed = 360f;

    public void Init()
    {
        
    }

    void Start ()
    {
	}

    void Update ()
    {
        if (Input.GetMouseButton(1))
        {
            _mousePosition = Input.mousePosition;
            _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
        }

        //MOVE
        if (transform.position != _mousePosition)
        {
            transform.position = Vector2.Lerp(transform.position, _mousePosition, _moveSpeed);
        }

        //ROTATE
        var ray = Camera.main.ScreenPointToRay(_mousePosition);
        var plane = new Plane(Vector3.up, transform.position);
        float dist;

        if (plane.Raycast(ray, out dist))
        {
            var rotation = Quaternion.LookRotation(ray.GetPoint(dist) - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        var a = coll;
    }
}
