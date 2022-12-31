using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject _platform;
    [SerializeField] GameObject _startPoint;
    [SerializeField] GameObject _endPoint;
    [SerializeField] float _speed = 0.7f;

    private int _direction = 1;

    void Update()
    {
        Vector2 target = CurrentMoventTarget();
        _platform.transform.position = Vector2.Lerp(_platform.transform.position, target, _speed * Time.deltaTime);
        float distance = (target - (Vector2)_platform.transform.position).magnitude;

        if (distance <= 0.1f)
        {
            _direction *= -1;
        }
    }

    Vector2 CurrentMoventTarget()
    {
        if (_direction == 1)
        {
            return _startPoint.transform.position;
        }
        else
        {
            return _endPoint.transform.position;
        }
    }
}
