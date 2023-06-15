using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineOfSight2D))]
public class AITurretData : MonoBehaviour
{
    [SerializeField] float _maxRotationAngle;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _turretWaitTime;

    //Experimental: Testing linerenderer compatibility
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] GameObject _fovLight;

    Transform _playerTarget;

    LineOfSight2D _enemyLos;
    float _totalRotation;
    float _turretWaitTimer;

    // Start is called before the first frame update
    void Start()
    {
        _enemyLos = GetComponent<LineOfSight2D>();
        SetWaitTime();
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
    
    public void TrackPlayer()
    {
        _enemyLos.SetLastPosition(_enemyLos.GetPlayer().transform.position);
    }

    public void SetLaserActive(bool isActive = false)
    {
        GetComponent<AITurretCombat>().SetLaserLineStatus(isActive);
        _fovLight.SetActive(!isActive);
    }

    

    public Transform GetTargetTransform()
    {
        return _playerTarget;
    }

    public bool GetLessThanMaxWaitTime()
    {
        return _turretWaitTimer < _turretWaitTime;
    }

    public bool GetWaitTimerEnded()
    {
        return _turretWaitTimer <= 0;
    }

    public bool GetRotationExceeded()
    {
        return _totalRotation >= _maxRotationAngle;
    }

    public bool GetCanSeePlayer()
    {
        return _enemyLos.GetCanSeePlayer();
    }
}
