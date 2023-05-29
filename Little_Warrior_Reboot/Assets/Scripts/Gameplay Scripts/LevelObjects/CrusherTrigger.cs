using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageInterface damageInterface = other.gameObject.GetComponent<IDamageInterface>();
        if (damageInterface != null)
        {
            //Damage the player
            damageInterface.DetectHit(20);
            Debug.Log("HIT!");
            //To do: Insert logic for resetting player position to just before crusher (Will need to work in checkpoints)
            CheckpointsManager.MovePlayer(other.gameObject, true);
        }
    }
}
