using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait Finished Decision", menuName = "ScriptableObjects/PluggableAI/Decisions/Wait Finished")]
public class AIWaitFinishedDecision : AIDecision
{
    public override bool Decide(AIThinker thinker)
    {
        bool waitFinished = _CheckWaitFinished(thinker);
        return waitFinished;
    }

    bool _CheckWaitFinished(AIThinker thinker)
    {
        AIPatrolData patrolData = thinker.GetComponent<AIPatrolData>();

        if (patrolData == null) return false;

        if(patrolData.GetWaitTimer() <= 0)
        {
            patrolData.SetWaitTimer();
            return true;
        }
        else
        {
            return false;
        }

        
    }
}
