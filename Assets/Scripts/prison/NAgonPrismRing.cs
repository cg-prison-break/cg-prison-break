using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NAgonPrismRing : MonoBehaviour
{
    [Header("Prisma Parameter")]
    [Range(3, 17)] public int sides = 7;
    public float innerRadius = 0.5f;
    public float outerRadius = 1f;
    public float height = 1f;

    private MeshFilter _meshFilter;

    private void OnValidate() => GeneratePrism();
    private void Reset() => GeneratePrism();

    /// <summary>
    /// Erstellt ein reguläres n-Eck-Prisma (z. B. Heptagon).
    /// </summary>
    private void GeneratePrism()
    {
        if (innerRadius < 0f) innerRadius = 0f;
        if (outerRadius <= innerRadius) outerRadius = innerRadius + 0.01f;

        _meshFilter ??= GetComponent<MeshFilter>();

        var mesh = new Mesh { name = "NAgonPrism" };
        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        CreateVertices(vertices);
        CreateTriangles(triangles);
        ApplyMeshData(mesh, vertices, triangles);

        _meshFilter.sharedMesh = mesh;
    }

    /// <summary>
    /// Erstellt die Punkte (Vertices) für Ober- und Unterseite des Prismas.
    /// </summary>
    private void CreateVertices(List<Vector3> vertices)
    {
        var halfHeight = height / 2f;
        var angleStep = 2 * Mathf.PI / sides;

        for (var sideIndex = 0; sideIndex < sides; sideIndex++)
        {
            var angle = sideIndex * angleStep;
            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);

            var outerTop    = new Vector3(cos * outerRadius,  halfHeight, sin * outerRadius);
            var innerTop    = new Vector3(cos * innerRadius,  halfHeight, sin * innerRadius);
            var outerBottom = new Vector3(cos * outerRadius, -halfHeight, sin * outerRadius);
            var innerBottom = new Vector3(cos * innerRadius, -halfHeight, sin * innerRadius);

            vertices.AddRange(new[] { outerTop, innerTop, outerBottom, innerBottom });
        }
    }

    /// <summary>
    /// Erstellt die Flächen (Triangles) für Seitenwände, Deckel und Boden.
    /// </summary>
    private void CreateTriangles(List<int> triangles)
    {
        for (var sideIndex = 0; sideIndex < sides; sideIndex++)
        {
            var nextSide = (sideIndex + 1) % sides;

            // Basisindex für diesen und den nächsten Eckpunkt
            var currentBase = sideIndex * 4;
            var nextBase = nextSide * 4;

            var topOuterCurrent = currentBase;
            var topInnerCurrent = currentBase + 1;
            var bottomOuterCurrent = currentBase + 2;
            var bottomInnerCurrent = currentBase + 3;

            var topOuterNext = nextBase;
            var topInnerNext = nextBase + 1;
            var bottomOuterNext = nextBase + 2;
            var bottomInnerNext = nextBase + 3;

            // outer wall
            triangles.AddRange(new[]
            {
                topOuterCurrent, topOuterNext, bottomOuterCurrent,
                topOuterNext, bottomOuterNext, bottomOuterCurrent
            });

            // inner wall (turned normals)
            triangles.AddRange(new[]
            {
                bottomInnerCurrent, bottomInnerNext, topInnerCurrent,
                bottomInnerNext, topInnerNext, topInnerCurrent
            });

            // top – anti-clockwise
            triangles.AddRange(new[]
            {
                topOuterCurrent, topInnerCurrent, topOuterNext,
                topOuterNext, topInnerCurrent, topInnerNext
            });

            // ground - clockwise
            triangles.AddRange(new[]
            {
                bottomOuterCurrent, bottomOuterNext, bottomInnerCurrent,
                bottomOuterNext, bottomInnerNext, bottomInnerCurrent
            });
        }
    }

    /// <summary>
    /// Wendet Vertices, UVs und Triangles auf das Mesh an und berechnet Normalen & Bounds.
    /// </summary>
    private void ApplyMeshData(Mesh mesh, List<Vector3> vertices, List<int> triangles)
    {
        var uvs = vertices
            .Select(v => new Vector2(v.x / (outerRadius * 2f) + 0.5f, v.z / (outerRadius * 2f) + 0.5f))
            .ToList();

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
