using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rb;
    Camera _mainCamera;

    [SerializeField] float _power = 7f;
    [SerializeField] Vector2 _minPower = new Vector2(-20f, -20f);
    [SerializeField] Vector2 _maxPower = new Vector2(20f, 20f);

    Vector2 _force;
    Vector3 _startPoint;
    Vector3 _endPoint;

    bool _isClick = false;
    bool _isGround = true;
    bool _isTrown = false;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _isGround)
        {
            _rb.isKinematic = false;
            _startPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15;
        }
        if (Input.GetButtonUp("Fire1") && _isGround)
        {
            _endPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;
            _isClick = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isClick && _isGround && !_isTrown)
        {
            _force = new Vector2(Mathf.Clamp(_startPoint.x - _endPoint.x, _minPower.x, _maxPower.x), Mathf.Clamp(_startPoint.y - _endPoint.y, _minPower.y, _maxPower.y));
            _rb.AddForce(_force * _power, ForceMode2D.Impulse);
            _isClick = false;
            // _isTrown = true;
            _isGround = false;
            Debug.Log("Force: " + _force);
        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("Ground") && _rb.velocity == new Vector2(0f, 0f))
        {
            _isTrown = false;
            _isGround = true;
            Debug.Log("Ground");
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && _rb.velocity == new Vector2(0f, 0f))
        {
            Debug.Log("Sleeping " + _rb.IsSleeping() + "_isGround " + _isGround);
            _isGround = true;
        }
    }

}

