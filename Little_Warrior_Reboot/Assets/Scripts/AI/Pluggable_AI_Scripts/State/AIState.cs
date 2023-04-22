using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "ScriptableObjects/PluggableAI/State")]
public class AIState : ScriptableObject
{
    [SerializeField] AIAction[] _actions;
    [SerializeField] AITransition[] _transitions;

    public void UpdateState(AIThinker thinker)
    {
        _ExecuteActions(thinker);
        _CheckTransitions(thinker);
    }

    void _ExecuteActions(AIThinker thinker)
    {
        for(int i = 0; i < _actions.Length; i++)
        {
            _actions[i].Act(thinker);
        }
    }

    void _CheckTransitions(AIThinker thinker)
    {
        for(int i = 0; i < _transitions.Length; i++)
        {
            bool decisionSuccess = _transitions[i].decision.Decide(thinker);

            if (decisionSuccess)
            {
                thinker.TransitionToState(_transitions[i].trueState);
            }
            else
            {
                thinker.TransitionToState(_transitions[i].falseState);
            }
        }
    }
}
