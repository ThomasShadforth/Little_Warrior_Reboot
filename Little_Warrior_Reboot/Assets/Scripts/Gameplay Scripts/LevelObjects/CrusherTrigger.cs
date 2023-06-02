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

            if (ExplosionObjectPool.instance != null)
            {
                GameObject explosionObj = ExplosionObjectPool.instance.GetFromPool();
                explosionObj.transform.position = other.transform.position;
            }

            CheckpointsManager.MovePlayer(other.gameObject, true);

            

        }
    }
}
