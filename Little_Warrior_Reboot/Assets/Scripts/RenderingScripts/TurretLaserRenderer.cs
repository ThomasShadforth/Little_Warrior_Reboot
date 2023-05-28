using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretLaserRenderer : MonoBehaviour
{
    [SerializeField] float _turretLaserLength;
    [SerializeField] LineRenderer _FOVLineRenderer;
    [SerializeField] LineRenderer _targetingLineRenderer;
    LineOfSight2D _turretLos;
    void Start()
    {
        _FOVLineRenderer = GetComponent<LineRenderer>();
        _turretLos = GetComponentInParent<LineOfSight2D>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if(_turretLos != null && _turretLos.GetCanSeePlayer())
        {
            if (_FOVLineRenderer.enabled)
            {
                _FOVLineRenderer.enabled = false;
                _targetingLineRenderer.enabled = true;
            }

            _targetingLineRenderer.SetPositions(new Vector3[] { transform.position, _turretLos.GetLastPlayerPosition() });
        }
        else
        {
            if (!_FOVLineRenderer.enabled)
            {
                _FOVLineRenderer.enabled = true;
                _targetingLineRenderer.enabled = false;
            }

            Vector3 endPosition = transform.position + transform.right * _turretLaserLength;
            _FOVLineRenderer.SetPositions(new Vector3[] { transform.position, endPosition });
        }

        
    }
}
