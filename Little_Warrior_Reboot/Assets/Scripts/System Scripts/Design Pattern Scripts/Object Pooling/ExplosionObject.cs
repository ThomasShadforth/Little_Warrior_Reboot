using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.Play("Explode");
    }

    public void OnExplosionFinished()
    {
        ExplosionObjectPool.instance.AddToPool(this.gameObject);
    }
}
