using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase Action", menuName = "ScriptableObjects/PluggableAI/Actions/Chase")]
public class AIChaseAction : AIAction
{
    public override void Act(AIThinker thinker)
    {
        _CheckTargetDirection(thinker);
        _TickAttackCooldown(thinker);
    }

    void _CheckTargetDirection(AIThinker thinker)
    {
        AITargetData targetData = thinker.GetComponent<AITargetData>();

        if (targetData == null) return;

        Vector2 targetDirection = targetData.GetPlayerTarget().position - thinker.transform.position;

        AIMovement aiMove = thinker.GetComponent<AIMovement>();

        if (aiMove == null) return;

        if(targetDirection.x > .1f)
        {
            aiMove.SetXDirection(1f);
        } else if(targetDirection.x < -.1f)
        {
            aiMove.SetXDirection(-1f);
        }
    }

    void _TickAttackCooldown(AIThinker thinker)
    {
        AICombat aiCombat = thinker.GetComponent<AICombat>();

        if (aiCombat == null) return;

        aiCombat.TickAttackCooldown();
    }
}
