using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] protected bool _needsPlayer;
    protected bool _hasStarted;

    // Start is called before the first frame update
    void Start()
    {
        if (!_needsPlayer)
        {
            _hasStarted = true;
        }
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
}
