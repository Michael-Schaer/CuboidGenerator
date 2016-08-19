using UnityEngine;
using UnityEditor;

namespace GeneratedCuboids
{
    [InitializeOnLoad]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GeneratedCuboid))]
    public class GeneratedCuboidEditor : Editor
    {
        SerializedProperty xProperty, yProperty, zProperty;
        private string output = "Use this to adjust the size of the cube";
        private GeneratedCuboid[] targetCuboids;
        private GeneratedCuboid firstTargetCuboid;

        void OnEnable()
        {
            xProperty = serializedObject.FindProperty("x");
            yProperty = serializedObject.FindProperty("y");
            zProperty = serializedObject.FindProperty("z");

            targetCuboids = System.Array.ConvertAll(targets, item => (GeneratedCuboid)item);
            firstTargetCuboid = targetCuboids[0];

            if (!firstTargetCuboid.Output.Equals(string.Empty))
            {
                output = firstTargetCuboid.Output;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Cuboid Size");

            serializedObject.Update();
            EditorGUILayout.PropertyField(xProperty);
            EditorGUILayout.PropertyField(yProperty);
            EditorGUILayout.PropertyField(zProperty);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            firstTargetCuboid.ForceVerticalMap = EditorGUILayout.Toggle("Force vertical mapping", firstTargetCuboid.ForceVerticalMap);

            if (GUILayout.Button("Resize"))
            {
                foreach (GeneratedCuboid targetCuboid in targetCuboids)
                {
                    RecreateCuboidAndSetColliderCenter(targetCuboid);
                }
            }

            if (GUILayout.Button("Resize to Collider bounds (\"C\")"))
            {
                foreach (GeneratedCuboid targetCuboid in targetCuboids)
                {
                    ResizeToColliderBounds(targetCuboid);
                }
            }

            EditorGUILayout.Space();
            firstTargetCuboid.UvSize = EditorGUILayout.IntField("UV size", firstTargetCuboid.UvSize);

            if (GUILayout.Button("Generate UV Map"))
            {
                GenerateUVMap(firstTargetCuboid);
            }

            EditorGUILayout.Space();
            GUILayout.Box(output);
        }

        private void ResizeToColliderBounds(GeneratedCuboid targetCuboid)
        {
            BoxCollider col = targetCuboid.GetComponent<BoxCollider>();
            if (col != null)
            {
                Vector3 newCenter = col.transform.TransformPoint(col.center);
                xProperty.floatValue = col.size.x;
                yProperty.floatValue = col.size.y;
                zProperty.floatValue = col.size.z;
                RecreateCuboid(targetCuboid);
                targetCuboid.MoveToColliderCenter(newCenter);
            }
        }

        void OnSceneGUI()
        {
            if (Selection.activeGameObject == null || firstTargetCuboid == null)
            {
                return;
            }

            if (Selection.activeGameObject.GetInstanceID() == firstTargetCuboid.gameObject.GetInstanceID())
            {
                if (Event.current.type == EventType.keyDown)
                {
                    if (Event.current.keyCode == (KeyCode.C))
                    {
                        ResizeToColliderBounds(firstTargetCuboid);
                    }
                }
            }
        }

        private void RecreateCuboidAndSetColliderCenter(GeneratedCuboid targetCuboid)
        {
            RecreateCuboid(targetCuboid);

            BoxCollider col = targetCuboid.GetComponent<BoxCollider>();
            if (col != null)
            {
                targetCuboid.ColliderCenter = col.transform.TransformPoint(col.center);
            }
        }

        private void RecreateCuboid(GeneratedCuboid targetCuboid)
        {
            AbstractCuboidMeshGenerator meshGenerator;
            if (targetCuboid.ForceVerticalMap)
            {
                meshGenerator = new VerticalCuboidMeshGenerator(xProperty.floatValue, yProperty.floatValue, zProperty.floatValue);
            }
            else
            {
                meshGenerator = AbstractCuboidMeshGenerator.ConstructOptimalGenerator(xProperty.floatValue, yProperty.floatValue, zProperty.floatValue);
            }
            meshGenerator.CreateCuboid();
            Mesh newMesh = meshGenerator.GetMesh();

            SetNewMesh(targetCuboid, newMesh);
            AdaptCollider(targetCuboid, meshGenerator);

            targetCuboid.Uvs = meshGenerator.GetUVs();
        }

        private void AdaptCollider(GeneratedCuboid targetCuboid, AbstractCuboidMeshGenerator meshGenerator)
        {
            BoxCollider col = targetCuboid.GetComponent<BoxCollider>();
            if (col != null)
            {
                Undo.RecordObject(col, "Collider bounds change");
                meshGenerator.AssignCuboidVariables(col);
                meshGenerator.AdjustCollider(col);
            }
        }

        private void SetNewMesh(GeneratedCuboid targetCuboid, Mesh newMesh)
        {
            MeshFilter meshFilter = targetCuboid.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Undo.RecordObject(meshFilter, "MeshFilter mesh change");
                meshFilter.mesh = newMesh;
            }
        }

        private void GenerateUVMap(GeneratedCuboid targetCuboid)
        {
            UVBitmapGenerator generator = new UVBitmapGenerator();
            output = generator.StoreUVMap(targetCuboid.Uvs, targetCuboid.UvSize);
            AssetDatabase.Refresh();
        }
    }
}
