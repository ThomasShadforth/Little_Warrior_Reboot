using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAcceleration
{
    float _speed;
    float _maxSpeed;
    float _timeToReachMaxSpeed = .3f;
    float _timeToDecel = .3f;
    float _accelRate;
    float _decelRate;
    float _friction = 2.2f;

    float _prevMoveInput = 1;
    float _moveInput;

    bool _decelerating;
    bool _decelTargetReached;

    public PlayerAcceleration(float maxSpeed)
    {
        SetMaxSpeed(maxSpeed);
    }

    public float GetAccelSpeed(float moveInput)
    {

        if(_moveInput != 0)
        {
            _prevMoveInput = _moveInput;
        }

        _moveInput = moveInput;

        if (_moveInput != _prevMoveInput && _moveInput != 0)
        {
            Debug.Log("TURNING");
            _decelerating = true;
        }

        
        if (_speed > 0 && _decelerating)
        {
            Debug.Log("DECELERATING");
            _speed -= _decelRate * 1.9f * GamePause.deltaTime;

            if (_speed < .5f)
            {
                _speed = .5f;
                _decelerating = false;
            }
                
        }
        else if (_speed < _maxSpeed && !_decelerating)
        {
            _speed += _accelRate * GamePause.deltaTime;

            if (_speed >= _maxSpeed)
            {
                _speed = _maxSpeed;
            }
        }
        else if (_moveInput == 0)
        {

            _speed -= (Mathf.Min(Mathf.Abs(_speed * 2.2f), _friction * 2.2f) * Mathf.Sign(_speed) * _maxSpeed * GamePause.deltaTime);
            _decelerating = false;
        }

        //Debug.Log(_speed);

        return _speed;
    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        this._maxSpeed = newMaxSpeed;
        _accelRate = (_maxSpeed) / _timeToReachMaxSpeed;
        _decelRate = (_maxSpeed - (_maxSpeed / 2)) / _timeToDecel;
    }
}
