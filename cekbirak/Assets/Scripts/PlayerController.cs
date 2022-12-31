using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rb;
    LineRenderer _lr;
    Camera _mainCamera;

    [SerializeField] float _power = 7f;
    [SerializeField] Vector2 _minPower = new Vector2(-20f, -20f);
    [SerializeField] Vector2 _maxPower = new Vector2(20f, 20f);

    [SerializeField] Vector2 _minPowerLine;
    [SerializeField] Vector2 _maxPowerLine;

    Vector2 _force;
    Vector3 _startPoint;
    Vector3 _endPoint;

    Vector3 _startLrPos;
    Vector3 _endLrPos;
    Vector3 _mouseDir;

    bool _isClick = false;
    bool _isGround = true;
    private bool _doubleJump = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_isGround || _doubleJump)
            {
                _rb.isKinematic = false;
                _startPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                _startPoint.z = 15;

                _lr.enabled = true;

                _startLrPos = gameObject.transform.position;
                _startLrPos.z = 15;
                _lr.SetPosition(0, _startLrPos);
                _lr.SetPosition(1, _startLrPos);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (_isGround || _doubleJump)
            {
                _isClick = true;
                _doubleJump = !_doubleJump;
                _lr.enabled = false;
                Debug.Log(_doubleJump);
            }
        }
        if (Input.GetButton("Fire1"))
        {
            if (_rb.velocity != new Vector2(0f, 0f))
            {
                _startLrPos = gameObject.transform.position;
                _lr.SetPosition(0, _startLrPos);
                _lr.SetPosition(1, _startLrPos);
            }
            _endPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;
            _force = new Vector2(Mathf.Clamp(_startPoint.x - _endPoint.x, _minPower.x, _maxPower.x), Mathf.Clamp(_startPoint.y - _endPoint.y, _minPower.y, _maxPower.y));
            _endLrPos = new Vector2(Mathf.Clamp(_force.x, _minPowerLine.x, _maxPowerLine.y), Mathf.Clamp(_force.y, _minPowerLine.x, _maxPowerLine.y));
            _endLrPos.z = 15;
            _lr.SetPosition(1, gameObject.transform.position + _endLrPos);
        }
    }

    private void FixedUpdate()
    {
        Jump();
    }

    private void Jump()
    {
        if (_isClick)
        {
            _rb.AddForce(_force * _power, ForceMode2D.Impulse);
            _isClick = false;
            _isGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Vector3 offset = col.gameObject.transform.position - transform.position;
            offset = offset.normalized;
            _rb.velocity = offset * _power;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && _rb.velocity == new Vector2(0f, 0f))
        {
            _isGround = true;
            _doubleJump = false;
        }
    }
}

