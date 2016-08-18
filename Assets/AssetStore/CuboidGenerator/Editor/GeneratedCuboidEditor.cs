using UnityEngine;
using UnityEditor;

namespace GeneratedCuboids
{
    [InitializeOnLoad]
    [CustomEditor(typeof(GeneratedCuboid))]
    public class GeneratedCuboidEditor : Editor
    {
        private float x, y, z;
        private string output = "Use this to adjust the size of the cube";
        private bool firstUpdate = true;
        private GeneratedCuboid targetCuboid;

        private void OnInitialize()
        {
            targetCuboid = (GeneratedCuboid)target;

            x = targetCuboid.X;
            y = targetCuboid.Y;
            z = targetCuboid.Z;

            if (!targetCuboid.Output.Equals(string.Empty))
            {
                output = targetCuboid.Output;
            }
        }

        public override void OnInspectorGUI()
        {
            if (firstUpdate)
            {
                firstUpdate = false;
                OnInitialize();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Cuboid Size");
            x = EditorGUILayout.FloatField("x", x);
            y = EditorGUILayout.FloatField("y", y);
            z = EditorGUILayout.FloatField("z", z);

            EditorGUILayout.Space();
            targetCuboid.ForceVerticalMap = EditorGUILayout.Toggle("Force vertical mapping", targetCuboid.ForceVerticalMap);

            if (GUILayout.Button("Resize"))
            {
                RecreateCuboidAndSetColliderCenter();
            }

            if (GUILayout.Button("Resize to Collider bounds (\"C\")"))
            {
                ResizeToColliderBounds();
            }

            EditorGUILayout.Space();
            targetCuboid.UvSize = EditorGUILayout.IntField("UV size", targetCuboid.UvSize);

            if (GUILayout.Button("Generate UV Map"))
            {
                GenerateUVMap();
            }

            EditorGUILayout.Space();
            GUILayout.Box(output);
        }

        private void ResizeToColliderBounds()
        {
            BoxCollider col = targetCuboid.GetComponent<BoxCollider>();
            if (col != null)
            {
                Vector3 newCenter = col.transform.TransformPoint(col.center);
                x = col.size.x;
                y = col.size.y;
                z = col.size.z;
                RecreateCuboid();
                targetCuboid.MoveToColliderCenter(newCenter);
            }
        }

        void OnSceneGUI()
        {
            if (Selection.activeGameObject == null || targetCuboid == null)
            {
                return;
            }

            if (Selection.activeGameObject.GetInstanceID() == targetCuboid.gameObject.GetInstanceID())
            {
                if (Event.current.type == EventType.keyDown)
                {
                    if (Event.current.keyCode == (KeyCode.C))
                    {
                        ResizeToColliderBounds();
                    }
                }
            }
        }

        private void RecreateCuboidAndSetColliderCenter()
        {
            RecreateCuboid();

            BoxCollider col = targetCuboid.GetComponent<BoxCollider>();
            if (col != null)
            {
                targetCuboid.ColliderCenter = col.transform.TransformPoint(col.center);
            }
        }

        private void RecreateCuboid()
        {
            AbstractCuboidMeshGenerator meshGenerator;
            if (targetCuboid.ForceVerticalMap)
            {
                meshGenerator = new VerticalCuboidMeshGenerator(x, y, z);
            }
            else
            {
                meshGenerator = AbstractCuboidMeshGenerator.ConstructOptimalGenerator(x, y, z);
            }
            meshGenerator.CreateCuboid();
            Mesh newMesh = meshGenerator.GetMesh();

            SetNewMesh(newMesh);
            AdaptCollider(meshGenerator);

            targetCuboid.Uvs = meshGenerator.GetUVs();
        }

        private void AdaptCollider(AbstractCuboidMeshGenerator meshGenerator)
        {
            BoxCollider col = targetCuboid.GetComponent<BoxCollider>();
            if (col != null)
            {
                Undo.RecordObject(col, "Collider bounds change");
                meshGenerator.AssignCuboidVariables(col);
                meshGenerator.AdjustCollider(col);
            }
        }

        private void SetNewMesh(Mesh newMesh)
        {
            MeshFilter meshFilter = targetCuboid.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Undo.RecordObject(meshFilter, "MeshFilter mesh change");
                meshFilter.mesh = newMesh;
            }
        }

        private void GenerateUVMap()
        {
            UVBitmapGenerator generator = new UVBitmapGenerator();
            output = generator.StoreUVMap(targetCuboid.Uvs, targetCuboid.UvSize);
            AssetDatabase.Refresh();
        }
    }
}
