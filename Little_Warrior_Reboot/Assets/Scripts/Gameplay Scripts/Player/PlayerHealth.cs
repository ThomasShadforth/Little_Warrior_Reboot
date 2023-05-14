using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerHealth : Subject, IObserver, IDamageInterface
{
    [SerializeField] Subject _ownerSubject;
    [SerializeField] int _maxHealth;
    [SerializeField] HealthBar _playerHealthBar;

    HealthSystem _healthSystem;
    PlayerStatus _playerStatus;

    PlayerActionMap _playerInput;

    void Awake()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.TestRestart.Enable();
        _playerInput.Player.TestRestart.started += _TestRestartInput;
    }

    // Start is called before the first frame update
    void Start()
    {
        _healthSystem = new HealthSystem(_maxHealth);
        _healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        _playerStatus = GetComponent<PlayerStatus>();

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

    public void DetectHit(int damageTaken, Vector2 knockbackForce = default(Vector2), float knockbackDuration = 0)
    {
        _NotifyObservers(damageTaken);

        if (_playerStatus)
        {
            _playerStatus.SetKnockbackTime(knockbackDuration);
            _playerStatus.SetStatus(StatusEnum.Knockback, true);
            _playerStatus.SetDisabledHeightMaintenance();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().velocity = knockbackForce;
        }
    }

    //Note: This input exists at present for debugging purposes. This will be removed at a later point in time
    void _TestRestartInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
