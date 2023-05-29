using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckpointsManager
{
    public static Vector3 currentLevelCheckpoint;
    public static Vector3 currentCrusherCheckpoint;


    public static void MovePlayer(GameObject player, bool wasCrushed = false)
    {
        if (wasCrushed)
        {
            player.transform.position = currentCrusherCheckpoint;
        }
        else
        {
            player.transform.position = currentLevelCheckpoint;
        }
    }
}
