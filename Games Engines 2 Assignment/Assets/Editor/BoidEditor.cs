using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Boid))]
public class BoidEditor :  Editor{

    /*
        This class sets up a custom inspector editor
    */

    public override void OnInspectorGUI()
    {
        Boid boid = target as Boid;

        
        EditorGUILayout.LabelField("General Information", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        boid.maxSpeed = EditorGUILayout.FloatField("Max Speed", boid.maxSpeed);
        boid.maxForce = EditorGUILayout.FloatField("Max Force", boid.maxForce);
        boid.damping = EditorGUILayout.FloatField("Damping", boid.damping);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Speed: " + boid.velocity.magnitude);
        EditorGUILayout.LabelField("Velocity: " + boid.velocity);
        EditorGUILayout.LabelField("Acceleration: " + boid.acceleration);
        EditorGUILayout.LabelField("Force: " + boid.force);

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        boid.seekEnabled = GUILayout.Toggle(boid.seekEnabled, "Seeking", EditorStyles.radioButton);
        EditorGUILayout.EndHorizontal();
        if (boid.seekEnabled)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Seeking target position: " + boid.seekTargetPos);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        boid.arriveEnabled = GUILayout.Toggle(boid.arriveEnabled, "Arriving", EditorStyles.radioButton);
        EditorGUILayout.EndHorizontal();
        if (boid.arriveEnabled)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Arriving target position: " + boid.arriveTargetPos);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        boid.fleeEnabled = GUILayout.Toggle(boid.fleeEnabled, "Fleeing", EditorStyles.radioButton);
        EditorGUILayout.EndHorizontal();
        if (boid.fleeEnabled)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Fleeing away from: " + boid.fleeTargetPos);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        boid.formationFollowingEnabled = GUILayout.Toggle(boid.formationFollowingEnabled, "Formation Following", EditorStyles.radioButton);
        EditorGUILayout.EndHorizontal();
        if (boid.formationFollowingEnabled)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Formation position: " + boid.formationPos);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        boid.pursueEnabled = GUILayout.Toggle(boid.pursueEnabled, "Pursuing", EditorStyles.radioButton);
        EditorGUILayout.EndHorizontal();
        if (boid.pursueEnabled)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Pursuing target position: " + boid.pursueTargetPos);
        }
        EditorGUILayout.EndVertical();
    }
}
