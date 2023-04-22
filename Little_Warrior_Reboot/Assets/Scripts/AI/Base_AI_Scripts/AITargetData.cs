using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetData : MonoBehaviour
{

    [SerializeField] Transform _playerTarget;
    [SerializeField] float _minimumTargetChaseDistance;
    [SerializeField] float _minimumTargetAttackDistance;



    void Start()
    {
        _playerTarget = FindObjectOfType<PlayerMovement>().transform;
    }

    public Transform GetPlayerTarget()
    {
        if (_playerTarget)
        {
            return _playerTarget;
        }
        else
        {
            return null;
        }
    }

    public float GetMinimumTargetDistance()
    {
        return _minimumTargetChaseDistance;
    }

    public float GetMinimumAttackDistance()
    {
        return _minimumTargetAttackDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _minimumTargetChaseDistance);
    }
}
