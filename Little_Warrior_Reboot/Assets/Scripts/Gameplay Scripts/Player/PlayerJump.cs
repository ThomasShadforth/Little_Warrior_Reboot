using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Configuration Values:")]
    [SerializeField] float _jumpHeight;
    [SerializeField] float _jumpGravity = 1.7f;
    [SerializeField] float _fallGravity = 3f;
    [SerializeField] float _jumpBuffer = .15f;
    [SerializeField] float _coyoteTime = .25f;
    [SerializeField] int _maxAirJumps;

    Vector2 _velocity;

    bool _desiredJump;
    bool _isJumping;
    int _jumpPhase;
    float _jumpBufferTimer;
    float _coyoteTimer;
    float _jumpFloat;

    bool _holdingJump;

    Rigidbody2D _rb2d;
    PlayerActionMap _playerInput;
    PlayerMovement _playerMove;
    PlayerHeightMaintenance _heightMaintenance;
    PlayerCombat _playerCombat;
    PlayerStatus _playerStatus;

    private void Awake()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.Jump.Enable();
        _playerInput.Player.Jump.started += _JumpInput;
        _playerInput.Player.Jump.canceled += _JumpInput;
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
        _playerMove = GetComponent<PlayerMovement>();
        _heightMaintenance = GetComponent<PlayerHeightMaintenance>();
        _playerCombat = GetComponent<PlayerCombat>();
        _playerStatus = GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        _jumpFloat = _playerInput.Player.Jump.ReadValue<float>();
        
    }

    private void FixedUpdate()
    {
        if((_playerCombat && _playerCombat.GetIsAttacking()) || (_playerStatus && _playerStatus.CheckForStatus()))
        {
            return;
        }

        _velocity = _rb2d.velocity;

        _Jump();

        _rb2d.velocity = new Vector2(_rb2d.velocity.x, _velocity.y);
    }

    public bool GetIsJumping()
    {
        return _isJumping;
    }

    void _Jump()
    {
        if (_playerMove.GetGroundedState() && _rb2d.velocity.y <= 0)
        {
            _coyoteTimer = _coyoteTime;
            _isJumping = false;
            _jumpPhase = 0;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }

        if (_desiredJump)
        {
            _desiredJump = false;
            _jumpBufferTimer = _jumpBuffer;
        } else if(!_desiredJump && _jumpBufferTimer > 0)
        {
            _jumpBufferTimer -= Time.deltaTime;
        }

        if(_jumpBufferTimer > 0)
        {
            _JumpAction();
        }

        if(_holdingJump && _velocity.y > 0)
        {
            _rb2d.gravityScale = _jumpGravity;
            _heightMaintenance.SetMaintainHeight(false);
        }

        if(!_holdingJump || (_velocity.y < 0 && !_playerMove.GetGroundedState()))
        {
            _rb2d.gravityScale = _fallGravity;
            _heightMaintenance.SetMaintainHeight(true);
        }

        if (_playerMove.GetGroundedState())
        {
            
            _rb2d.gravityScale = 1f;
        }
    }

    void _JumpAction()
    {
        
        if(_coyoteTimer > 0f || (_jumpPhase < _maxAirJumps && _isJumping) || (_coyoteTimer < 0 && !_playerMove.GetGroundedState() && _jumpPhase < _maxAirJumps))
        {   
            if (_isJumping || (_coyoteTimer < 0 && !_playerMove.GetGroundedState()))
            {
                _jumpPhase += 1;
            }

            //Debug.Log(_jumpPhase);
            _jumpBufferTimer = 0;
            _coyoteTimer = 0;

            float jumpSpeed = Mathf.Sqrt(-2 * Physics2D.gravity.y * _jumpHeight);
            _isJumping = true;

            if(_velocity.y > 0)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0f);
            } else if(_velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(_rb2d.velocity.y);
            }

            _velocity.y += jumpSpeed;
        }
    }

    void _JumpInput(InputAction.CallbackContext context)
    {
        if (GamePause.paused) return;

        if (context.started)
        {
            _holdingJump = true;
            _desiredJump = true;
            //_heightMaintenance.SetMaintainHeight(false);

        } else if (context.canceled)
        {
            _holdingJump = false;
            //_heightMaintenance.SetMaintainHeight(true);
        }
    }
}
