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
    }

    private void FixedUpdate()
    {
        if((_playerCombat && _playerCombat.GetIsAttacking() && _playerCombat.GetAirAttack()) || (_playerStatus && _playerStatus.GetDisabledHeightMaintenance()))
        {
            return;
        }

        CheckForGrounded();
    }

    public void SetMaintainHeight(bool shouldMaintainHeight)
    {
        _shouldMaintainHeight = shouldMaintainHeight;
    }

    public bool GetGrounded()
    {
        return _grounded;
    }

    public void CheckForGrounded()
    {
        Vector2 velocity = _rb2d.velocity;
        
        (bool rayHitGround, RaycastHit2D hit) = _RaycastToGround();

        
        _CheckHazards(hit);

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

    private void _CheckHazards(RaycastHit2D hit)
    {
        if (hit)
        {
            //Debug.Log(hit.collider.gameObject);
            LevelHazard hazard = hit.collider.gameObject.GetComponent<LevelHazard>();

            if(hazard != null && !_playerStatus.GetInvincibility())
            {
                Debug.Log("HAZARD IS DETECTED");
                _playerStatus.SetInvincibility();

                IDamageInterface damageInterface = GetComponent<IDamageInterface>();

                if(damageInterface != null)
                {
                    Debug.Log("DAMAGE SHOULD TAKE PLACE");
                    damageInterface.DetectHit(10, new Vector2(-400000, -40), 2f);
                }
            }

        }
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
