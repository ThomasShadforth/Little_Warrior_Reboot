using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Check Angle Decision", menuName = "ScriptableObjects/PluggableAI/Decisions/Turret Check Angle")]
public class AITurretCheckAngleDecision : AIDecision
{
    public override bool Decide(AIThinker thinker)
    {
        bool hasExceededAngle = _CheckTurretAngle(thinker);
        return hasExceededAngle;
    }

    bool _CheckTurretAngle(AIThinker thinker)
    {
        AITurretData turretData = thinker.GetComponent<AITurretData>();

        if (turretData == null) return false;

        if (turretData.GetRotationExceeded())
        {
            turretData.SetNewRotation();
            return true;
        }
        else
        {
            return false;
        }
    }
}
