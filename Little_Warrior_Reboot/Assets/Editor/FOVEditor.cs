using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineOfSight2D))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        LineOfSight2D los = (LineOfSight2D)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(los.transform.position, Vector3.forward, Vector3.right, 360, los.losRadius);

        Vector3 viewAngle1 = _DirectionFromAngle(los.transform.eulerAngles.z, -los.losAngle / 2);
        Vector3 viewAngle2 = _DirectionFromAngle(los.transform.eulerAngles.z, los.losAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(los.transform.position, los.transform.position + viewAngle1 * los.losRadius);
        Handles.DrawLine(los.transform.position, los.transform.position + viewAngle2 * los.losRadius);

        if (los.GetCanSeePlayer())
        {
            Handles.color = Color.green;
            Handles.DrawLine(los.transform.position, los.GetPlayer().transform.position);
            Handles.EndGUI();
        }
    }

    Vector3 _DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
