using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Subject, IObserver, IDamageInterface
{
    [SerializeField] Subject _ownerSubject;
    [SerializeField] int _maxHealth;
    [SerializeField] HealthBar _enemyHealthBar;
    [SerializeField] float _deathHitStop;
    HealthSystem _healthSystem;
    AIStatus _aiStatus;

    // Start is called before the first frame update
    void Start()
    {
        _healthSystem = new HealthSystem(_maxHealth);
        _healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        _ownerSubject = this;
        _ownerSubject.AddObserver(this);
        _aiStatus = GetComponent<AIStatus>();
        //_ownerSubject.GetObserverCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HealthSystem_OnHealthChanged(object obj, System.EventArgs e)
    {
        if(_enemyHealthBar != null)
        {
            _enemyHealthBar.UpdateHealthFillAmount(_healthSystem.GetHealthPercent());
        }

        if (_healthSystem.CheckIsDead())
        {
            FindObjectOfType<HitStop>().StopTime(_deathHitStop);

            if(EnemyObjectPool.instance != null)
            {
                EnemyObjectPool.instance.AddToPool(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }

            //Return enemy to object pool (TODO: Create object pooling scripts)
        }
    }

    public void OnNotify(int damageTaken)
    {
        _healthSystem.Damage(damageTaken);
    }

    public void DetectHit(int damageTaken, Vector2 knockbackForce = default(Vector2), float knockbackDuration = 0)
    {
        _NotifyObservers(damageTaken);

        if (_aiStatus)
        {
            _aiStatus.SetKnockbackTime(knockbackDuration);
            _aiStatus.SetStatus(StatusEnum.Knockback, true);
            GetComponent<Rigidbody2D>().velocity = knockbackForce;
        }

        
    }
}
