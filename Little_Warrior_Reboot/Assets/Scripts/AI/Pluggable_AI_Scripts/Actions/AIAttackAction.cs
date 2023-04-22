using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Action", menuName = "ScriptableObjects/PluggableAI/Actions/Attack")]
public class AIAttackAction : AIAction
{
    public override void Act(AIThinker thinker)
    {
        AICombat aiCombat = thinker.GetComponent<AICombat>();

        if (aiCombat == null) return;

        if (!aiCombat.GetIsAttacking())
        {
            _Attack(thinker, aiCombat);
        }
    }

    void _Attack(AIThinker thinker, AICombat aiCombat)
    {
        aiCombat.StartAttack();
        //aiCombat.SetIsAttacking(true);
    }
}
