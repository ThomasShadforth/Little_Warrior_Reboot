using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLevelSystem : MonoBehaviour, IDataPersistence
{
    [SerializeField] int[] _expToNextLevel;
    [SerializeField] int[] _skillPointLevels;
    [SerializeField] int _currentLevel = 1;
    [SerializeField] int _maxLevel;
    [SerializeField] int _baseEXP;
    [SerializeField] int _currentEXP;
    [SerializeField] float _expMult = 1.08f;

    PlayerActionMap _playerInput;

    private void Awake()
    {
        _SetupLevelRequirements();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.TestEXPAdd.Enable();
        _playerInput.Player.TestEXPAdd.started += _AddEXPInput;
        
    }

    public void IncreaseExp(int expToAdd)
    {
        _currentEXP += expToAdd;

        FindObjectOfType<ExpBar>().UpdateEXPFillAmount(((float)_currentEXP / _expToNextLevel[_currentLevel]), _currentEXP, _expToNextLevel[_currentLevel], _expToNextLevel[_currentLevel + 1]);

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

    public void LoadData(GameData data)
    {
        _currentLevel = data.playerLevel;

        GetComponent<PlayerAbilities>().CallPlayerLevelNotify(_currentLevel);
        FindObjectOfType<ExpBar>().LoadLevelText(_currentLevel);

        _currentEXP = data.playerCurrentEXP;

        FindObjectOfType<ExpBar>().UpdateEXPFillAmount(((float)_currentEXP / _expToNextLevel[_currentLevel]), _currentEXP, _expToNextLevel[_currentLevel], _expToNextLevel[_currentLevel + 1]);
    }

    public void SaveData(GameData data)
    {
        data.playerLevel = _currentLevel;

        data.playerCurrentEXP = _currentEXP;
    }
}
