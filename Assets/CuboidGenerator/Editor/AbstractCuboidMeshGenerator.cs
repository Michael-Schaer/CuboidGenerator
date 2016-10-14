using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace GeneratedCuboids
{
    public abstract class AbstractCuboidMeshGenerator
    {
        protected float x, y, z;
        protected float uvScale = 1;
        protected Mesh mesh;
        protected List<Vector3> vertices;
        protected int[] tris;
        protected List<Vector2> uvs;
        protected GeneratedCuboid lastCreatedCuboid;

        protected abstract void CreateTopVertices(float xH, float yH, float zH);
        protected abstract void CreateTopTris();
        protected abstract void AssignUVCoordinates();
        protected abstract void FindUVScale();

        public AbstractCuboidMeshGenerator(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static AbstractCuboidMeshGenerator ConstructOptimalGenerator(float x, float y, float z)
        {
            if (x >= z)
            {
                return new VerticalCuboidMeshGenerator(x, y, z);
            }
            else
            {
                return new HorizontalCuboidMeshGenerator(x, y, z);
            }
        }

        public Mesh GetMesh()
        {
            return mesh;
        }

        public List<Vector2> GetUVs()
        {
            return uvs;
        }

        public void CreateCuboid()
        {
            mesh = new Mesh();
            mesh.name = "CuboidMesh";

            CreateVertices();
            CreateTris();
            FindUVScale();
            CreateUVs();
            AssignMeshComponents();
        }

        /// <summary>
        /// Is used, when the Cuboid is generated the first time
        /// </summary>
        public GeneratedCuboid CreateNewObject()
        {
            GameObject newObj = new GameObject();
            newObj.name = "Cuboid";
            Undo.RegisterCreatedObjectUndo(newObj, "Created Cuboid");

            BoxCollider col = AddCollider(newObj);
            AddGeneratedCube(newObj);
            AssignCuboidVariables(col);

            AddMeshFilter(newObj);
            lastCreatedCuboid.AddMaterial(newObj);
            return lastCreatedCuboid;
        }

        private void AssignMeshComponents()
        {
            mesh.SetVertices(vertices);
            mesh.SetTriangles(tris, 0);
            mesh.SetUVs(0, uvs);
            mesh.RecalculateNormals();
        }

        private void CreateVertices()
        {
            vertices = new List<Vector3>();

            float xH = x / 2f;
            float yH = y / 2f;
            float zH = z / 2f;

            //bottom
            vertices.Add(new Vector3(-xH, -yH, -zH)); //0
            vertices.Add(new Vector3(xH, -yH, -zH)); //1
            vertices.Add(new Vector3(-xH, -yH, zH)); //2
            vertices.Add(new Vector3(xH, -yH, zH)); //3

            //front
            vertices.Add(new Vector3(-xH, -yH, -zH)); //4
            vertices.Add(new Vector3(xH, -yH, -zH)); //5
            vertices.Add(new Vector3(-xH, yH, -zH)); //6
            vertices.Add(new Vector3(xH, yH, -zH)); //7

            //top
            CreateTopVertices(xH, yH, zH);

            //back
            vertices.Add(new Vector3(-xH, yH, zH)); //12
            vertices.Add(new Vector3(xH, yH, zH)); //13
            vertices.Add(new Vector3(-xH, -yH, zH)); //14
            vertices.Add(new Vector3(xH, -yH, zH)); //15

            //left
            vertices.Add(new Vector3(-xH, yH, -zH)); // 16
            vertices.Add(new Vector3(-xH, yH, zH)); // 17
            vertices.Add(new Vector3(-xH, -yH, -zH)); // 18
            vertices.Add(new Vector3(-xH, -yH, zH)); // 19

            //right
            vertices.Add(new Vector3(xH, yH, -zH)); // 20
            vertices.Add(new Vector3(xH, yH, zH)); // 21
            vertices.Add(new Vector3(xH, -yH, -zH)); // 22
            vertices.Add(new Vector3(xH, -yH, zH)); // 23
        }

        private void CreateTris()
        {
            tris = new int[36];

            //bottom
            tris[0] = 0;
            tris[1] = 1;
            tris[2] = 2;

            tris[3] = 3;
            tris[4] = 2;
            tris[5] = 1;

            //front
            tris[6] = 4;
            tris[7] = 6;
            tris[8] = 7;

            tris[9] = 5;
            tris[10] = 4;
            tris[11] = 7;

            //top
            CreateTopTris();

            //back
            tris[18] = 14;
            tris[19] = 15;
            tris[20] = 12;

            tris[21] = 13;
            tris[22] = 12;
            tris[23] = 15;

            //left
            tris[24] = 16;
            tris[25] = 18;
            tris[26] = 17;

            tris[27] = 19;
            tris[28] = 17;
            tris[29] = 18;

            //right
            tris[30] = 22;
            tris[31] = 20;
            tris[32] = 23;

            tris[33] = 21;
            tris[34] = 23;
            tris[35] = 20;
        }

        private void CreateUVs()
        {
            uvs = new List<Vector2>();
            AssignUVCoordinates();
        }

        private BoxCollider AddCollider(GameObject newObj)
        {
            BoxCollider col = newObj.AddComponent<BoxCollider>();
            AdjustCollider(col);
            return col;
        }

        public void AdjustCollider(BoxCollider col)
        {
            col.center = new Vector3(0, 0, 0);
            col.size = new Vector3(x, y, z);
        }

        private void AddGeneratedCube(GameObject newObj)
        {
            newObj.AddComponent<GeneratedCuboid>();
        }

        public void AssignCuboidVariables(BoxCollider col)
        {
            GeneratedCuboid generatedCuboid = col.GetComponent<GeneratedCuboid>();
            generatedCuboid.X = x;
            generatedCuboid.Y = y;
            generatedCuboid.Z = z;
            generatedCuboid.ColliderCenter = col.transform.TransformPoint(col.center);
            generatedCuboid.Uvs = uvs;
            lastCreatedCuboid = generatedCuboid;
        }

        public void SetParent(Transform parent)
        {
            if (parent != null && lastCreatedCuboid != null)
            {
                lastCreatedCuboid.gameObject.transform.parent = parent;
            }
        }

        private void AddMeshFilter(GameObject newObj)
        {
            MeshFilter meshFilter = newObj.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
        }
    }
}