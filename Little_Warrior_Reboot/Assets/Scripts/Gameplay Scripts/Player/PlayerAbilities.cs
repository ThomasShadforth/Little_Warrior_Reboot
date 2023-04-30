using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAbilities : MonoBehaviour
{
    PlayerSkillManager _playerSkills;
    PlayerMovement _playerMove;
    PlayerCombat _playerCombat;

    // Start is called before the first frame update
    void Start()
    {
        _playerMove = GetComponent<PlayerMovement>();
        _playerCombat = GetComponent<PlayerCombat>();
        _playerSkills = new PlayerSkillManager();
        
        SkillTreeUI skillTree = FindObjectOfType<SkillTreeUI>(true);

        if(skillTree != null)
        {
            skillTree.SetPlayerSkills(_playerSkills);
        }

        _playerSkills.OnSkillUnlocked += PlayerSkillManager_OnSkillUnlocked;

    }

    void PlayerSkillManager_OnSkillUnlocked(object sender, PlayerSkillManager.OnSkillUnlockedEventArgs e)
    {
        switch (e.abilityType)
        {
            case PlayerSkillManager.AbilityType.HealthMax_1:
                _playerMove.SetMaxSpeed(20);
                break;
            case PlayerSkillManager.AbilityType.HealthMax_2:
                _playerMove.SetMaxSpeed(100);
                break;
            case PlayerSkillManager.AbilityType.MoveSpeedMax_1:
                _playerMove.SetMaxSpeed(30);
                break;
            case PlayerSkillManager.AbilityType.Rising_Punch_1:
                _playerCombat.UnlockAttack("Rising Punch");
                break;
        }
    }

    public void CallPlayerLevelNotify(int playerLevel)
    {
        //Debug.Log("LEVEL: " + playerLevel);
        _playerSkills.PlayerLevelUp(playerLevel);
    }

    public void CallAddSkillPoint(int skillpoints)
    {
        _playerSkills.IncreaseSkillPoints(skillpoints);
    }

    public void UnlockDefaultAbility(PlayerSkillManager.AbilityType abilityType)
    {
        bool unlocked = _playerSkills.TryUnlockAbility(abilityType, 0);
    }
}
