using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastSaved;

    //Determine what data is necessary
    //Player's current position in the last saved scene
    public Vector3 playerPosition;
    //Last scene the player was saved in
    public string lastSceneSaved;

    //Determine whether a completion percentage is necessary

    public GameData()
    {
        this.playerPosition = Vector3.zero;
        this.lastSceneSaved = "";
    }
}
