using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour, IDamageInterface
{
    Animator _animator;

    private readonly float _heavyHitThreshold = 20.0f;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    public void DetectHit(int damageTaken, Vector2 knockbackApplied = default, float knockbackDuration = 0)
    {
        if(damageTaken < _heavyHitThreshold)
        {
            _animator.Play("DummyLightHit");
        }
        else
        {
            _animator.Play("DummyHeavyHit");
        }
    }

    public void ResetToIdle()
    {
        _animator.Play("Idle");
    }

}
