using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttackInfo : MonoBehaviour, IDataPersistence
{
    [SerializeField] AttackData[] _playerLightAttacks;
    [SerializeField] AttackData[] _playerHeavyAttacks;

    PlayerAbilities _playerAbilities;

    // Start is called before the first frame update
    void Start()
    {
        _playerAbilities = GetComponent<PlayerAbilities>();
        _SetupDefaultSkills();
    }

    void _SetupDefaultSkills()
    {
        foreach(AttackData attack in _playerLightAttacks)
        {
            if (attack.GetDefaultStatus())
            {
                _playerAbilities.UnlockDefaultAbility(attack.GetAbilityType());
                attack.SetUnlockStatus(true);
            }
        }

        foreach(AttackData attack in _playerHeavyAttacks)
        {
            if (attack.GetDefaultStatus())
            {
                _playerAbilities.UnlockDefaultAbility(attack.GetAbilityType());
                attack.SetUnlockStatus(true);
            }
        }

    }

    public void UnlockAttack(string attackName)
    {
        foreach(AttackData data in _playerLightAttacks)
        {
            if(data.GetAttackName() == attackName)
            {
                data.SetUnlockStatus(true);
                return;
            }
        }

        foreach(AttackData data in _playerHeavyAttacks)
        {
            if(data.GetAttackName() == attackName)
            {
                data.SetUnlockStatus(true);
                return;
            }
        }
    }

    public AttackData SearchForLightAttack(string attackName = "")
    {
        AttackData attackToReturn = null;

        if(attackName == "")
        {
            if (_playerLightAttacks[0].GetUnlockStatus())
            {
                attackToReturn = _playerLightAttacks[0];
            }
        }
        else
        {
            for(int i = 0; i < _playerLightAttacks.Length; i++)
            {
                if(_playerLightAttacks[i].GetAttackName() == attackName)
                {
                    if (_playerLightAttacks[i].GetUnlockStatus())
                    {
                        attackToReturn = _playerLightAttacks[i];
                        i = _playerLightAttacks.Length;
                    }
                }
            }
        }

        if(attackToReturn == null)
        {
            Debug.Log("NO ATTACK FOUND");
        }

        //Debug.Log(attackToReturn.GetAttackName());

        return attackToReturn;
    }

    public AttackData SearchForHeavyAttack(string attackName)
    {
        AttackData attackToReturn = null;

        
        for (int i = 0; i < _playerHeavyAttacks.Length; i++)
        {
            if (_playerHeavyAttacks[i].GetAttackName() == attackName)
            {
                if (_playerHeavyAttacks[i].GetUnlockStatus())
                {
                    attackToReturn = _playerHeavyAttacks[i];
                    i = _playerHeavyAttacks.Length;
                }
            }
        }
        

        return attackToReturn;
    }

    public void SaveData(GameData data)
    {
        data.playerLightAttacks = _playerLightAttacks.ToList();
        data.playerHeavyAttacks = _playerHeavyAttacks.ToList();
    }
    public void LoadData(GameData data)
    {
        if (data.playerLightAttacks.Count != 0)
        {
            AttackData[] loadedLightAttacks = data.playerLightAttacks.ToArray();

            for (int i = 0; i < _playerLightAttacks.Length; i++)
            {
                _playerLightAttacks[i].SetUnlockStatus(loadedLightAttacks[i].GetUnlockStatus());
            }
        }

        if (data.playerHeavyAttacks.Count != 0)
        {
            AttackData[] loadedHeavyAttacks = data.playerHeavyAttacks.ToArray();

            for (int i = 0; i < _playerHeavyAttacks.Length; i++)
            {
                _playerHeavyAttacks[i].SetUnlockStatus(loadedHeavyAttacks[i].GetUnlockStatus());
            }
        }
    }

    
}
