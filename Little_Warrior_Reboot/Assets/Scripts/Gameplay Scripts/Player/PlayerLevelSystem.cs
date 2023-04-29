using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLevelSystem : MonoBehaviour
{
    [SerializeField] int[] _expToNextLevel;
    [SerializeField] int[] _skillPointLevels;
    [SerializeField] int _currentLevel = 1;
    [SerializeField] int _maxLevel;
    [SerializeField] int _baseEXP;
    [SerializeField] int _currentEXP;
    [SerializeField] float _expMult = 1.08f;

    PlayerActionMap _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.TestEXPAdd.Enable();
        _playerInput.Player.TestEXPAdd.started += _AddEXPInput;

        _SetupLevelRequirements();
        
    }

    public void IncreaseExp(int expToAdd)
    {
        _currentEXP += expToAdd;

        if(_currentLevel < _maxLevel)
        {
            if(_currentEXP >= _expToNextLevel[_currentLevel])
            {
                _currentEXP -= _expToNextLevel[_currentLevel];

                _currentLevel++;

                GetComponent<PlayerAbilities>().CallPlayerLevelNotify(_currentLevel);

                _IncreaseSkillPoints();
            }
        }
    }

    void _SetupLevelRequirements()
    {
        _expToNextLevel = new int[_maxLevel];

        _expToNextLevel[_currentLevel] = _baseEXP;

        for(int i = 2; i < _expToNextLevel.Length; i++)
        {
            _expToNextLevel[i] = Mathf.FloorToInt(_expToNextLevel[i - 1] * _expMult);
        }
    }

    void _IncreaseSkillPoints()
    {
        GetComponent<PlayerAbilities>().CallAddSkillPoint(_skillPointLevels[_currentLevel]);
    }

    void _AddEXPInput(InputAction.CallbackContext context)
    {
        IncreaseExp(20);
    }
}
