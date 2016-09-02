using UnityEngine;
using UnityEditor;

namespace GeneratedCuboids
{
    [InitializeOnLoad]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GeneratedCuboid))]
    public class GeneratedCuboidEditor : Editor
    {
        private const string MESH_DIALOGUE_TITLE = "Delete mesh data";
        private const string MESH_DIALOGUE_MESSAGE = "Do you want to delete the mesh data in your Assets?";
        SerializedProperty xProperty, yProperty, zProperty, forceVerticalMapProperty;
        private string output = string.Empty;
        private GeneratedCuboid[] targetCuboids;
        private GeneratedCuboid firstTargetCuboid;

        void OnEnable()
        {
            xProperty = serializedObject.FindProperty("x");
            yProperty = serializedObject.FindProperty("y");
            zProperty = serializedObject.FindProperty("z");
            forceVerticalMapProperty = serializedObject.FindProperty("forceVerticalMap");

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

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(forceVerticalMapProperty);
            serializedObject.ApplyModifiedProperties();
            //firstTargetCuboid.ForceVerticalMap = EditorGUILayout.Toggle("Force vertical mapping", firstTargetCuboid.ForceVerticalMap);

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

            if (targetCuboids.Length == 1)
            {
                EditorGUILayout.Space();
                firstTargetCuboid.UvSize = EditorGUILayout.IntField("UV size", firstTargetCuboid.UvSize);

                if (GUILayout.Button("Generate UV Map"))
                {
                    GenerateUVMap(firstTargetCuboid);
                }
            }

            EditorGUILayout.Space();

            if (!output.Equals(string.Empty))
            {
                GUILayout.Box(output);
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
            float x, y, z;
            bool forceVertical;
            AssignSizeValues(targetCuboid, out x, out y, out z, out forceVertical);

            AbstractCuboidMeshGenerator meshGenerator;
            if (forceVertical)
            {
                meshGenerator = new VerticalCuboidMeshGenerator(x, y, z);
            }
            else
            {
                meshGenerator = AbstractCuboidMeshGenerator.ConstructOptimalGenerator(x, y, z);
            }
            meshGenerator.CreateCuboid();
            Mesh newMesh = meshGenerator.GetMesh();

            SetNewMesh(targetCuboid, newMesh);
            AdaptCollider(targetCuboid, meshGenerator);

            targetCuboid.Uvs = meshGenerator.GetUVs();
        }

        private void AssignSizeValues(GeneratedCuboid targetCuboid, out float x, out float y, out float z, out bool forceVertical)
        {
            if (xProperty.hasMultipleDifferentValues)
            {
                x = targetCuboid.X;
            }
            else
            {
                x = xProperty.floatValue;
            }

            if (yProperty.hasMultipleDifferentValues)
            {
                y = targetCuboid.Y;
            }
            else
            {
                y = yProperty.floatValue;
            }

            if (zProperty.hasMultipleDifferentValues)
            {
                z = targetCuboid.Z;
            }
            else
            {
                z = zProperty.floatValue;
            }

            if(forceVerticalMapProperty.hasMultipleDifferentValues)
            {
                forceVertical = targetCuboid.ForceVerticalMap;
            }
            else
            {
                forceVertical = forceVerticalMapProperty.boolValue;
            }
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
