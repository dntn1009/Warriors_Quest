using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(WallColliderGenerator))]
public class WallColliderGeneratorEditor : Editor
{
    WallColliderGenerator wall;

    void OnEnable()
    {
        wall = target as WallColliderGenerator;
        UnityEditorInternal.ComponentUtility.MoveComponentUp(wall);
        wall.OnEnableCallback();
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        wall.height = EditorGUILayout.FloatField("Height", wall.height);
        wall.radius = EditorGUILayout.FloatField("Radius", wall.radius);

        if (EditorGUI.EndChangeCheck())
            wall.UpdateHeightRadius();

        if (GUILayout.Button("Clear Collider"))
            wall.ClearCollider();

        if (GUILayout.Button("Create Collider"))
            wall.CreateCollider();
    }
}

#endif

[RequireComponent(typeof(LineRenderer))]
public class WallColliderGenerator : MonoBehaviour
{
    public float height = 3;
    public float radius = 0.5f;
    float Diameter => radius * 2;
    LineRenderer lineRenderer;

    public void OnEnableCallback()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.receiveShadows = false;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = Diameter;
        lineRenderer.endWidth = Diameter;
        lineRenderer.numCornerVertices = 10;
        lineRenderer.numCapVertices = 10;

        if (lineRenderer.sharedMaterial == null)
        {
            var material = new Material(Shader.Find("Sprites/Default"));
            material.color = Color.gray;
            lineRenderer.sharedMaterial = material;
        }
    }

    public void ClearCollider()
    {
        Array.ForEach(GetComponents<CapsuleCollider>(), x => DestroyImmediate(x));

        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }

    public void CreateCollider()
    {
        ClearCollider();

        int size = lineRenderer.positionCount;
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < size; i++)
        {
            var capsule = gameObject.AddComponent<CapsuleCollider>();
            points.Add(lineRenderer.GetPosition(i));

            capsule.center = lineRenderer.GetPosition(i);
            capsule.height = height;
            capsule.radius = radius;
        }

        for (int i = 0; i < size - 1; i++)
        {
            var box = new GameObject().AddComponent<BoxCollider>().transform;
            box.name = "Box";
            box.SetParent(transform);

            box.localPosition = (points[i] + points[i + 1]) * 0.5f;
            box.localRotation = Quaternion.LookRotation(points[i + 1] - points[i]);
            box.localScale = new Vector3(Diameter, height - Diameter, Vector3.Distance(points[i], points[i + 1]));
        }
    }

    public void UpdateHeightRadius()
    {
        Array.ForEach(GetComponents<CapsuleCollider>(), x => { x.radius = radius; x.height = height; });

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = new Vector3(Diameter, height - Diameter, transform.GetChild(i).localScale.z);
        }

        lineRenderer.startWidth = Diameter;
        lineRenderer.endWidth = Diameter;
    }
}
