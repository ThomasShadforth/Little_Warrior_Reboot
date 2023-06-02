using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Player In Range Decision", menuName = "ScriptableObjects/PluggableAI/Decisions/Turret Player In Range")]
public class AITurretPlayerInRangeDecision : AIDecision
{
    public override bool Decide(AIThinker thinker)
    {
        bool playerInRange = _CheckPlayerInRange(thinker);
        return playerInRange;
    }

    bool _CheckPlayerInRange(AIThinker thinker)
    {
        AITurretData turretData = thinker.GetComponent<AITurretData>();

        if (turretData == null) return false;

        if (turretData.GetCanSeePlayer())
        {
            turretData.SetLaserActive(true);
            return true;
        }
        else
        {
            turretData.SetLaserActive();

            if (!turretData.GetIsAttacking())
            {
                turretData.SetAttackWaitTime();
            }

            return false;
        }
    }
}
