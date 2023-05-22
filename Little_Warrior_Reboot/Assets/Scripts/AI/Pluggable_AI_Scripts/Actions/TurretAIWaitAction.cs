using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Wait Action", menuName = "ScriptableObjects/PluggableAI/Actions/Turret Wait")]
public class TurretAIWaitAction : AIAction
{
    public override void Act(AIThinker thinker)
    {
        _Wait(thinker);
    }

    void _Wait(AIThinker thinker)
    {
        AITurretData turretData = thinker.GetComponent<AITurretData>();

        if (turretData == null) return;

        turretData.TickWaitTimer();
    }
}
