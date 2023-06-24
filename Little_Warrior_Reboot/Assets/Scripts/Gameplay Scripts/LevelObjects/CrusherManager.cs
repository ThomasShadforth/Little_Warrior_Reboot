using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherManager : MonoBehaviour
{
    [SerializeField] float _crusherActivationDelay;
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
                yield return new WaitForSeconds(_crusherActivationDelay);
                crusher.ActivateCrusher();
                //Debug.Log("ACTIVATING CRUSHER");
                
            }
        }
    }
}
