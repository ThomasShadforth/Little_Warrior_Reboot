using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] bool _isLevelCheckpoint;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_isLevelCheckpoint)
            {
                CheckpointsManager.currentLevelCheckpoint = transform.position;
            }
            else
            {
                CheckpointsManager.currentCrusherCheckpoint = transform.position;
                Debug.Log(CheckpointsManager.currentCrusherCheckpoint);
            }
        }
    }
}
