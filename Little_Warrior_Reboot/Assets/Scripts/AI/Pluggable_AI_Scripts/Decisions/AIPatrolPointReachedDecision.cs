using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReachedPatrolPointDecision", menuName = "ScriptableObjects/PluggableAI/Decisions/ReachedPatrolPoint")]
public class AIPatrolPointReachedDecision : AIDecision
{
    public override bool Decide(AIThinker thinker)
    {
        bool hasReached = _CheckDistance(thinker);
        return hasReached;
    }

    bool _CheckDistance(AIThinker thinker)
    {
        AIPatrolData patrolData = thinker.GetComponent<AIPatrolData>();

        if (patrolData == null) return false;

        if (patrolData.GetPatrolPoints().Length == 0) return false;

        AIMovement aiMove = thinker.GetComponent<AIMovement>();

        if (aiMove == null) return false;

        float distanceToPoint = Vector2.Distance(thinker.transform.position, patrolData.GetPatrolPoints()[patrolData.GetPatrolIndex()].position);

        if(distanceToPoint <= patrolData.GetMinimumDistance())
        {
            patrolData.SetPatrolIndex();
            aiMove.SetXDirection(0);
            return true;
        }
        else
        {
            return false;
        }
    }
}
