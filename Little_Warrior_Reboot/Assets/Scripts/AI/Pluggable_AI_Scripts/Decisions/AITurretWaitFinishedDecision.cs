using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Wait Decision", menuName = "ScriptableObjects/PluggableAI/Decisions/Turret Wait Finished")]
public class AITurretWaitFinishedDecision : AIDecision
{
    public override bool Decide(AIThinker thinker)
    {
        bool waitFinished = _CheckWaitFinished(thinker);
        return waitFinished;
    }

    bool _CheckWaitFinished(AIThinker thinker)
    {
        AITurretData turretData = thinker.GetComponent<AITurretData>();

        if (turretData == null) return false;

        if (turretData.GetWaitTimerEnded())
        {
            turretData.SetWaitTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
