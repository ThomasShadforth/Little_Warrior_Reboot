using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Subject, IObserver, IDamageInterface
{
    [SerializeField] Subject _ownerSubject;
    [SerializeField] int _maxHealth;
    [SerializeField] HealthBar _playerHealthBar;

    HealthSystem _healthSystem;


    // Start is called before the first frame update
    void Start()
    {
        _healthSystem = new HealthSystem(_maxHealth);
        _healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        _ownerSubject = this;
        _ownerSubject.AddObserver(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void HealthSystem_OnHealthChanged(object obj, System.EventArgs e)
    {
        //Call when player health changes (on hit or heal)
        if(_playerHealthBar != null)
        {
            _playerHealthBar.UpdateHealthFillAmount(_healthSystem.GetHealthPercent());
        }

        Debug.Log(_healthSystem.GetHealth());

        if (_healthSystem.CheckIsDead())
        {
            //Temp: Restart the current level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            //Game over
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
