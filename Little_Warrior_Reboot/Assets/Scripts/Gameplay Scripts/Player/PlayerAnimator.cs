using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    Animator _animator;

    PlayerMovement _playerMove;

    PlayerActionMap _playerInput;

    Rigidbody2D _rb2d;

    float _xDirection;
    [SerializeField]
    float _prevXDirection;

    bool _grounded;
    [SerializeField]
    bool _prevGrounded;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMovement>();
        _playerInput = new PlayerActionMap();
        _playerInput.Player.PlayerMovement.Enable();
        _rb2d = GetComponent<Rigidbody2D>();
        _prevXDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        _grounded = _playerMove.GetGroundedState();

        if(_xDirection != 0)
        {
            _prevXDirection = _xDirection;
        }

        _xDirection = _playerInput.Player.PlayerMovement.ReadValue<float>();

        if (_xDirection != _prevXDirection && _xDirection != 0)
        {
            _SwitchXScale();
        }

        _UpdateAnimation();

        _prevGrounded = _grounded;
        
    }

    void _SwitchXScale()
    {
        Vector3 scalar = transform.localScale;
        scalar.x *= -1;
        transform.localScale = scalar;
    }

    void _UpdateAnimation()
    {
        if (_grounded)
        {
            if (!_prevGrounded)
            {
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isFalling", false);
            }

            if (_xDirection != 0)
            {
                _animator.SetBool("isMoving", true);
            }
            else
            {
                _animator.SetBool("isMoving", false);
            }
        }
        else
        {
            if(_rb2d.velocity.y > 0)
            {
                _animator.SetBool("isJumping", true);
                
            }

            if(_rb2d.velocity.y < 0)
            {
                _animator.SetBool("isFalling", true);
                _animator.SetBool("isJumping", false);
            }
        }
    }
}
