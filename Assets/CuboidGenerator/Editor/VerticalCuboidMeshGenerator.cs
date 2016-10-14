using UnityEngine;

namespace GeneratedCuboids
{
    public class VerticalCuboidMeshGenerator : AbstractCuboidMeshGenerator
    {
        public VerticalCuboidMeshGenerator(float x, float y, float z) : base(x, y, z)
        {
        }

        protected override void CreateTopVertices(float xH, float yH, float zH)
        {
            vertices.Add(new Vector3(-xH, yH, zH)); //8
            vertices.Add(new Vector3(xH, yH, zH)); //9
            vertices.Add(new Vector3(-xH, yH, -zH)); //10
            vertices.Add(new Vector3(xH, yH, -zH)); //11
        }

        protected override void CreateTopTris()
        {
            tris[12] = 10;
            tris[13] = 8;
            tris[14] = 11;

            tris[15] = 9;
            tris[16] = 11;
            tris[17] = 8;
        }

        protected override void FindUVScale()
        {
            float height = 2 * y + 2 * z;
            float width = 2 * y + x;

            if (height > width)
            {
                uvScale = 1 / height;
            }
            else
            {
                uvScale = 1 / width;
            }
        }

        protected override void AssignUVCoordinates()
        {
            uvs.Add(new Vector2(y * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2(y * uvScale, (y + 2 * z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + 2 * z) * uvScale));

            uvs.Add(new Vector2(y * uvScale, z * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, z * uvScale));
            uvs.Add(new Vector2(y * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + z) * uvScale));

            uvs.Add(new Vector2(y * uvScale, 0));
            uvs.Add(new Vector2((x + y) * uvScale, 0));
            uvs.Add(new Vector2(y * uvScale, z * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, z * uvScale));

            uvs.Add(new Vector2(y * uvScale, (2 * y + 2 * z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (2 * y + 2 * z) * uvScale));
            uvs.Add(new Vector2(y * uvScale, (y + 2 * z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + 2 * z) * uvScale));

            uvs.Add(new Vector2(0, (y + z) * uvScale));
            uvs.Add(new Vector2(0, (y + 2 * z) * uvScale));
            uvs.Add(new Vector2(y * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2(y * uvScale, (y + 2 * z) * uvScale));

            uvs.Add(new Vector2((x + 2 * y) * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2((x + 2 * y) * uvScale, (y + 2 * z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + 2 * z) * uvScale));
        }
    }
}