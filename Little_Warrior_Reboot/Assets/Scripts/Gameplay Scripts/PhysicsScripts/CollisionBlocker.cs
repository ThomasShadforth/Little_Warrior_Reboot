using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlocker : MonoBehaviour
{
    [SerializeField] Collider2D _characterCollider;
    [SerializeField] Collider2D _blockingCollider;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(_characterCollider, _blockingCollider, true);
    }

}
