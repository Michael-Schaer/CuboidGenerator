using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GeneratedCuboids
{
    public class RectGenerator : EditorWindow
    {
        private float x = DEFAULT_SIZE;
        private float z = DEFAULT_SIZE;
        private float colliderHeight = DEFAULT_SIZE;
        private int uvSize = 512;
        private bool generateUVMap = false;
        private string output = "Cuboid will have the selected GameObject as a parent.";

        public const int DEFAULT_SIZE = 1;
        private const string SUCCESS_MESSAGE = "Cuboid is in your Scene, parent GameObject: ";
        private const string ROOT_NAME = "root";
        private const string DOT_SPACE = ". ";

        [MenuItem("GameObject/3D Object/Rect...")]
        static void InitUnwindedUV()
        {
            // Get existing open window or if none, make a new one:  
            EditorWindow window = EditorWindow.GetWindow(typeof(RectGenerator));
            window.Show();
        }

        void OnGUI()
        {
            DrawInputFields();
        }

        private void DrawInputFields()
        {
            x = EditorGUI.FloatField(new Rect(0, 120, position.width, 15), "X", x);
            z = EditorGUI.FloatField(new Rect(0, 140, position.width, 15), "Z", z);
            colliderHeight = EditorGUI.FloatField(new Rect(0, 160, position.width, 15), "Collider Height", colliderHeight);

            generateUVMap = EditorGUI.Toggle(new Rect(0, 220, position.width, 15), "Generate UV Map", generateUVMap);
            uvSize = EditorGUI.IntField(new Rect(0, 240, position.width, 15), "UV size", uvSize);

            if (GUILayout.Button("Create Rect"))
            {
                RectMeshGenerator meshGenerator = new RectMeshGenerator(x, z, colliderHeight);

                meshGenerator.CreateRect();
                meshGenerator.CreateNewObject();
                meshGenerator.SetParent(Selection.activeTransform);
                List<Vector2> uvs = meshGenerator.GetUVs();

                if (Selection.activeTransform == null)
                {
                    output = SUCCESS_MESSAGE + ROOT_NAME + DOT_SPACE;
                }
                else
                {
                    output = SUCCESS_MESSAGE + Selection.activeTransform.gameObject.name + DOT_SPACE;
                }

                if (generateUVMap)
                {
                    UVBitmapGenerator generator = new UVBitmapGenerator();
                    output += generator.StoreUVMap(uvs, uvSize);
                    AssetDatabase.Refresh();
                }
            }

            GUILayout.Box(output);
        }
    }
}