using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateCrushersCo());
    }

    
    IEnumerator ActivateCrushersCo()
    {
        Crusher[] crushers = GetComponentsInChildren<Crusher>();

        if(crushers.Length != 0)
        {
            foreach(Crusher crusher in crushers)
            {
                crusher.ActivateCrusher();
                Debug.Log("ACTIVATING CRUSHER");
                yield return new WaitForSeconds(.4f);
            }
        }
    }
}
