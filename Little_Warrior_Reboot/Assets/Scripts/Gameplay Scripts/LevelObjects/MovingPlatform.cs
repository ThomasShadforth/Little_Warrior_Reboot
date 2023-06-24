using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Platform
{
    [SerializeField] Vector2 _moveVelocity;
    [SerializeField] Vector2 _startPosition;
    [SerializeField] float _distanceLimit;

    [SerializeField] bool _shouldWait;
    [SerializeField] float _waitTime = 1f;
    bool _waiting;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }


    protected override void Move()
    {
        if(_waiting || GamePause.paused)
        {
            return;
        }

        transform.position += (Vector3)_moveVelocity * GamePause.deltaTime;

        float distanceTravelled = Vector2.Distance(_startPosition, transform.position);

        if(distanceTravelled >= _distanceLimit)
        {
            if (_shouldWait)
            {
                StartCoroutine(_WaitCo());
            }
            else
            {
                transform.position = _startPosition;
            }
        }
    }

    IEnumerator _WaitCo()
    {
        _waiting = true;
        _startPosition = transform.position;
        _moveVelocity *= -1;

        float timePercentage = 0f;

        while(timePercentage < 1)
        {
            timePercentage += GamePause.deltaTime / _waitTime;

            yield return null;
        }

        _waiting = false;
    }
}
