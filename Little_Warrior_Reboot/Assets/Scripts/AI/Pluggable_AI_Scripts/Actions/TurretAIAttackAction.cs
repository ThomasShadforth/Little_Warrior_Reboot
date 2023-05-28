using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Attack Action", menuName = "ScriptableObjects/PluggableAI/Actions/Turret Attack")]
public class TurretAIAttackAction : AIAction
{
    public override void Act(AIThinker thinker)
    {
        _AttackCheck(thinker);
    }

    void _AttackCheck(AIThinker thinker)
    {
        AITurretData turretData = thinker.GetComponent<AITurretData>();

        if (turretData == null) return;

        if (!turretData.GetIsAttacking())
        {
            turretData.TickAttackWaitTimer();
            turretData.TrackPlayer();

            if (turretData.GetAttackWaitTimerEnded())
            {
                turretData.Attack();
            }
        }

    }
}
