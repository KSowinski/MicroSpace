using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeController : MonoBehaviour, IInit
{
    public bool RotateRight = true;

    private SpriteRenderer _spriteRenderer;
    private readonly float _minimum = 0.25f;
    private readonly float _maximum = 0.9f;
    private readonly float _duration = 1.0f;
    private readonly float _rotSpeed = 180f;
    private float _startTime;
    private bool _fadeIn = true;

    public void Init()
    {
        _startTime = Time.time;
        _spriteRenderer =  transform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var t = (Time.time - _startTime) / _duration;
        if (_fadeIn)
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(_minimum, _maximum, t));
            if (_spriteRenderer.color.a >= _maximum)
            {
                _fadeIn = false;
                _startTime = Time.time;
            }    
        }
        else
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(_maximum, _minimum, t));
            if (_spriteRenderer.color.a <= _minimum)
            {
                _fadeIn = true;
                _startTime = Time.time;
            }
        }
       
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotSpeed * (RotateRight ? 1f : -1f));
    }
}