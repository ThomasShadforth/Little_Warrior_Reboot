using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombat : MonoBehaviour
{
    //Temp variable used to test AI attack calls from
    [SerializeField] string _tempAttackName;
    [SerializeField] Transform _hitDetectPoint;
    [SerializeField] float _hitDetectRadius;
    [SerializeField] float _attackCoolTime;
    [SerializeField] Vector2 _knockForce;
    [SerializeField] LayerMask _playerLayer;
    IDamageInterface _aiDamageInterface;

    bool _isAttacking;
    float _attackCoolTimer;

    // Start is called before the first frame update
    void Start()
    {
        _aiDamageInterface = GetComponent<IDamageInterface>();
        _attackCoolTimer = _attackCoolTime;
    }

    public void StartAttack()
    {
        GetComponent<Animator>().Play(_tempAttackName);
    }

    public void AttackHitDetect()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_hitDetectPoint.position, _hitDetectRadius);

        foreach(Collider2D hitObject in hitObjects)
        {
            IDamageInterface damageInterface = hitObject.gameObject.GetComponent<IDamageInterface>();

            //In the case of the AI, only damage if the player is detected (So they don't unintentionally harm other enemies/environmental objects)
            if(damageInterface != null && damageInterface != _aiDamageInterface && hitObject.gameObject.CompareTag("Player"))
            {
                Vector2 knockForce = _knockForce;
                if(hitObject.transform.position.x < transform.position.x)
                {
                    knockForce.x *= -1;
                }

                //To do: Pass in position for purpose of knockback calculation
                damageInterface.DetectHit(20, knockForce, .8f);
            }
        }
    }

    public float GetAttackCooldown()
    {
        return _attackCoolTimer;
    }

    public void SetAttackCooldown()
    {
        _attackCoolTimer = _attackCoolTime;
    }

    public void TickAttackCooldown()
    {
        if(_attackCoolTimer > 0)
        {
            _attackCoolTimer -= GamePause.deltaTime;
        }
    }

    public bool GetIsAttacking()
    {
        return _isAttacking;
    }

    public void CallSetAttack()
    {
        SetIsAttacking(true);
    }

    public void SetIsAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }

    public void ResetAttackAnimation()
    {
        GetComponent<Animator>().Play("Idle");
    }
}
