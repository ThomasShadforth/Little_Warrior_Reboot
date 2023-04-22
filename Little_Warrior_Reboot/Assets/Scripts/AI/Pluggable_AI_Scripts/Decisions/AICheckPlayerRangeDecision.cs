using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player In Range Decision", menuName = "ScriptableObjects/PluggableAI/Decisions/Player In Range")]
public class AICheckPlayerRangeDecision : AIDecision
{
    public override bool Decide(AIThinker thinker)
    {
        bool playerInRange = _CheckPlayerDistance(thinker);
        return playerInRange;
    }

    bool _CheckPlayerDistance(AIThinker thinker)
    {
        AITargetData targetData = thinker.GetComponent<AITargetData>();

        if (targetData == null) return false;

        AIMovement aiMove = thinker.GetComponent<AIMovement>();

        if (aiMove == null) return false;

        if(Vector2.Distance(thinker.transform.position, targetData.GetPlayerTarget().position) <= targetData.GetMinimumTargetDistance())
        {
            aiMove.SetMaxSpeed(true);
            return true;
        }
        else
        {
            if (aiMove.GetChasingState())
            {
                aiMove.SetMaxSpeed(false);
            }

            return false;
        }
        
    }
}
