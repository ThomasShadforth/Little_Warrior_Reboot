using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerHeightMaintenance : MonoBehaviour
{
    Vector2 _rayDir = Vector2.down;
    Vector2 _gravForce;

    Rigidbody2D _rb2d;

    [Header("Floating Capsule Config:")]
    [SerializeField] LayerMask _whatIsGround;
    [SerializeField] float _rayToGroundLength;
    [SerializeField] float _rideHeight;
    [SerializeField] float _rideHeightMultiplier;
    [SerializeField] float _springStrength;
    [SerializeField] float _springDamp;
    [SerializeField] bool _shouldMaintainHeight = true;

    //experimental - Slope check variables
    [SerializeField] private float _slopeCheckDistance;
    private CapsuleCollider2D _capsuleCollider;
    private Vector2 _colliderSize;
    private Vector2 _slopeNormalPerpendicular;
    private float _slopeDownAngle;
    private float _slopeSideAngle;
    private float _slopeDownAngleOld;
    private bool _isOnSlope;

    bool _grounded;
    PlayerCombat _playerCombat;
    PlayerStatus _playerStatus;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _gravForce = Physics2D.gravity * _rb2d.mass;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerCombat = GetComponent<PlayerCombat>();
        _playerStatus = GetComponent<PlayerStatus>();

        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _colliderSize = _capsuleCollider.size;
    }

    private void FixedUpdate()
    {
        if((_playerCombat && _playerCombat.GetIsAttacking() && _playerCombat.GetAirAttack()) || (_playerStatus && _playerStatus.CheckForStatus()))
        {
            return;
        }

        CheckForGrounded();
        _CheckSlope();
    }

    void _CheckSlope()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, (_colliderSize.y / 2));

        _VertSlopeCheck(checkPos);
        _HorSlopeCheck(checkPos);
    }

    void _HorSlopeCheck(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, _slopeCheckDistance, _whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, _slopeCheckDistance, _whatIsGround);

        if (slopeHitFront)
        {
            //Debug.Log("SLOPE HIT FROM FRONT");
            _isOnSlope = true;
            _slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        } else if (slopeHitBack)
        {
            _isOnSlope = true;
            _slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            _slopeSideAngle = 0.0f;
            _isOnSlope = false;
        }
    }

    void _VertSlopeCheck(Vector2 checkPos)
    {
        RaycastHit2D slopeHit = Physics2D.Raycast(checkPos, Vector2.down, _slopeCheckDistance, _whatIsGround);
        if (slopeHit)
        {
            //Debug.Log("SLOPE IS HIT VERT");
            _slopeNormalPerpendicular = Vector2.Perpendicular(slopeHit.normal).normalized;

            _slopeDownAngle = Vector2.Angle(slopeHit.normal, Vector2.up);

            if(_slopeDownAngle != _slopeDownAngleOld)
            {
                _isOnSlope = true;
            }

            _slopeDownAngleOld = _slopeDownAngle;

            Debug.DrawRay(slopeHit.point, _slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(slopeHit.point, slopeHit.normal, Color.green);
        }
    }

    public void SetMaintainHeight(bool shouldMaintainHeight)
    {
        //Debug.Log(shouldMaintainHeight);

        _shouldMaintainHeight = shouldMaintainHeight;
    }

    public Vector2 GetSlopeNormalPerpendicular()
    {
        return _slopeNormalPerpendicular;
    }

    public bool GetGrounded()
    {
        return _grounded;
    }

    public bool GetOnSlope()
    {
        return _isOnSlope;
    }

    public void CheckForGrounded()
    {
        Vector2 velocity = _rb2d.velocity;
        
        (bool rayHitGround, RaycastHit2D hit) = _RaycastToGround();

        _grounded = _CheckGrounded(rayHitGround, hit);

        if(rayHitGround && _shouldMaintainHeight)
        {
            
            velocity.y = 0;
            _MaintainHeight(hit);
        }

        _rb2d.velocity = velocity;
    }

    private bool _CheckGrounded(bool rayHitGround, RaycastHit2D hit)
    {
        bool grounded = false;

        if (rayHitGround)
        {
            grounded = hit.distance < _rideHeight * _rideHeightMultiplier;
        }
        else
        {
            grounded = false;
        }

        return grounded;
    }

    private (bool rayHitGround, RaycastHit2D hit) _RaycastToGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _rayToGroundLength, _whatIsGround);
        Debug.DrawRay(transform.position, Vector2.down * _rayToGroundLength, Color.red);
        bool rayHitGround = Physics2D.Raycast(transform.position, Vector2.down, _rayToGroundLength, _whatIsGround);

        return (rayHitGround, hit);
    }

    private void _MaintainHeight(RaycastHit2D hit)
    {
        Vector2 velocity = _rb2d.velocity;

        Vector2 otherVelocity = Vector2.zero;

        Rigidbody2D hitRb2d = hit.rigidbody;

        if (hitRb2d != null)
        {
            //Set the other velocity to be that of the hit rigidody (If it has it)
        }

        float rayDirVelocity = Vector2.Dot(_rayDir, velocity);
        float otherDirVelocity = Vector2.Dot(_rayDir, otherVelocity);

        float relativeVelocity = rayDirVelocity - otherDirVelocity; ;
        float currentHeight = hit.distance - _rideHeight;
        float springForce = (currentHeight * _springStrength) - (relativeVelocity * _springDamp);

        Vector2 maintainHeightForce = -_gravForce + springForce * Vector2.down;

        _rb2d.AddForce(maintainHeightForce);

        if (hitRb2d != null)
        {

        }
    }
}
