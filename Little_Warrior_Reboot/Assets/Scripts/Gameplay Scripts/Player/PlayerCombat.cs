using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour, IDataPersistence
{
    [SerializeField] AttackData[] _playerLightAttacks;
    [SerializeField] AttackData[] _playerHeavyAttacks;
    AttackData _currentAttack;
    [SerializeField] Transform _hitDetectPoint;
    [SerializeField] float _hitDetectRadius;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] LayerMask _ignoreLayer;


    IDamageInterface _playerDamageInterface;
    bool _isAttacking;
    bool _airAttack;
    PlayerHeightMaintenance _playerHeight;
    PlayerAbilities _playerAbilities;
    

    void Start()
    {
        _playerDamageInterface = GetComponent<IDamageInterface>();
        _playerHeight = GetComponent<PlayerHeightMaintenance>();
        _playerAbilities = GetComponent<PlayerAbilities>();
        _SetupDefaultSkills();
    }

    private void _SetupDefaultSkills()
    {
        for(int i = 0; i < _playerLightAttacks.Length; i++)
        {
            if (_playerLightAttacks[i].GetDefaultStatus()) {
                _playerAbilities.UnlockDefaultAbility(_playerLightAttacks[i].GetAbilityType());
                _playerLightAttacks[i].SetUnlockStatus(true);
            
            }
        }

        for(int i = 0; i < _playerHeavyAttacks.Length; i++)
        {
            if (_playerHeavyAttacks[i].GetDefaultStatus()) {
                _playerAbilities.UnlockDefaultAbility(_playerHeavyAttacks[i].GetAbilityType());
                _playerHeavyAttacks[i].SetUnlockStatus(true);
            } 
        }
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
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_hitDetectPoint.position, _hitDetectRadius, ~_ignoreLayer);

        if (hitObjects.Length != 0)
        {
            foreach (Collider2D hitObject in hitObjects)
            {
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
                if (_currentAttack.GetNextHeavyAttack() != "")
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

    public void LoadData(GameData data)
    {
        if(data.playerLightAttacks.Count != 0)
        {
            AttackData[] loadedLightAttacks = data.playerLightAttacks.ToArray();

            for(int i = 0; i < _playerLightAttacks.Length; i++)
            {
                _playerLightAttacks[i].SetUnlockStatus(loadedLightAttacks[i].GetUnlockStatus());
            }
        }

        if(data.playerHeavyAttacks.Count != 0)
        {
            AttackData[] loadedHeavyAttacks = data.playerHeavyAttacks.ToArray();

            for(int i = 0; i < _playerHeavyAttacks.Length; i++)
            {
                _playerHeavyAttacks[i].SetUnlockStatus(loadedHeavyAttacks[i].GetUnlockStatus());
            }
        }
    }

    public void SaveData(GameData data)
    {
        data.playerLightAttacks = _playerLightAttacks.ToList();
        data.playerHeavyAttacks = _playerHeavyAttacks.ToList();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_hitDetectPoint.position, _hitDetectRadius);
    }
}
