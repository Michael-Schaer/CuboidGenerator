using UnityEngine;

namespace GeneratedCuboids
{
    public class HorizontalCuboidMeshGenerator : AbstractCuboidMeshGenerator
    {
        public HorizontalCuboidMeshGenerator(float x, float y, float z) : base(x, y, z)
        {
        }

        protected override void CreateTopVertices(float xH, float yH, float zH)
        {
            vertices.Add(new Vector3(-xH, yH, -zH)); //8
            vertices.Add(new Vector3(-xH, yH, zH)); //9
            vertices.Add(new Vector3(xH, yH, -zH)); //10
            vertices.Add(new Vector3(xH, yH, zH)); //11
        }

        protected override void CreateTopTris()
        {
            tris[12] = 11;
            tris[13] = 10;
            tris[14] = 9;

            tris[15] = 8;
            tris[16] = 9;
            tris[17] = 10;
        }

        protected override void FindUVScale()
        {
            float height = 2 * y + z;
            float width = 2 * x + 2 * y;

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
            uvs.Add(new Vector2(y * uvScale, y * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, y * uvScale));
            uvs.Add(new Vector2(y * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + z) * uvScale));

            uvs.Add(new Vector2(y * uvScale, 0));
            uvs.Add(new Vector2((x + y) * uvScale, 0));
            uvs.Add(new Vector2(y * uvScale, y * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, y * uvScale));

            uvs.Add(new Vector2((2 * y + 2 * x) * uvScale, y * uvScale));
            uvs.Add(new Vector2((2 * y + 2 * x) * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2((2 * y + x) * uvScale, y * uvScale));
            uvs.Add(new Vector2((2 * y + x) * uvScale, (y + z) * uvScale));

            uvs.Add(new Vector2(y * uvScale, (2 * y + z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (2 * y + z) * uvScale));
            uvs.Add(new Vector2(y * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + z) * uvScale));

            uvs.Add(new Vector2(0, y * uvScale));
            uvs.Add(new Vector2(0, (y + z) * uvScale));
            uvs.Add(new Vector2(y * uvScale, y * uvScale));
            uvs.Add(new Vector2(y * uvScale, (y + z) * uvScale));

            uvs.Add(new Vector2((x + 2 * y) * uvScale, y * uvScale));
            uvs.Add(new Vector2((x + 2 * y) * uvScale, (y + z) * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, y * uvScale));
            uvs.Add(new Vector2((x + y) * uvScale, (y + z) * uvScale));
        }
    }
}