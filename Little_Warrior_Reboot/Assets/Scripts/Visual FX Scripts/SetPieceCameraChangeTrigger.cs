using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPieceCameraChangeTrigger : MonoBehaviour
{
    [SerializeField] string _camToSwitchTo;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Flag a message saying the player has entered the area (Just for testing purposes)
            //Debug.Log("Player has entered the trigger area");

            if(CinemachineStateDrivenCam.instance != null)
            {
                CinemachineStateDrivenCam.instance.ChangeState(_camToSwitchTo);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CinemachineStateDrivenCam.instance != null)
            {
                CinemachineStateDrivenCam.instance.ChangeState("PlayerCam");
            }
        }
    }
}
