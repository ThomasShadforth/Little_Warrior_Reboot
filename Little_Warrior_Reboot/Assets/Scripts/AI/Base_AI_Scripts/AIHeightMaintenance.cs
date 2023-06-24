using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIMovement))]
public class AIHeightMaintenance : MonoBehaviour
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

    AIStatus _aiStatus;

    bool _grounded;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _gravForce = Physics2D.gravity * _rb2d.mass;
    }

    // Start is called before the first frame update
    void Start()
    {
        _aiStatus = GetComponent<AIStatus>();
    }

    private void FixedUpdate()
    {
        if(_aiStatus && _aiStatus.GetDisabledHeightMaintenance())
        {
            return;
        }

        CheckForGrounded();

    }

    public bool GetGrounded()
    {
        return _grounded;
    }

    public void SetMaintainHeight(bool shouldMaintainHeight)
    {
        _shouldMaintainHeight = shouldMaintainHeight;
    }

    public void CheckForGrounded()
    {
        (bool rayHitGround, RaycastHit2D hit) = _RaycastToGround();

        _SetPlatform(hit);

        _grounded = _CheckGrounded(rayHitGround, hit);

        if (rayHitGround && _shouldMaintainHeight)
        {
            _MaintainHeight(hit);
        }
    }

    private bool _CheckGrounded(bool rayHitGround, RaycastHit2D hit)
    {
        bool grounded = false;

        if (rayHitGround)
        {
            grounded = hit.distance < _rideHeight * 1.3f;
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

    private void _SetPlatform(RaycastHit2D hit)
    {
        try
        {
            Platform platformParent = hit.collider.gameObject.GetComponent<Platform>();
            transform.parent = platformParent.transform;
            platformParent.CheckMovementCondition();
        }
        catch
        {
            transform.parent = null;
        }
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
