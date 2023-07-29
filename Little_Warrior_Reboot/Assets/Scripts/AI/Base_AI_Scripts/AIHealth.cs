using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Subject, IObserver, IDamageInterface
{
    [SerializeField] Subject _ownerSubject;
    [SerializeField] int _maxHealth;
    [SerializeField] HealthBar _enemyHealthBar;
    [SerializeField] float _deathHitStop;
    [SerializeField] int _expOnDeath;
    HealthSystem _healthSystem;
    AIStatus _aiStatus;

    bool _isDying;

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

    private void OnEnable()
    {
        _isDying = false;

        if (_healthSystem != null)
        {
            _healthSystem.Heal(_maxHealth);
        }
    }

    private void OnDisable()
    {
        if (_isDying)
        {
            FindObjectOfType<PlayerLevelSystem>().IncreaseExp(_expOnDeath);
        }
    }

    void HealthSystem_OnHealthChanged(object obj, System.EventArgs e)
    {
        if(_enemyHealthBar != null)
        {
            _enemyHealthBar.UpdateHealthFillAmount(_healthSystem.GetHealthPercent());
        }

        if (_healthSystem.CheckIsDead())
        {
            _isDying = true;

            HitStop hitStopManager = FindObjectOfType<HitStop>();

            if (hitStopManager != null)
            {
                hitStopManager.StopTime(_deathHitStop);
            }

            
            _NotifyObservers(0);

            if(EnemyObjectPool.instance != null)
            {
                EnemyObjectPool.instance.AddToPool(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnNotify(int damageTaken)
    {
        if (damageTaken == 0) return;

        _healthSystem.Damage(damageTaken);
    }

    public void DetectHit(int damageTaken, Vector2 knockbackForce = default(Vector2), float knockbackDuration = 0)
    {
        _NotifyObservers(damageTaken);

        if (_aiStatus && gameObject.activeInHierarchy)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Animator>().Play("EnemyHurt");
            _aiStatus.SetKnockbackTime(knockbackDuration);
            _aiStatus.SetDisabledHeightMaintenance();
            _aiStatus.SetStatus(StatusEnum.Knockback, true);
            GetComponent<Rigidbody2D>().velocity = knockbackForce;
            
        }

        
    }

    public HealthSystem GetHealthSystem()
    {
        return _healthSystem;
    }
}
