using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] float _platformExitTimer;
    [SerializeField] protected RigidParent _rigidParent;
    [SerializeField] protected bool _needsPlayer;
    [SerializeField] protected bool _requiresTrigger;
    protected bool _hasStarted;

    // Start is called before the first frame update
    void Start()
    {
        SetStartingState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!_hasStarted) return;
        Move();
    }

    protected virtual void Move()
    {

    }

    public virtual void RespondToPlayer()
    {

    }

    public virtual void CheckMovementCondition()
    {
        
    }

    public RigidParent GetRigidParent()
    {
        return _rigidParent;
    }

    public float GetPlatformExitTimer()
    {
        return _platformExitTimer;
    }

    public bool GetRequiresTrigger()
    {
        return _requiresTrigger;
    }

    public virtual void SetStartingState()
    {
        if (!_needsPlayer)
        {
            _hasStarted = true;
        }
    }
}
