using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager
{
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;

    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public AbilityType abilityType;
    }

    public enum AbilityType
    {
        None,
        HealthMax_1,
        HealthMax_2,
        MoveSpeedMax_1
    }

    private List<AbilityType> _unlockedAbilityList;
    //Insert skill points when ready

    public PlayerSkillManager()
    {
        _unlockedAbilityList = new List<AbilityType>();
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

    public AbilityType[] GetAbilityRequirements(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.HealthMax_2: return new AbilityType[1] { AbilityType.HealthMax_1};
        }

        return new AbilityType[1] {AbilityType.None};
    }

    public bool TryUnlockAbility(AbilityType abilityType)
    {
        if (CanUnlock(abilityType))
        {
            _UnlockAbility(abilityType);
            return true;
        }
        else
        {
            return false;
        }
    }

}
