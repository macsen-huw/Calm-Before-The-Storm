using System;
using UnityEditor.UIElements;

[CustomEditor(typeof(InteractableWater))]
public class InteractableWaterEditor : Editor
{
    private InteractableWater water;

    private void OnEnable()
    {
        water = (InteractableWater)target;
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        root.Add(new VisualElement { style = { height = 10 } });

        Button generateMeshButton = new Button(() => water.GenerateMesh())
        {
            text = "Generate Mesh"
        };

        root.Add(generateMeshButton);

        Button placeEdgeColliderButton = new Button(() => water.ResetEdgeCollider())
        {
            text = "Place Edge Collide"
        };

        root.Add(placeEdgeColliderButton);

        return root;
    }

    private void ChangeDimensions(ref float width, ref float height, float calculatedwidthMax, float calculatedheightMax)
    {
        width = Mathf.Max(0.1f, calculatedwidthMax);
        height = Mathf.Max(0.1f, calculatedheightMax);
    }

    private void OnSceneGUI()
    {
        // Draw the wireframe box
        Handles.color = water.GizmoColor;
        Vector3 center = water.transform.position;
        Vector3 size = new Vector3(water.width, water.height, 0.1f);
        Handles.DrawWireCube(center, size);

        // Handles for width and height
        float handleSize = HandleUtility.GetHandleSize(center) * 0.1f;
        Vector3 snap = Vector3.one * 0.1f;

        // Corner handles
        Vector3[] corners = new Vector3[4];
        corners[0] = center + new Vector3(-water.width / 2, -water.height / 2, 0); // Bottom-left
        corners[1] = center + new Vector3(water.width / 2, -water.height / 2, 0); // Bottom-right
        corners[2] = center + new Vector3(-water.width / 2, water.height / 2, 0); // Top-left
        corners[3] = center + new Vector3(water.width / 2, water.height / 2, 0); // Top-right

        // Handle for each corner
        EditorGUI.BeginChangeCheck();
        Vector3 newBottomLeft = Handles.FreeMoveHandle(corners[0], handleSize, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            ChangeDimensions(ref water.width, ref water.height, corners[1].x - newBottomLeft.x, corners[3].y - newBottomLeft.y);
            water.transform.position += new Vector3((newBottomLeft.x - corners[0].x) / 2, (newBottomLeft.y - corners[0].y) / 2, 0);
        }

        EditorGUI.BeginChangeCheck();
        Vector3 newBottomRight = Handles.FreeMoveHandle(corners[1], handleSize, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            ChangeDimensions(ref water.width, ref water.height, newBottomRight.x - corners[0].x, corners[3].y - newBottomRight.y);
            water.transform.position += new Vector3((newBottomRight.x - corners[1].x) / 2, (newBottomRight.y - corners[1].y) / 2, 0);
        }

        EditorGUI.BeginChangeCheck();
        Vector3 newTopLeft = Handles.FreeMoveHandle(corners[2], handleSize, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            ChangeDimensions(ref water.width, ref water.height, corners[3].x - newTopLeft.x, newTopLeft.y - corners[0].y);
            water.transform.position += new Vector3((newTopLeft.x - corners[2].x) / 2, (newTopLeft.y - corners[2].y) / 2, 0);
        }

        EditorGUI.BeginChangeCheck();
        Vector3 newTopRight = Handles.FreeMoveHandle(corners[3], handleSize, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            ChangeDimensions(ref water.width, ref water.height, newTopRight.x - corners[2].x, newTopRight.y - corners[1].y);
            water.transform.position += new Vector3((newTopRight.x - corners[3].x) / 2, (newTopRight.y - corners[3].y) / 2, 0);
        }

        // Update the mesh if the handles are moved
        if (GUI.changed)
        {
            water.GenerateMesh();
        }
    }

}
