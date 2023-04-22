using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait Action", menuName = "ScriptableObjects/PluggableAI/Actions/Wait")]
public class AIWaitAction : AIAction
{
    public override void Act(AIThinker thinker)
    {
        _Wait(thinker);
    }

    void _Wait(AIThinker thinker)
    {
        AIPatrolData patrolData = thinker.GetComponent<AIPatrolData>();
        
        if (patrolData == null) return;

        patrolData.TickWaitTimer();
    }
}
