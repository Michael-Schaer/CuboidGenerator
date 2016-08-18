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
            vertices.Add(new Vector3(-xH, yH, zH)); //6
            vertices.Add(new Vector3(xH, yH, zH)); //7
        }

        protected override void CreateTopTris()
        {
            tris[12] = 4;
            tris[13] = 6;
            tris[14] = 5;

            tris[15] = 7;
            tris[16] = 5;
            tris[17] = 6;
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
            tempVec = new Vector2(y * uvScale, (y + z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2((x + y) * uvScale, (y + z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2(y * uvScale, (y + 2 * z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2((x + y) * uvScale, (y + 2 * z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2(y * uvScale, z * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2((x + y) * uvScale, z * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2(y * uvScale, 0);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2((x + y) * uvScale, 0);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2(y * uvScale, (2 * y + 2 * z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2((x + y) * uvScale, (2 * y + 2 * z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2(0, (y + z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2(0, (y + 2 * z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2((x + 2 * y) * uvScale, (y + z) * uvScale);
            uvCoordinates += AddToUVs();
            tempVec = new Vector2((x + 2 * y) * uvScale, (y + 2 * z) * uvScale);
            uvCoordinates += AddToUVs();
        }
    }
}