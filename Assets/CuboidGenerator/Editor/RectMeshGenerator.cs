using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace GeneratedCuboids
{
    public class RectMeshGenerator
    {
        protected float x, z;
        protected float colliderHeight = RectGenerator.DEFAULT_SIZE;
        protected Mesh mesh;
        protected GeneratedRect lastCreatedRect;
        protected List<Vector3> vertices;
        protected int[] tris;
        protected List<Vector2> uvs;
        protected float uvScale = 1;

        public RectMeshGenerator(float x, float z, float colliderHeight)
        {
            this.x = x;
            this.z = z;
            this.colliderHeight = colliderHeight;
        }

        public Mesh GetMesh()
        {
            return mesh;
        }

        public List<Vector2> GetUVs()
        {
            return uvs;
        }
        
        public void CreateRect()
        {
            mesh = new Mesh();
            mesh.name = "RectMesh";

            CreateVertices();
            CreateTris();
            FindUVScale();
            CreateUVs();
            AssignMeshComponents();
        }

        /// <summary>
        /// Is used, when the Cuboid is generated the first time
        /// </summary>
        public void CreateNewObject()
        {
            GameObject newObj = new GameObject();
            newObj.name = "Rect";
            Undo.RegisterCreatedObjectUndo(newObj, "Created Rect");

            BoxCollider col = AddCollider(newObj);
            AddGeneratedRect(newObj);
            AssignRectVariables(col);

            AddMeshFilter(newObj);
            lastCreatedRect.AddMaterial(newObj);
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
            float zH = z / 2f;

            vertices.Add(new Vector3(-xH, 0, -zH)); //0
            vertices.Add(new Vector3(xH, 0, -zH)); //1
            vertices.Add(new Vector3(-xH, 0, zH)); //2
            vertices.Add(new Vector3(xH, 0, zH)); //3
        }

        private void CreateTris()
        {
            tris = new int[6];

            //bottom
            tris[0] = 0;
            tris[1] = 2;
            tris[2] = 1;

            tris[3] = 3;
            tris[4] = 1;
            tris[5] = 2;
        }

        private void FindUVScale()
        {
            if(x >= z)
            {
                uvScale = 1f / x;
            }
            else
            {
                uvScale = 1f / z;
            }
        }

        private void CreateUVs()
        {
            uvs = new List<Vector2>();

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(x * uvScale, 0));
            uvs.Add(new Vector2(0, z * uvScale));
            uvs.Add(new Vector2(x * uvScale, z * uvScale));
        }

        private BoxCollider AddCollider(GameObject newObj)
        {
            BoxCollider col = newObj.AddComponent<BoxCollider>();
            AdjustCollider(col, colliderHeight);
            return col;
        }

        public void AdjustCollider(BoxCollider col, float colliderHeight)
        {
            col.center = new Vector3(0, -colliderHeight / 2f, 0);
            col.size = new Vector3(x, colliderHeight, z);
        }

        private void AddGeneratedRect(GameObject newObj)
        {
            newObj.AddComponent<GeneratedRect>();
        }

        public void AssignRectVariables(BoxCollider col)
        {
            GeneratedRect generatedRect = col.GetComponent<GeneratedRect>();
            generatedRect.X = x;
            generatedRect.Z = z;
            generatedRect.ColliderHeight = colliderHeight;
            generatedRect.ColliderCenter = col.transform.TransformPoint(col.center);
            lastCreatedRect = generatedRect;
        }

        public void SetParent(Transform parent)
        {
            if (parent != null && lastCreatedRect != null)
            {
                lastCreatedRect.gameObject.transform.parent = parent;
            }
        }

        private void AddMeshFilter(GameObject newObj)
        {
            MeshFilter meshFilter = newObj.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
        }
    }
}