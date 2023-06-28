using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlocker : MonoBehaviour
{
    [SerializeField] Collider2D _characterCollider;
    [SerializeField] Collider2D _blockingCollider;

    [SerializeField] LayerMask _ignoredLayer;
    [SerializeField] LayerMask _collisionBlockerLayer;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(_characterCollider, _blockingCollider, true);

        if(_ignoredLayer.value != 0)
        {
            int logLayerValue = (int)Mathf.Log(_ignoredLayer.value, 2);
            int logCollisionBlockerLayer = (int)Mathf.Log(_collisionBlockerLayer.value, 2);

            Physics2D.IgnoreLayerCollision(logLayerValue, logCollisionBlockerLayer, true);
        }
        
    }

}
