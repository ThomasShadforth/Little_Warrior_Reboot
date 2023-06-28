using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovingPlatform : Platform
{
    [SerializeField] Vector2 _moveVelocity;
    [SerializeField] Vector2 _startPosition;
    [SerializeField] float _distanceLimit;

    [SerializeField] bool _shouldWait;
    [SerializeField] float _waitTime;

    Rigidbody2D _rb;
    bool _waiting;

    protected override void Move()
    {
        if (_waiting || GamePause.paused) return;

        _rb.transform.position += (Vector3) _moveVelocity * GamePause.deltaTime;

        float distanceTravelled = Vector2.Distance(_rb.transform.position, _startPosition);

        //Debug.Log(distanceTravelled);

        if(distanceTravelled >= _distanceLimit)
        {
            if (_shouldWait)
            {
                //Debug.Log("REACHED/EXCEEDED MAXIMUM DISTANCE");
                StartCoroutine(_WaitCo());
            }
            else
            {
                _rb.transform.position = _startPosition;
            }
        }
    }

    public override void SetStartingState()
    {
        base.SetStartingState();

        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator _WaitCo()
    {
        _waiting = true;
        _startPosition = _rb.transform.position;
        _moveVelocity *= -1;

        float timePercentage = 0f;

        while (timePercentage < 1)
        {
            timePercentage += GamePause.deltaTime / _waitTime;

            yield return null;
        }

        _waiting = false;
    }

}
