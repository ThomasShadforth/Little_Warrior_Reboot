using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThinker : MonoBehaviour
{
    //Insert: Current State and remain state
    [SerializeField] AIState _currentState;
    [SerializeField] AIState _remainState;

    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        if(_currentState != null)
        {
            _currentState.UpdateState(this);
        }
    }

    public void TransitionToState(AIState stateToTransitionTo)
    {
        if(stateToTransitionTo != _remainState)
        {
            _currentState = stateToTransitionTo;
        }
    }

    
    
}
