using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    [SerializeField] float _crusherWaitTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        CrusherManager parentManager = GetComponentInParent<CrusherManager>();

        if(parentManager == null)
        {
            
            ResetWait();
        }
    }

    public void ActivateCrusher()
    {
        ResetWait();
    }

    public void ResetWait()
    {
        GetComponent<Animator>().SetBool("IsActivated", false);
        StartCoroutine(WaitToCrushCo());
    }

    IEnumerator WaitToCrushCo()
    {
        yield return new WaitForSeconds(_crusherWaitTime);
        GetComponent<Animator>().SetBool("IsActivated", true);
    }

    
}
