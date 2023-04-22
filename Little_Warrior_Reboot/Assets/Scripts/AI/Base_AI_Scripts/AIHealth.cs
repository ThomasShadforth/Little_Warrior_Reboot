using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Subject, IObserver, IDamageInterface
{
    [SerializeField] Subject _ownerSubject;
    [SerializeField] int _maxHealth;
    [SerializeField] HealthBar _enemyHealthBar;
    HealthSystem _healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        _healthSystem = new HealthSystem(_maxHealth);
        _healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        _ownerSubject = this;
        _ownerSubject.AddObserver(this);
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

        Debug.Log(_healthSystem.GetHealth());

        if (_healthSystem.CheckIsDead())
        {
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

    public void DetectHit(int damageTaken, Transform hitOrigin = null)
    {
        _NotifyObservers(damageTaken);
    }
}
