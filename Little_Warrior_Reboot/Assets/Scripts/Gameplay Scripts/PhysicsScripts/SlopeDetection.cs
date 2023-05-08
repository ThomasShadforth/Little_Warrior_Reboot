using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDetection
{
    Transform _ownerTransform;
    Vector2 _ownerColliderSize;
    Vector2 _slopeNormalPerp;
    float _slopeCheckDistance;
    float _slopeDownAngle;
    float _slopeDownAngleOld;
    float _slopeSideAngle;
    bool _isOnSlope;
    LayerMask _whatIsGround;

    public SlopeDetection(Transform ownerTransform, Vector2 colliderSize, float slopeCheckDist, LayerMask whatIsGround)
    {
        _ownerTransform = ownerTransform;
        _ownerColliderSize = colliderSize;
        _slopeCheckDistance = slopeCheckDist;
        _whatIsGround = whatIsGround;
    }

    public bool GetIsOnSlope()
    {
        return _isOnSlope;
    }

    public Vector2 GetSlopeNormalPerpendicular()
    {
        return Vector2.zero;
    }

    public void CheckSlope()
    {
        Vector2 checkPos = _ownerTransform.position - new Vector3(0.0f, _ownerColliderSize.y / 2);
        _CheckSlopeVert(checkPos);
        _CheckSlopeHor(checkPos);
    }

    void _CheckSlopeVert(Vector2 checkPos)
    {
        RaycastHit2D slopeHit = Physics2D.Raycast(checkPos, Vector2.down, _slopeCheckDistance, _whatIsGround);

        if (slopeHit)
        {
            _slopeNormalPerp = Vector2.Perpendicular(slopeHit.normal).normalized;

            _slopeDownAngle = Vector2.Angle(slopeHit.normal, Vector2.up);

            if(_slopeDownAngle != _slopeDownAngleOld)
            {
                _isOnSlope = true;
            }

            _slopeDownAngleOld = _slopeDownAngle;
            Debug.DrawRay(slopeHit.point, _slopeNormalPerp, Color.red);
            Debug.DrawRay(slopeHit.point, slopeHit.normal, Color.green);
        }

    }

    void _CheckSlopeHor(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, Vector2.right, _slopeCheckDistance, _whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -Vector2.right, _slopeCheckDistance, _whatIsGround);

        if (slopeHitFront)
        {
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
}
