using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] AttackData[] _playerLightAttacks;
    [SerializeField] AttackData[] _playerHeavyAttacks;
    AttackData _currentAttack;
    [SerializeField] Transform _hitDetectPoint;
    [SerializeField] float _hitDetectRadius;
    IDamageInterface _playerDamageInterface;
    bool _isAttacking;
    bool _airAttack;
    PlayerHeightMaintenance _playerHeight;


    void Start()
    {
        _playerDamageInterface = GetComponent<IDamageInterface>();
        _playerHeight = GetComponent<PlayerHeightMaintenance>();
    }

    public void UnlockAttack(string attackName)
    {
        foreach(AttackData attack in _playerLightAttacks)
        {
            if(attack.GetAttackName() == attackName)
            {
                attack.SetUnlockStatus(true);
                return;
            }
        }

        foreach(AttackData attack in _playerHeavyAttacks)
        {
            if(attack.GetAttackName() == attackName)
            {
                attack.SetUnlockStatus(true);
                return;
            }
        }
    }

    public void AttackHitDetect()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_hitDetectPoint.position, _hitDetectRadius);

        foreach(Collider2D hitObject in hitObjects)
        {
            IDamageInterface damageInterface = hitObject.GetComponent<IDamageInterface>();

            if(damageInterface != null && damageInterface != _playerDamageInterface)
            {
                Vector2 knockForce = _currentAttack.GetAttackKnockback();

                if(hitObject.transform.position.x < transform.position.x)
                {
                    knockForce.x *= -1;
                }

                damageInterface.DetectHit(_currentAttack.GetDamageDealt(), knockForce, _currentAttack.GetKnockbackDuration());
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

    
    public void TriggerAttack(string attackType)
    {
        if(attackType == "Light")
        {
            if(_currentAttack == null)
            {
                if (_playerLightAttacks[0].GetUnlockStatus())
                {
                    _currentAttack = _playerLightAttacks[0];
                    GetComponent<Animator>().Play(_currentAttack.GetAttackName());
                }
            }
            else
            {
                if(_currentAttack.GetNextLightAttack() != "")
                {
                    _SearchForAttack(_currentAttack.GetNextLightAttack());
                } else if(_currentAttack.GetNextHeavyAttack() != "")
                {
                    _SearchForAttack(_currentAttack.GetNextHeavyAttack());
                }
            }
            
        } else if(attackType == "Heavy")
        {
            if (_currentAttack == null)
            {
                if (_playerHeavyAttacks[0].GetUnlockStatus())
                {
                    _currentAttack = _playerHeavyAttacks[0];
                    GetComponent<Animator>().Play(_currentAttack.GetAttackName());
                }
            }
            else
            {
                if (_currentAttack.GetNextLightAttack() != "")
                {
                    _SearchForAttack(_currentAttack.GetNextLightAttack());
                }
                else if (_currentAttack.GetNextHeavyAttack() != "")
                {
                    _SearchForAttack(_currentAttack.GetNextHeavyAttack());
                }
            }
        }
    }


    void _SearchForAttack(string nextAttackName)
    {
        foreach(AttackData attack in _playerLightAttacks)
        {
            if(attack.GetAttackName() == nextAttackName)
            {
                if (attack.GetUnlockStatus())
                {
                    _currentAttack = attack;
                    GetComponent<Animator>().Play(_currentAttack.GetAttackName());
                }
                return;
            }
        }

        foreach(AttackData attack in _playerHeavyAttacks)
        {
            if(attack.GetAttackName() == nextAttackName)
            {
                if (attack.GetUnlockStatus())
                {
                    _currentAttack = attack;
                    GetComponent<Animator>().Play(_currentAttack.GetAttackName());
                }
                return;
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

}
