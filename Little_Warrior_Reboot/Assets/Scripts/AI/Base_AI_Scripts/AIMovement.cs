using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIMovement : MonoBehaviour
{
    AIHeightMaintenance _heightMaintenance;
    SlopeDetection _slopeDetection;
    AIStatus _aiStatus;
    Rigidbody2D _rb2d;
    Vector2 _velocity;

    float _xDirection;

    bool _grounded;
    bool _isChasing;

    //AI Max speed will change based on patrol/chase states
    float _speed;
    [SerializeField]
    float _patrolMaxSpeed;
    [SerializeField]
    float _chaseMaxSpeed;
    float _currentMaxSpeed;
    float _timeToReachMaxSpeed = .3f;
    float _timeToDecel = .3f;
    float _accelRate;
    float _decelRate;
    float _friction = 2.2f;

    [Header("Slope Detection Config:")]
    [SerializeField] float _slopeCheckDistance;
    [SerializeField] LayerMask _whatIsGround;
    CapsuleCollider2D _capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _rb2d = GetComponent<Rigidbody2D>();
        _aiStatus = GetComponent<AIStatus>();
        _heightMaintenance = GetComponent<AIHeightMaintenance>();
        SetMaxSpeed(false);
        _slopeDetection = new SlopeDetection(this.transform, _capsuleCollider.size, _slopeCheckDistance, _whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (GamePause.paused)
        {
            _rb2d.velocity = Vector2.zero;
            return;
        }

        if((_aiStatus && _aiStatus.CheckForStatus()))
        {
            _speed = 0;
            _xDirection = 0;
            return;
        }

        _grounded = _heightMaintenance.GetGrounded();

        _velocity = _rb2d.velocity;

        if (_xDirection < -.1f)
        {
            if (_speed > 0)
            {
                _speed += _decelRate * 1.9f * GamePause.deltaTime;

                if (_speed < 0)
                {
                    _speed = -.5f;
                }
            }
            else if (_speed > -_currentMaxSpeed)
            {
                _speed -= _accelRate * GamePause.deltaTime;

                if (_speed < -_currentMaxSpeed) _speed = -_currentMaxSpeed;

            }
        }
        else if (_xDirection > .1f)
        {
            if (_speed < 0)
            {
                _speed -= _decelRate * 1.9f * GamePause.deltaTime;

                if (_speed > 0) _speed = .5f;

            }
            else if (_speed < _currentMaxSpeed)
            {
                _speed += _accelRate * GamePause.deltaTime;

                if (_speed > _currentMaxSpeed) _speed = _currentMaxSpeed;
            }
        }
        else if (_xDirection == 0)
        {
            _speed -= (Mathf.Min(Mathf.Abs(_speed * 2.2f), _friction * 2.2f) * Mathf.Sign(_speed) * _currentMaxSpeed * GamePause.deltaTime);
        }

        if (_slopeDetection != null)
        {
            _slopeDetection.CheckSlope();
        }

        if((_heightMaintenance.GetGrounded() && !_slopeDetection.GetIsOnSlope()) || !_heightMaintenance.GetGrounded())
        {
            _velocity = new Vector2(_speed, _rb2d.velocity.y);
        } else if(_heightMaintenance.GetGrounded() && _slopeDetection.GetIsOnSlope())
        {
            _velocity = new Vector2(Mathf.Abs(_speed) * _slopeDetection.GetSlopeNormalPerpendicular().x * -_xDirection,
                Mathf.Abs(_speed) * _slopeDetection.GetSlopeNormalPerpendicular().y * -_xDirection);
        }

        //_velocity.x = _speed;
        _rb2d.velocity = _velocity;
    }

    public void SetXDirection(float xDirect)
    {
        _xDirection = xDirect;
    }

    public float GetXDirection()
    {
        return _xDirection;
    }

    public bool GetGroundedState()
    {
        return _grounded;
    }

    public bool GetChasingState()
    {
        return _isChasing;
    }

    public void SetMaxSpeed(bool isChasing)
    {
        _isChasing = isChasing;

        if (isChasing)
        {
            _currentMaxSpeed = _chaseMaxSpeed;
        }
        else
        {
            _currentMaxSpeed = _patrolMaxSpeed;
        }

        _accelRate = (_currentMaxSpeed) / _timeToReachMaxSpeed;
        _decelRate = -(_currentMaxSpeed - (_currentMaxSpeed / 2)) / _timeToDecel;
    }
}
