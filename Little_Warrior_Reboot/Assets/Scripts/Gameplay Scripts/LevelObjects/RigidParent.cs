using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidParent : MonoBehaviour
{
    [SerializeField] Rigidbody2D _targetRB;
    // Start is called before the first frame update
    void Start()
    {
        if(_targetRB != null)
        {
            transform.position = _targetRB.transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_targetRB != null)
        {
            transform.position = _targetRB.transform.position;

        }
    }
}
