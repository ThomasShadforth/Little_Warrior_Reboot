using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatedMovingPlatform : Platform
{
    [SerializeField] Vector2 _moveVelocity;
    [SerializeField] Vector2 _startPosition;
    [SerializeField] Vector2 _positionOffset;

    [SerializeField] float _conditionCheckDistance;
    [SerializeField] float _maxDistance;

    [SerializeField] LevelCondition _primaryCondition;

    bool _conditionFulfilled;
    bool _platformStopped;

    Rigidbody2D _rb;


    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Move()
    {
        if (GamePause.paused || _platformStopped)
        {
            return;
        }

        _rb.transform.position += (Vector3)_moveVelocity * GamePause.deltaTime;

        float distanceTravelled = Vector2.Distance(_startPosition, _rb.transform.position);

        //Debug.Log(distanceTravelled);

        if(distanceTravelled >= _conditionCheckDistance && !_conditionFulfilled)
        {
            

            if(_primaryCondition != null)
            {
                _AssessCondition();
            }

            if (!_conditionFulfilled)
            {
                //Debug.Log("SETTING POSITION");
                _rb.transform.position = _startPosition;
            }

            
        }

        if(distanceTravelled >= +_maxDistance)
        {
            _platformStopped = true;
        }
    }

    public override void CheckMovementCondition()
    {
        if (_needsPlayer)
        {
            if (_primaryCondition != null && !_hasStarted)
            {
                _hasStarted = true;
                _primaryCondition.ActivateCondition();
            }

        }
    }

    public override void SetStartingState()
    {
        base.SetStartingState();

        _startPosition = transform.position + (Vector3)_positionOffset;
        _rb = GetComponent<Rigidbody2D>();
    }

    //Compared to regular moving platforms, this platform will have a condition check distance and maximum distance.
    //When reaching the condition check distance, the primary condition will be assessed, along with any sub-conditions
    void _AssessCondition()
    {
        if (_primaryCondition.CheckCondition())
        {
            _conditionFulfilled = true;
        }
    }
}
