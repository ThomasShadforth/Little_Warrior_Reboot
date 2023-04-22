using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack End Decision", menuName = "ScriptableObjects/PluggableAI/Decisions/Attack End")]
public class AIAttackEndDecision : AIDecision
{
    public override bool Decide(AIThinker thinker)
    {
        bool isAttackEnd = _CheckAttackEnd(thinker);
        return isAttackEnd;
    }

    bool _CheckAttackEnd(AIThinker thinker)
    {
        AICombat aiCombat = thinker.GetComponent<AICombat>();

        if (aiCombat == null) return false;

        if (!aiCombat.GetIsAttacking()) return false;

        AnimatorStateInfo stateInfo = thinker.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        float nTime = stateInfo.normalizedTime;

        if (nTime >= 1.0f)
        {
            //End the animation
            aiCombat.SetIsAttacking(false);
            aiCombat.SetAttackCooldown();
            aiCombat.ResetAttackAnimation();

            return true;
        }
        else
        {

            return false;
        }

        
        
    }
}
