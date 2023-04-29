using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager
{
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public event EventHandler OnSkillPointsChanged;
    public event EventHandler OnPlayerLevelUp;

    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public AbilityType abilityType;
    }

    public enum AbilityType
    {
        None,
        HealthMax_1,
        HealthMax_2,
        MoveSpeedMax_1,
        Rising_Punch_1,
        Rising_Punch_2,
        Punch_Default,
        Punch_1,
        Punch_2
    }

    private List<AbilityType> _unlockedAbilityList;
    private int _skillPoints;
    private int _currentLevel;

    public PlayerSkillManager()
    {
        _unlockedAbilityList = new List<AbilityType>();
        _skillPoints = 10;
        _currentLevel = 1;
    }

    public void IncreaseSkillPoints(int numberOfSP)
    {
        _skillPoints += numberOfSP;
        OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void PlayerLevelUp(int currentLevel)
    {
        _currentLevel = currentLevel;
        OnPlayerLevelUp?.Invoke(this, EventArgs.Empty);
    }

    private void _UnlockAbility(AbilityType abilityType)
    {
        if (!IsAbilityUnlocked(abilityType))
        {
            _unlockedAbilityList.Add(abilityType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { abilityType = abilityType });
        }
    }

    public bool IsAbilityUnlocked(AbilityType abilityType)
    {
        return _unlockedAbilityList.Contains(abilityType);
    }

    public bool CheckLevelRequirement(int levelRequirement)
    {
        if (_currentLevel >= levelRequirement)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanUnlock(AbilityType abilityType)
    {
        AbilityType[] abilityRequirements = GetAbilityRequirements(abilityType);

        if (abilityRequirements.Length != 0)
        {
            if (abilityRequirements[0] == AbilityType.None)
            {
               
                return true;
            }
            else
            {
                bool canUnlock = true;

                for (int i = 0; i < abilityRequirements.Length; i++)
                {
                    if (!IsAbilityUnlocked(abilityRequirements[i]))
                    {
                        canUnlock = false;
                    }
                }
                
                return canUnlock;
            }
        }
        else
        {
            Debug.Log(abilityType.ToString());
            return false;
        }
        

    }

    public int GetSkillPoints()
    {
        return _skillPoints;
    }

    public bool CheckSkillPointRequirement(int costRequirement)
    {
        return _skillPoints >= costRequirement;
    }

    public AbilityType[] GetAbilityRequirements(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.HealthMax_2: return new AbilityType[1] { AbilityType.HealthMax_1};
            case AbilityType.Rising_Punch_2: return new AbilityType[1] { AbilityType.Rising_Punch_1 };
        }

        return new AbilityType[1] {AbilityType.None};
    }

    public bool TryUnlockAbility(AbilityType abilityType, int abilityCost)
    {
        if (CanUnlock(abilityType))
        {
            _UnlockAbility(abilityType);
            _skillPoints -= abilityCost;
            OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }

}
