using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimator : MonoBehaviour
{
    Animator _animator;
    AIMovement _aiMove;

    float _xDirection;
    float _prevXDirection;

    bool _grounded;
    bool _prevGrounded;
    

    // Start is called before the first frame update
    void Start()
    {
        _prevXDirection = 1;
        _animator = GetComponent<Animator>();
        _aiMove = GetComponent<AIMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _grounded = _aiMove.GetGroundedState();

        if(_xDirection != 0)
        {
            _prevXDirection = _xDirection;
        }

        _xDirection = _aiMove.GetXDirection();

        if(_xDirection != _prevXDirection && _xDirection != 0)
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
            if(_xDirection != 0)
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
            //Insert jump checks if the enemy ever receives a jump
        }
    }
}
