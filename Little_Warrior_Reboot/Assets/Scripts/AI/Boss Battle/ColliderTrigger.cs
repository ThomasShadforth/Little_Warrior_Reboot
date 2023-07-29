using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public EventHandler OnPlayerEnterTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement playerMove = other.GetComponent<PlayerMovement>();

        //If the object is a player, then trigger the event
        if (playerMove)
        {
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        }
    }
}
