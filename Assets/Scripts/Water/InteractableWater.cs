using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(EdgeCollider2D))]
[RequireComponent(typeof(WaterTriggerHandler))]
public class InteractableWater : MonoBehaviour
{

    [Header("Springs")]
    [SerializeField] private float springConstant = 1.4f;
    [SerializeField] private float damping = 1.1f;
    [SerializeField] private float spread = 6.5f;
    [SerializeField, Range(1, 10)] private int wavePropagationIterations = 8;
    [SerializeField, Range(0f, 20f)] private float speedMult = 5.5f;

    [Header("Force")]
    public float forceMultiplier = 0.2f;
    [Range(1f, 50f)] public float maxForce = 5f;


    [Header("Collision")]
    [SerializeField, Range(1f, 10f)] private float playerCollisionRadiusMult = 4.15f;

    [Header("Mesh Generation")]
    [Range(2, 500)] public int totalXVertices = 70;
    public float width = 10f;
    public float height = 4f;
    public Material waterMaterial;
    private const int totalYVertices = 2; //top and bottom

    [Header("Gizmo")]
    public Color GizmoColor = Color.white;


    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Vector3[] vertices;
    private int[] topVerticesIndex;

    private EdgeCollider2D coll;

    private class WaterPoint
    {
        public float velocity, position, targetHeight;
    }

    private List<WaterPoint> waterPoints = new List<WaterPoint>();

    public void Start()
    {
        coll = GetComponent<EdgeCollider2D>();
        GenerateMesh();
        CreateWaterPoints();
    }
    private void Reset()
    {
        coll = GetComponent<EdgeCollider2D>();
        coll.isTrigger = true;
    }

    private void FixedUpdate()
    {
        //Update Spring Positions
        for (int i = 1; i < waterPoints.Count - 1; i++)
        {
            WaterPoint point = waterPoints[i];

            float x = point.position - point.targetHeight;
            float acceleration = -springConstant * x - damping * point.velocity;
            point.position += point.velocity * speedMult * Time.fixedDeltaTime;
            vertices[topVerticesIndex[i]].y = point.position;
            point.velocity += acceleration * speedMult * Time.fixedDeltaTime;
        }

        //Wave Propagation
        for (int j = 0; j < wavePropagationIterations; j++)
        {
            for (int i = 1; i < waterPoints.Count - 1; i++)
            {
                float leftDelta = spread * (waterPoints[i].position - waterPoints[i - 1].position) * speedMult * Time.fixedDeltaTime;
                waterPoints[i - 1].velocity += leftDelta;

                float rightDelta = spread * (waterPoints[i].position - waterPoints[i + 1].position) * speedMult * Time.fixedDeltaTime;
                waterPoints[i + 1].velocity += rightDelta;
            }

        }

        //Update Mesh
        mesh.vertices = vertices;

    }

    public void Splash(Collider2D collision, float force)
    {
        float radius = collision.bounds.extents.x * playerCollisionRadiusMult;
        Vector2 center = collision.transform.position;

        for (int i = 0; i < waterPoints.Count; i++)
        {
            Vector2 vertexWorldPos = transform.TransformPoint(vertices[topVerticesIndex[i]]);

            if (IsPointInsideCircle(vertexWorldPos, center, radius))
                waterPoints[i].velocity = force;
        }
    }

    private bool IsPointInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        float distanceSquared = (point - center).sqrMagnitude;
        return distanceSquared <= radius * radius;
    }

    public void ResetEdgeCollider()
    {
        coll = GetComponent<EdgeCollider2D>();

        Vector2[] newPoints = new Vector2[2];

        Vector2 firstPoint = new Vector2(vertices[topVerticesIndex[0]].x, vertices[topVerticesIndex[0]].y);
        newPoints[0] = firstPoint;

        Vector2 secondPoint = new Vector2(vertices[topVerticesIndex[topVerticesIndex.Length - 1]].x, vertices[topVerticesIndex[topVerticesIndex.Length - 1]].y);
        newPoints[1] = secondPoint;

        coll.offset = Vector2.zero;
        coll.points = newPoints;
    }

    public void GenerateMesh()
    {
        mesh = new Mesh();

        //Add Vertices
        vertices = new Vector3[totalXVertices * totalYVertices];
        topVerticesIndex = new int[totalXVertices];
        for (int y = 0; y < totalYVertices; y++)
        {
            for (int x = 0; x < totalXVertices; x++)
            {
                float xPos = (x / (float)(totalXVertices - 1)) * width - width / 2;
                float yPos = (y / (float)(totalYVertices - 1)) * height - height / 2;
                vertices[y * totalXVertices + x] = new Vector3(xPos, yPos, 0f);
                if (y == totalYVertices - 1)
                {
                    topVerticesIndex[x] = y * totalXVertices + x;
                }

            }

        }

        //Construct Triangles
        int[] triangles = new int[(totalXVertices - 1) * (totalYVertices - 1) * 6];
        int index = 0;


        for (int y = 0; y < totalYVertices - 1; y++)
        {
            for (int x = 0; x < totalXVertices - 1; x++)
            {
                int bottomLeft = y * totalXVertices + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + totalXVertices;
                int topRight = topLeft + 1;

                //first triangle
                triangles[index++] = bottomLeft;
                triangles[index++] = topLeft;
                triangles[index++] = bottomRight;
                //second triangle
                triangles[index++] = bottomRight;
                triangles[index++] = topLeft;
                triangles[index++] = topRight;

            }

        }

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2((vertices[i].x + width / 2) / width, (vertices[i].y + height / 2) / height);
        }

        if(meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        if(meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();

        meshRenderer.material = waterMaterial;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

    }

    private void CreateWaterPoints()
    {
        waterPoints.Clear();

        for (int i = 0; i < topVerticesIndex.Length; i++)
        {
            waterPoints.Add(new WaterPoint
            {
                position = vertices[topVerticesIndex[i]].y,
                targetHeight = vertices[topVerticesIndex[i]].y,
            });
        }
    }

}

