using UnityEngine;
using UnityEditor;
using System;

namespace GeneratedCuboids
{
    [InitializeOnLoad]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GeneratedRect))]
    public class GeneratedRectEditor : Editor
    {
        SerializedProperty xProperty, zProperty, heightProperty;
        private string output = String.Empty;
        private GeneratedRect[] targetRects;
        private GeneratedRect firstTargetRect;

        void OnEnable()
        {
            xProperty = serializedObject.FindProperty("x");
            zProperty = serializedObject.FindProperty("z");
            heightProperty = serializedObject.FindProperty("colliderHeight");

            targetRects = System.Array.ConvertAll(targets, item => (GeneratedRect)item);
            firstTargetRect = targetRects[0];

            if (!firstTargetRect.Output.Equals(string.Empty))
            {
                output = firstTargetRect.Output;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rect Size");

            serializedObject.Update();
            EditorGUILayout.PropertyField(xProperty);
            EditorGUILayout.PropertyField(zProperty);
            EditorGUILayout.PropertyField(heightProperty);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            if (GUILayout.Button("Resize"))
            {
                foreach (GeneratedRect targetRect in targetRects)
                {
                    RecreateRectAndSetColliderCenter(targetRect);
                }
            }

            if (GUILayout.Button("Resize to Collider bounds (\"C\")"))
            {
                foreach (GeneratedRect targetRect in targetRects)
                {
                    ResizeToColliderBounds(targetRect);
                }
            }

            if (targetRects.Length == 1)
            {
                EditorGUILayout.Space();
                firstTargetRect.UvSize = EditorGUILayout.IntField("UV size", firstTargetRect.UvSize);

                if (GUILayout.Button("Generate UV Map"))
                {
                    GenerateUVMap(firstTargetRect);
                }
            }
            EditorGUILayout.Space();

            if (!output.Equals(string.Empty))
            {
                GUILayout.Box(output);
            }
        }

        private void ResizeToColliderBounds(GeneratedRect targetRect)
        {
            BoxCollider col = targetRect.GetComponent<BoxCollider>();
            if (col != null)
            {
                Vector3 newCenter = col.transform.TransformPoint(col.center);
                xProperty.floatValue = col.size.x;
                zProperty.floatValue = col.size.z;
                heightProperty.floatValue = col.size.y;
                RecreateRect(targetRect);
                targetRect.MoveToColliderCenter(newCenter);
            }
        }

        void OnSceneGUI()
        {
            if (Selection.activeGameObject == null || firstTargetRect == null)
            {
                return;
            }

            if (Selection.activeGameObject.GetInstanceID() == firstTargetRect.gameObject.GetInstanceID())
            {
                if (Event.current.type == EventType.keyDown)
                {
                    if (Event.current.keyCode == (KeyCode.C))
                    {
                        ResizeToColliderBounds(firstTargetRect);
                    }
                }
            }
        }

        private void RecreateRectAndSetColliderCenter(GeneratedRect targetRect)
        {
            RecreateRect(targetRect);

            BoxCollider col = targetRect.GetComponent<BoxCollider>();
            if (col != null)
            {
                targetRect.ColliderCenter = col.transform.TransformPoint(col.center);
            }
        }

        private void RecreateRect(GeneratedRect targetRect)
        {
            float x, z, height;
            AssignSizeValues(targetRect, out x, out z, out height);

            RectMeshGenerator meshGenerator = new RectMeshGenerator(x, z, height);
            meshGenerator.CreateRect();
            Mesh newMesh = meshGenerator.GetMesh();

            SetNewMesh(targetRect, newMesh);
            AdaptCollider(targetRect, meshGenerator);

            targetRect.Uvs = meshGenerator.GetUVs();
        }

        private void AssignSizeValues(GeneratedRect targetRect, out float x, out float z, out float height)
        {
            if (xProperty.hasMultipleDifferentValues)
            {
                x = targetRect.X;
            }
            else
            {
                x = xProperty.floatValue;
            }

            if (zProperty.hasMultipleDifferentValues)
            {
                z = targetRect.Z;
            }
            else
            {
                z = zProperty.floatValue;
            }

            if (heightProperty.hasMultipleDifferentValues)
            {
                height = targetRect.ColliderHeight;
            }
            else
            {
                height = heightProperty.floatValue;
            }
        }

        private void AdaptCollider(GeneratedRect targetRect, RectMeshGenerator meshGenerator)
        {
            BoxCollider col = targetRect.GetComponent<BoxCollider>();
            if (col != null)
            {
                Undo.RecordObject(col, "Collider bounds change");
                meshGenerator.AssignRectVariables(col);
                meshGenerator.AdjustCollider(col, heightProperty.floatValue);
            }
        }

        private void SetNewMesh(GeneratedRect targetRect, Mesh newMesh)
        {
            MeshFilter meshFilter = targetRect.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Undo.RecordObject(meshFilter, "MeshFilter mesh change");
                meshFilter.mesh = newMesh;
            }
        }

        private void GenerateUVMap(GeneratedRect targetRect)
        {
            UVBitmapGenerator generator = new UVBitmapGenerator();
            output = generator.StoreUVMap(targetRect.Uvs, targetRect.UvSize);
            AssetDatabase.Refresh();
        }
    }
}
