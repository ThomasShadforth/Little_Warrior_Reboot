using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAbilities : MonoBehaviour, IDataPersistence
{
    PlayerSkillManager _playerSkills;
    PlayerMovement _playerMove;

    PlayerAttackInfo _playerAttackInfo;

    // Start is called before the first frame update
    private void Awake()
    {
        _playerSkills = new PlayerSkillManager();
    }

    void Start()
    {
        _playerMove = GetComponent<PlayerMovement>();
        _playerAttackInfo = GetComponent<PlayerAttackInfo>();
        
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
                _playerAttackInfo.UnlockAttack("Rising Punch");
                break;
            case PlayerSkillManager.AbilityType.Thrust_Kick_1:
                _playerAttackInfo.UnlockAttack("Thrust Kick");
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
        if(_playerSkills == null)
        {
            Debug.Log("NO SKILL MANAGER");
            return;
        }

        bool unlocked = _playerSkills.TryUnlockAbility(abilityType, 0);
        
    }

    public void LoadData(GameData data)
    {
        _playerSkills.LoadUnlockedAbilities(data.unlockedAbilities);
        _playerSkills.LoadSkillPoints(data.currentSkillPoints);
    }

    public void SaveData(GameData data)
    {
        data.unlockedAbilities = _playerSkills.GetUnlockedAbilities();
        data.currentSkillPoints = _playerSkills.GetSkillPoints();
    }
}
