using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Patrol Action", menuName = "ScriptableObjects/PluggableAI/Actions/Turret Patrol")]
public class TurretAIPatrolAction : AIAction
{
    public override void Act(AIThinker thinker)
    {
        _PatrolScan(thinker);
    }

    void _PatrolScan(AIThinker thinker)
    {
        AITurretData turretData = thinker.GetComponent<AITurretData>();

        if (turretData == null) return;

        turretData.RotateTurret();
    }
}
