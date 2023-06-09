using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [Header("Slope Detection Config:")]
    [SerializeField] float _slopeCheckDist;
    [SerializeField] LayerMask _whatIsGround;

    Vector2 _velocity;

    bool _grounded;
    bool _prevGrounded;

    Rigidbody2D _rb2d;

    float _playerMoveInput;
    float _speed;
    float _maxSpeed = 8.0f;
    float _timeToReachMaxSpeed = .3f;
    float _timeToDecel = .3f;
    float _accelRate;
    float _decelRate;
    float _friction = 2.2f;

    PlayerActionMap _playerInput;

    PlayerHeightMaintenance _heightMaintenance;
    PlayerSprint _playerSprint;
    PlayerCombat _playerCombat;
    PlayerStatus _playerStatus;
    PlayerJump _playerJump;

    SlopeDetection _slopeDetection;
    CapsuleCollider2D _capsuleCollider;

    int _testDeathCount;

    private void Awake()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        if(_playerInput != null)
        {
            _playerInput.Dispose();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _heightMaintenance = GetComponent<PlayerHeightMaintenance>();
        _playerSprint = GetComponent<PlayerSprint>();
        _playerCombat = GetComponent<PlayerCombat>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerJump = GetComponent<PlayerJump>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _slopeDetection = new SlopeDetection(this.transform, _capsuleCollider.size, _slopeCheckDist, _whatIsGround);
        SetMaxSpeed(_maxSpeed);
        //_accelerationComponent = new PlayerAcceleration(_maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.paused)
        {
            if (!_rb2d.IsSleeping())
            {
                _rb2d.Sleep();
            }

            return;
        }

        if (_rb2d.IsSleeping())
        {
            _rb2d.WakeUp();
        }
        

        _playerMoveInput = _playerInput.Player.PlayerMovement.ReadValue<float>();
    }

    void FixedUpdate()
    {
        if (GamePause.paused)
        {
            _rb2d.velocity = Vector2.zero;
            return;
        }

        if ((_playerCombat && _playerCombat.GetIsAttacking()) || (_playerStatus && _playerStatus.CheckForStatus()))
        {
            return;
        }

        _testDeathCount += 1;

        _grounded = _heightMaintenance.GetGrounded();

        //Debug.Log(_grounded);

        if (_grounded)
        {
            //Checks if the player wasn't grounded on the previous frame. Will play a particle or squash & stretch effect to emphasise the movement
            if (!_prevGrounded)
            {
                //Debug.Log("GROUNDED NOW");
                _rb2d.velocity = new Vector2(_rb2d.velocity.x, 0);
            }
        }

        _velocity = _rb2d.velocity;

        //Experiment: Original Little Warrior Movement Physics (For running at least)
        
        if (_playerMoveInput < -.1f)
        {
            if (_speed > 0)
            {
                _speed += _decelRate * 1.9f * GamePause.deltaTime;

                if (_speed < 0)
                {
                    _speed = -.5f;
                }
            } else if (_speed > -_maxSpeed)
            {
                _speed -= _accelRate * GamePause.deltaTime;

                if (_speed < -_maxSpeed) _speed = -_maxSpeed;

            }
        } else if (_playerMoveInput > .1f)
        {
            if(_speed < 0)
            {
                _speed -= _decelRate * 1.9f * GamePause.deltaTime;

                if(_speed > 0) _speed = .5f;

            } else if(_speed < _maxSpeed)
            {
                _speed += _accelRate * GamePause.deltaTime;

                if (_speed > _maxSpeed) _speed = _maxSpeed;
            }
        } else if(_playerMoveInput == 0)
        {
            _speed -= (Mathf.Min(Mathf.Abs(_speed * 2.2f), _friction * 2.2f) * Mathf.Sign(_speed) * _maxSpeed * GamePause.deltaTime);
            
        }
        
        float extraSpeed = 0;

        if(_playerSprint != null)
        {
            extraSpeed = _playerSprint.GetSprintSpeed();
        }

        if(_slopeDetection != null)
        {
            _slopeDetection.CheckSlope();
        }

        

        if((_heightMaintenance.GetGrounded() && !_slopeDetection.GetIsOnSlope()) || !_heightMaintenance.GetGrounded())
        {
            _velocity = new Vector2((_speed + extraSpeed), _rb2d.velocity.y);
        } else if(_heightMaintenance.GetGrounded() && _slopeDetection.GetIsOnSlope() && !_playerJump.GetIsJumping())
        {
            _velocity = new Vector2(Mathf.Abs(_speed + extraSpeed) * _slopeDetection.GetSlopeNormalPerpendicular().x * -_playerMoveInput,
                Mathf.Abs(_speed + extraSpeed) * _slopeDetection.GetSlopeNormalPerpendicular().y * -_playerMoveInput);
        }

        _prevGrounded = _grounded;
        _rb2d.velocity = _velocity;

    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        _maxSpeed = newMaxSpeed;
        _accelRate = (_maxSpeed) / _timeToReachMaxSpeed;
        _decelRate = -(_maxSpeed - (_maxSpeed / 2)) / _timeToDecel;
    }

    public bool GetGroundedState()
    {
        return _grounded;
    }

    public void SaveData(GameData data)
    {
        
        data.fixedUpdateCount = _testDeathCount;

        data.playerPosition = this.transform.position;
        Debug.Log("Saved fixed update count: " + data.fixedUpdateCount);
        Debug.Log("Saved player position: " + data.playerPosition);
    }

    public void LoadData(GameData data)
    {
        Debug.Log("Loaded fixed update count: " + data.fixedUpdateCount);
        Debug.Log("Loaded player position" + data.playerPosition);
        this._testDeathCount = data.fixedUpdateCount;
        
        if (GameManager.instance != null)
        {
            if (GameManager.instance.GetCurrentScene() == GameManager.instance.GetSceneManagerScene() || GameManager.instance.GetCurrentScene() == "")
            {
                Debug.Log("LOADING PREVIOUS POSITION");
                this.transform.position = data.playerPosition;
            }
        }
        
    }
}
