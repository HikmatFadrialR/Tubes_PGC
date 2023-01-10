using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AI))]

[ExecuteInEditMode]
public class AIEDITOR : Editor
{
    private void OnSceneGUI()
    {
        AI fov = (AI)target;
        Handles.color = Color.red; //attack range
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.stopDistance + 0.4f); 

        Handles.color = Color.yellow; //fov close range
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.agent.radius + 0.7f); 

        Handles.color = Color.white; //fov range
        Handles.DrawWireArc(fov.PFOV.position, Vector3.up, Vector3.forward, 360, fov.range);

        Vector3 viewAngle01 = DirectionFromAngle(fov.PFOV.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.PFOV.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow; //fov angle
        Handles.DrawLine(fov.PFOV.position, fov.PFOV.position + viewAngle01 * fov.range);
        Handles.DrawLine(fov.PFOV.position, fov.PFOV.position + viewAngle02 * fov.range);

        if (fov.isSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.PFOV.position, fov.target.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}