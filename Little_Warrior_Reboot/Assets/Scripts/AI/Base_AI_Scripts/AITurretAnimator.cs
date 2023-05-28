using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AITurretData))]
public class AITurretAnimator : MonoBehaviour
{
    AITurretData _turretData;
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _turretData = GetComponent<AITurretData>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _AnimateTurret();
    }

    void _AnimateTurret()
    {
        if (_turretData.GetIsAttacking())
        {
            _animator.SetBool("IsAttacking", true);
        }
        else
        {
            _animator.SetBool("IsAttacking", false);
        }
    }
}
