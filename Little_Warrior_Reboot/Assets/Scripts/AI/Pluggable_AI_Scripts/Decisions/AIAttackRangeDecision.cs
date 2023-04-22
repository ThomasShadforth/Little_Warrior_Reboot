using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Range Decision", menuName = "ScriptableObjects/PluggableAI/Decisions/Attack Range")]
public class AIAttackRangeDecision : AIDecision
{
    public override bool Decide(AIThinker thinker)
    {

        bool playerInRange = _CheckAttackRange(thinker);
        return playerInRange;
    }


    bool _CheckAttackCooldown(AIThinker thinker)
    {
        AICombat aiCombat = thinker.GetComponent<AICombat>();

        if (aiCombat == null) return false;

        return aiCombat.GetAttackCooldown() <= 0;
    }

    bool _CheckAttackRange(AIThinker thinker)
    {
        if (!_CheckAttackCooldown(thinker)) return false;

        AITargetData targetData = thinker.GetComponent<AITargetData>();

        if (targetData == null) return false;

        

        if(Vector2.Distance(thinker.transform.position, targetData.GetPlayerTarget().position) <= targetData.GetMinimumAttackDistance())
        {
            thinker.GetComponent<AIMovement>().SetXDirection(0);
            return true;   
        }
        else
        {
            return false;
        }
    }
}
