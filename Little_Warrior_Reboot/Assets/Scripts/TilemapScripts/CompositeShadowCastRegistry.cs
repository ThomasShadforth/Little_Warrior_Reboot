using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class CompositeShadowCastRegistry : MonoBehaviour
{
    CompositeShadowCaster2D _compositeShadow;
    private void Awake()
    {
        _compositeShadow = GetComponent<CompositeShadowCaster2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if(_compositeShadow != null)
        {
            Debug.Log(_compositeShadow.GetShadowCasters().Count);
            //_compositeShadow.RegisterShadowCaster2D()
        }
    }
}
