using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public int fixedUpdateCount;
    public Vector3 playerPosition;
    public List<PlayerSkillManager.AbilityType> unlockedAbilities = new List<PlayerSkillManager.AbilityType>();
    
    public List<AttackData> playerLightAttacks;
    public List<AttackData> playerHeavyAttacks;

    public int playerLevel;
    public int playerCurrentEXP;

    public int currentSkillPoints;

    //To do: Add string to store the name of the last scene the game was saved in.

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        this.fixedUpdateCount = 0;
        playerPosition = Vector3.zero;
        playerLightAttacks = new List<AttackData>();
        playerHeavyAttacks = new List<AttackData>();
        playerLevel = 1;
        playerCurrentEXP = 0;
    }


}
