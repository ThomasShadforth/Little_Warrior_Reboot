using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol Action", menuName = "ScriptableObjects/PluggableAI/Actions/Patrol")]
public class AIPatrolAction : AIAction
{
    public override void Act(AIThinker thinker)
    {
        _CheckPatrolDirection(thinker);
    }

    void _CheckPatrolDirection(AIThinker thinker)
    {
        AIPatrolData patrolData = thinker.GetComponent<AIPatrolData>();

        if (patrolData == null) return;

        Vector2 patrolPointDirection = patrolData.GetPatrolPoints()[patrolData.GetPatrolIndex()].position - thinker.transform.position;

        AIMovement aiMove = thinker.GetComponent<AIMovement>();

        if (aiMove == null) return;

        if(patrolPointDirection.x > .1f)
        {
            aiMove.SetXDirection(1f);
        } else if(patrolPointDirection.x < -.1f)
        {
            aiMove.SetXDirection(-1f);
        }
    }


}
