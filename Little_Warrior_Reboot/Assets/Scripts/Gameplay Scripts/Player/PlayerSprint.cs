using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprint : MonoBehaviour
{
    [SerializeField] float _sprintSpeedIncrease;
    [SerializeField] float _timeTakenToSprint;
    [SerializeField] float _timeTakenToSlow;
    [SerializeField] float _distBetweenAfterImage;

    bool _startedSprint;
    [SerializeField]
    float _currentSprintSpeed;

    PlayerActionMap _playerInput;
    [SerializeField] float lerpPoint;


    float _xDirection;
    float _prevXDirection;
    float _lastImageXPos;

    private void Awake()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.Sprint.Enable();
        _playerInput.Player.PlayerMovement.Enable();
        _playerInput.Player.Sprint.started += _SprintInput;
        
    }

    private void OnDisable()
    {
        if(_playerInput != null)
        {
            _playerInput.Dispose();
        }
    }

    private void Start()
    {
        _prevXDirection = 1;
    }

    void FixedUpdate()
    {
        if(_xDirection != 0)
        {
            _prevXDirection = _xDirection;
        }

        _xDirection = _playerInput.Player.PlayerMovement.ReadValue<float>();

        if(_xDirection != _prevXDirection && (_xDirection != 0 || _xDirection == 0) && _startedSprint)
        {
            StopCoroutine(BurstSprintCo());
            StartCoroutine(StopSprintCo());
        }

        if (AfterImageObjectPool.instance != null)
        {
            if (_startedSprint)
            {
                if (Mathf.Abs(transform.position.x - _lastImageXPos) > _distBetweenAfterImage)
                {
                    AfterImageObjectPool.instance.GetFromPool();
                    _lastImageXPos = transform.position.x;
                }
            }
        }

        //Debug.Log(Mathf.Lerp(_sprintSpeedIncrease, 0, lerpPoint));
        //Debug.Log(Mathf.MoveTowards(0, _sprintSpeedIncrease, lerpPoint));

    }

    public float GetSprintSpeed()
    {
        return _currentSprintSpeed * _xDirection;
    }

    void _SprintInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!_startedSprint)
            {
                _startedSprint = true;
                StartCoroutine(BurstSprintCo());
            }
        }
    }

    IEnumerator BurstSprintCo()
    {

        float timePercentage = 0;
        while(timePercentage < 1)
        {
            timePercentage += GamePause.deltaTime / _timeTakenToSprint;
            _currentSprintSpeed = Mathf.Lerp(_currentSprintSpeed, _sprintSpeedIncrease, timePercentage);
            yield return null;
        }

        timePercentage = 1;

        while(timePercentage > 0)
        {
            
            timePercentage -= GamePause.deltaTime / _timeTakenToSlow;
            //_currentSprintSpeed = Mathf.MoveTowards(0, _sprintSpeedIncrease, timePercentage);
            _currentSprintSpeed = Mathf.Lerp(0, _sprintSpeedIncrease, timePercentage);
            //Debug.Log("DECREASING SPRINT SPEED");
            yield return null;
        }

        _currentSprintSpeed = 0;
        _startedSprint = false;
    }

    IEnumerator StopSprintCo()
    {
        float timePercentage = 1;

        while(timePercentage > 0)
        {
            timePercentage -= GamePause.deltaTime / _timeTakenToSlow;
            _currentSprintSpeed = Mathf.MoveTowards(0, _sprintSpeedIncrease, timePercentage);
            yield return null;
        }

        _currentSprintSpeed = 0;
        _startedSprint = false;
    }

}
