using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatInput : MonoBehaviour
{
    PlayerActionMap _playerInput;
    PlayerCombat _playerCombat;

    private void Awake()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.Enable();
        _playerInput.Player.LightAttack.started += _CheckLightInput;
        _playerInput.Player.HeavyAttack.started += _CheckHeavyInput;
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
        _playerCombat = GetComponent<PlayerCombat>();
    }

    void _CheckLightInput(InputAction.CallbackContext context)
    {
        if (_playerCombat)
        {
            _playerCombat.TriggerAttack("Light");
        }
    }

    void _CheckHeavyInput(InputAction.CallbackContext context)
    {
        if (_playerCombat)
        {
            _playerCombat.TriggerAttack("Heavy");
        }
    }
}
