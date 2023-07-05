using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    [SerializeField] string _attackName;
    [SerializeField] string _nextLightAttack;
    [SerializeField] string _nextHeavyAttack;
    //To Do: Insert Heavy and Down attacks.
    [SerializeField] bool _isUnlocked;
    [SerializeField] bool _defaultMove;
    [SerializeField] int _damageDealt;
    //Test: Knockback multiplier vectors for specific attacks (E.g. Uppercut)
    [SerializeField] Vector2 _attackKnockbackMinimum;
    [SerializeField] Vector2 _attackKnockbackMaximum;
    [SerializeField] float _knockbackDuration;
    [SerializeField] float _camShakeIntensity;
    [SerializeField] float _camShakeTime;
    //Test: Force Multipliers for attack movement
    [SerializeField] Vector2 _attackMoveForce;
    [SerializeField] PlayerSkillManager.AbilityType _abilityType;

    [SerializeField] string _playedSFXName;

    public string GetAttackName()
    {
        return _attackName;
    }

    public string GetNextLightAttack()
    {
        return _nextLightAttack;
    }
    
    public string GetNextHeavyAttack()
    {
        return _nextHeavyAttack;
    }

    public int GetDamageDealt()
    {
        return _damageDealt;
    }

    public bool GetUnlockStatus()
    {
        return _isUnlocked;
    }

    public bool GetDefaultStatus()
    {
        return _defaultMove;
    }

    public float GetKnockbackDuration()
    {
        return _knockbackDuration;
    }

    public float GetCamShakeIntensity()
    {
        return _camShakeIntensity;
    }

    public float GetCamShakeTime()
    {
        return _camShakeTime;
    }

    public string GetSFXName()
    {
        return _playedSFXName;
    }

    public Vector2 GetMovementForce()
    {
        return _attackMoveForce;
    }

    public Vector2 GetAttackKnockback()
    {
        return _attackKnockbackMinimum;
    }

    public Vector2 GetRandomKnockback()
    {
        Vector2 randomKnockback;
        randomKnockback.x = Random.Range(_attackKnockbackMinimum.x, _attackKnockbackMaximum.x);
        randomKnockback.y = Random.Range(_attackKnockbackMinimum.y, _attackKnockbackMaximum.y);

        return randomKnockback;
    }

    public PlayerSkillManager.AbilityType GetAbilityType()
    {
        return _abilityType;
    }

    public void SetUnlockStatus(bool willUnlock)
    {
        _isUnlocked = willUnlock;
    }
}
