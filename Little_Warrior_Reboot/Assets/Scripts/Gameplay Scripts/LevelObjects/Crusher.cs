using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CrusherManager parentManager = GetComponentInParent<CrusherManager>();

        if(parentManager != null)
        {
            Debug.Log("MANAGER FOUND");
        }
        else
        {
            Debug.Log("NO MANAGER!");
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
        yield return new WaitForSeconds(2.0f);
        GetComponent<Animator>().SetBool("IsActivated", true);
    }
}
