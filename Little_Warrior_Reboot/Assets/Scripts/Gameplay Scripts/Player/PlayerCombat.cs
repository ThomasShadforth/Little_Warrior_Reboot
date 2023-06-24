using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    AttackData _currentAttack;
    [SerializeField] Transform _hitDetectPoint;
    [SerializeField] float _hitDetectRadius;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] LayerMask _ignoreLayer;

    IDamageInterface _playerDamageInterface;
    bool _isAttacking;
    bool _airAttack;
    PlayerHeightMaintenance _playerHeight;
    PlayerAttackInfo _playerAttackInfo;

    void Start()
    {
        _playerDamageInterface = GetComponent<IDamageInterface>();
        _playerHeight = GetComponent<PlayerHeightMaintenance>();
        _playerAttackInfo = GetComponent<PlayerAttackInfo>();
    }

    public void AttackHitDetect()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_hitDetectPoint.position, _hitDetectRadius, ~_ignoreLayer);

        if (hitObjects.Length != 0)
        {
            foreach (Collider2D hitObject in hitObjects)
            {
                //Debug.Log(hitObject.gameObject.name);

                IDamageInterface damageInterface = hitObject.GetComponent<IDamageInterface>();

                if (damageInterface != null && damageInterface != _playerDamageInterface)
                {
                    Vector2 knockForce = _currentAttack.GetAttackKnockback();

                    if (hitObject.transform.position.x < transform.position.x)
                    {
                        knockForce.x *= -1;
                    }

                    damageInterface.DetectHit(_currentAttack.GetDamageDealt(), knockForce, _currentAttack.GetKnockbackDuration());
                }

            }

            StartCoroutine(CinemachineCameraShake.StartCamShakeCo(_currentAttack.GetCamShakeIntensity(), _currentAttack.GetCamShakeTime(), FindObjectOfType<CinemachineVirtualCamera>()));

            if(AudioManager.instance != null)
            {
                AudioManager.instance.Play(_currentAttack.GetSFXName());
            }

        }
    }

    public void ResetAttackAnimation()
    {
        GetComponent<Animator>().Play("Idle");
        _isAttacking = false;
        _airAttack = false;
        _currentAttack = null;
    }

    public void ApplyMoveForce()
    {
        Vector2 moveForce = _currentAttack.GetMovementForce();
        moveForce.x *= transform.localScale.x;

        if (moveForce.y > 0) {
            _playerHeight.SetMaintainHeight(false);
            GetComponent<Rigidbody2D>().gravityScale = 1.7f;
            _airAttack = true;
        }

        GetComponent<Rigidbody2D>().velocity = moveForce;
        
    }
    
    public void StopAttack()
    {
        _isAttacking = false;
        _airAttack = false;
        _currentAttack = null;
    }

    public void TriggerAttack(string attackType)
    {
        if(attackType == "Light")
        {
            if(_currentAttack == null)
            {
                _currentAttack = _playerAttackInfo.SearchForLightAttack();

                if (_currentAttack != null && _currentAttack.GetUnlockStatus())
                {
                    GetComponent<Animator>().Play(_currentAttack.GetAttackName());
                }
            }
            else
            {
                if(_currentAttack.GetNextLightAttack() != "")
                {
                    _currentAttack = _playerAttackInfo.SearchForLightAttack(_currentAttack.GetNextLightAttack());
                    if(_currentAttack != null)
                    {
                        GetComponent<Animator>().Play(_currentAttack.GetAttackName());
                    }
                }
            }
            
        } else if(attackType == "Heavy")
        {
            if (_currentAttack != null)
            {
                if(_currentAttack.GetNextHeavyAttack() != "")
                {
                    _currentAttack = _playerAttackInfo.SearchForHeavyAttack(_currentAttack.GetNextHeavyAttack());

                    if(_currentAttack != null)
                    {
                        GetComponent<Animator>().Play(_currentAttack.GetAttackName());
                    }
                }
            }
        }
    }

    public bool GetAirAttack()
    {
        return _airAttack;
    }

    public bool GetIsAttacking()
    {
        return _isAttacking;
    }

    public void SetIsAttacking()
    {
        _isAttacking = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_hitDetectPoint.position, _hitDetectRadius);
    }


}
