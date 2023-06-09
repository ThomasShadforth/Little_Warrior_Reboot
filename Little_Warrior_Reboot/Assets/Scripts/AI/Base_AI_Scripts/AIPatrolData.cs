using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolData : MonoBehaviour
{
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] float _minimumPatrolDistance;
    int _currentPatrolIndex;

    [SerializeField] float _waitTime;
    float _waitTimer;

    private void Start()
    {
        SetWaitTimer();
    }

    public Transform[] GetPatrolPoints()
    {
        return _patrolPoints;
    }

    public bool GetPatrolPointsNull()
    {
        if(_patrolPoints == null)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }

    // Start is called before the first frame update
    public int GetPatrolIndex()
    {
        return _currentPatrolIndex;
    }

    public float GetMinimumDistance()
    {
        return _minimumPatrolDistance;
    }

    public bool GetLessThanMaxWaitTime()
    {
        return _waitTimer < _waitTime;
    }

    public void SetPatrolIndex()
    {
        _currentPatrolIndex++;
        if(_currentPatrolIndex == _patrolPoints.Length)
        {
            _currentPatrolIndex = 0;
        }
    }

    public void TickWaitTimer()
    {
        if(_waitTimer > 0)
        {
            _waitTimer -= GamePause.deltaTime;
        }

    }

    public float GetWaitTimer()
    {
        return _waitTimer;
    }

    public void SetWaitTimer()
    {
        _waitTimer = _waitTime;
    }
}
