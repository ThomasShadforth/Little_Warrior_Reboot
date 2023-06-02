using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineOfSight2D))]
public class AITurretData : MonoBehaviour
{
    [SerializeField] float _maxRotationAngle;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _turretWaitTime;
    [SerializeField] float _attackWaitTime;
    [SerializeField] LayerMask _ignoreLayer;

    //Experimental: Testing linerenderer compatibility
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] GameObject _fovLight;

    Transform _playerTarget;

    Vector3 _lastPlayerLocation;

    LineOfSight2D _enemyLos;
    float _totalRotation;
    float _turretWaitTimer;
    float _attackWaitTimer;
    bool _isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        _enemyLos = GetComponent<LineOfSight2D>();
        SetWaitTime();
        SetAttackWaitTime();
    }

    private void Update()
    {
        if(_enemyLos.GetCanSeePlayer() && _playerTarget == null)
        {
            _playerTarget = _enemyLos.GetPlayer().transform;
        }

        else if(!_enemyLos.GetCanSeePlayer() && _playerTarget != null)
        {
            _playerTarget = null;
        }
    }

    // Update is called once per frame

    public void RotateTurret()
    {

        transform.localEulerAngles += new Vector3(0, 0, _rotationSpeed * GamePause.deltaTime);
        _totalRotation += Mathf.Abs(_rotationSpeed) * GamePause.deltaTime;
    }

    public void SetNewRotation()
    {
        float rotationDifference = _totalRotation - _maxRotationAngle;

        float currentZRot = transform.localEulerAngles.z;

        currentZRot -= rotationDifference;
        currentZRot = Mathf.Round(currentZRot);

        transform.localEulerAngles = new Vector3(0, 0, currentZRot);

        _totalRotation = 0;
        _rotationSpeed = -_rotationSpeed;
    }

    public void SetWaitTime()
    {
        _turretWaitTimer = _turretWaitTime;
    }

    public void TickWaitTimer()
    {
        _turretWaitTimer -= GamePause.deltaTime;
    }

    public void SetLastPlayerPosition()
    {
        _lastPlayerLocation = _playerTarget.position;
    }

    public void SetAttackWaitTime()
    {
        _isAttacking = false;
        _attackWaitTimer = _attackWaitTime;
    }

    public void TickAttackWaitTimer()
    {
        //Debug.Log(_attackWaitTimer);
        _attackWaitTimer -= GamePause.deltaTime;
    }

    public void Attack()
    {
        _isAttacking = true;
        SetLastPlayerPosition();
    }

    public void FireLaser()
    {
        Vector3 directionToTarget = (_lastPlayerLocation - transform.position).normalized;

        float distanceToLastTarget = Vector3.Distance(transform.position, _lastPlayerLocation);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionToTarget, distanceToLastTarget, ~_ignoreLayer);

        if (hits.Length != 0)
        {
            Debug.Log(hits[0].collider.gameObject.name);

            foreach (RaycastHit2D hit in hits)
            {
                Debug.DrawRay(hit.point, hit.normal * 40, Color.red);
                IDamageInterface damagedObject = hit.collider.gameObject.GetComponent<IDamageInterface>();
                if (damagedObject != null)
                {
                    damagedObject.DetectHit(10, default, 0);
                }

            }

            if (ExplosionObjectPool.instance != null)
            {
                GameObject explosionObject = ExplosionObjectPool.instance.GetFromPool();
                explosionObject.transform.position = hits[0].point;
            }

        }
        else
        {
            Debug.Log("AAAAAAAA");
        }

        
    }

    public void TrackPlayer()
    {
        _enemyLos.SetLastPosition(_enemyLos.GetPlayer().transform.position);
    }

    public void SetLaserActive(bool isActive = false)
    {
        _lineRenderer.enabled = isActive;
        _fovLight.SetActive(!isActive);
    }

    public void SetLaserLine()
    {
        if(_lineRenderer != null)
        {
            _lineRenderer.SetPositions(new Vector3[] { transform.position, _enemyLos.GetLastPlayerPosition() });
            _lineRenderer.startColor = Color.Lerp(Color.green, Color.red, _attackWaitTimer / _attackWaitTime);
            _lineRenderer.endColor = Color.Lerp(Color.green, Color.red, _attackWaitTimer / _attackWaitTime);
        }
    }

    public bool GetWaitTimerEnded()
    {
        return _turretWaitTimer <= 0;
    }

    public bool GetAttackWaitTimerEnded()
    {
        return _attackWaitTimer <= 0;
    }

    public bool GetRotationExceeded()
    {
        return _totalRotation >= _maxRotationAngle;
    }

    public bool GetIsAttacking()
    {
        return _isAttacking;
    }

    public bool GetCanSeePlayer()
    {
        return _enemyLos.GetCanSeePlayer();
    }
}
