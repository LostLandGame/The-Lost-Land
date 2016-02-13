using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LostLand.Combat.Map;

namespace LostLand.Combat.Visuals
{
    public class MoveRangeIndicator : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter filter;

        [SerializeField]
        private int numPoints = 12;
        [SerializeField]
        private float offset = 2f;

        public MapGenerator gen;

        private Mesh circleMesh = null;

        [ContextMenu("Test circle")]
        public void Test()
        {
            //MakeCircle(new Vector2(3f, 3f), 5f);
            MakeCone(new Vector2(3f, 3f), 5f, Vector2.right, 90f);
        }

        private void MakeCircle(Vector2 origin, float maxDistance)
        {
            float angleStep = Mathf.PI * 2f / numPoints;
            Vector3[] vertices = new Vector3[numPoints + 1];
            int[] triangles = new int[numPoints * 3];
            float currentAngle = 0f;

            // TODO: Add UVs

            Vector2 currentForward = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
            Vector2 currentEnd = Vector2.zero;

            // Make first triangle.
            vertices[0] = new Vector3(origin.x, offset, origin.y);

            for (int i = 1; i < numPoints + 1; i++)
            {
                currentAngle += angleStep;
                currentForward = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
                //currentEnd = MapGenerator.GetEndPoint(origin, currentForward, maxDistance);
                currentEnd = gen.HelpGetEndPoint(origin, currentForward, maxDistance);

                vertices[i] = new Vector3(currentEnd.x, offset, currentEnd.y);
            }

            for (int i = 0; i < numPoints - 1; ++i)
            {
                triangles[i * 3] = 0;                      // Index of circle center.
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }

            triangles[(numPoints - 1) * 3] = 0;                      // Index of circle center.
            triangles[(numPoints - 1) * 3 + 1] = 1;
            triangles[(numPoints - 1) * 3 + 2] = numPoints;

            if (circleMesh == null)
            {
                circleMesh = new Mesh();
                circleMesh.name = "Range Indicator";
            }

            circleMesh.vertices = vertices;
            circleMesh.triangles = triangles;

            circleMesh.RecalculateBounds();
            circleMesh.RecalculateNormals();

            if (filter.sharedMesh == null)
            {
                filter.sharedMesh = circleMesh;
            }
        }

        private void MakeCone(Vector2 origin, float maxDistance, Vector2 facing, float angle)
        {
            float angleStep = Mathf.PI * angle / 180f / numPoints; // Angle to radians by points
            Vector3[] vertices = new Vector3[numPoints + 1];
            int[] triangles = new int[(numPoints - 1) * 3];
            float currentAngle = Vector2.Angle(Vector2.right, facing);

            // TODO: Add UVs

            Vector2 currentForward = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
            Vector2 currentEnd = Vector2.zero;

            // Make first triangle.
            vertices[0] = new Vector3(origin.x, offset, origin.y);

            for (int i = 1; i < numPoints + 1; i++)
            {
                currentAngle += angleStep;
                currentForward = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
                //currentEnd = MapGenerator.GetEndPoint(origin, currentForward, maxDistance);
                currentEnd = gen.HelpGetEndPoint(origin, currentForward, maxDistance);

                vertices[i] = new Vector3(currentEnd.x, offset, currentEnd.y);
            }

            for (int i = 0; i < numPoints - 1; ++i)
            {
                triangles[i * 3] = 0;                      // Index of circle center.
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }

            if (circleMesh == null)
            {
                circleMesh = new Mesh();
                circleMesh.name = "Range Indicator";
            }

            circleMesh.vertices = vertices;
            circleMesh.triangles = triangles;

            circleMesh.RecalculateBounds();
            circleMesh.RecalculateNormals();

            if (filter.sharedMesh == null)
            {
                filter.sharedMesh = circleMesh;
            }
        }
    }
}