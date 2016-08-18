using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GeneratedCuboids
{
    public class CuboidGenerator : EditorWindow
    {
        private float x = DEFAULT_SIZE;
        private float y = DEFAULT_SIZE;
        private float z = DEFAULT_SIZE;
        private int uvSize = 512;
        private bool generateUVMap = false;
        private bool forceVerticalMap = false;
        private string output = "Cuboid will have the selected GameObject as a parent.";

        private const int DEFAULT_SIZE = 10;
        private const string SUCCESS_MESSAGE = "Cuboid is in your Scene, parent GameObject: ";
        private const string ROOT_NAME = "root";
        private const string DOT_SPACE = ". ";

        [MenuItem("Window/CuboidGenerator")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:  
            EditorWindow window = EditorWindow.GetWindow(typeof(CuboidGenerator));
            window.Show();
        }

        void OnGUI()
        {
            DrawInputFields();
        }

        private void DrawInputFields()
        {
            x = EditorGUI.FloatField(new Rect(0, 120, position.width, 15), "X", x);
            y = EditorGUI.FloatField(new Rect(0, 140, position.width, 15), "Y", y);
            z = EditorGUI.FloatField(new Rect(0, 160, position.width, 15), "Z", z);
            forceVerticalMap = EditorGUI.Toggle(new Rect(0, 180, position.width, 15), "Force vertical mapping", forceVerticalMap);

            generateUVMap = EditorGUI.Toggle(new Rect(0, 220, position.width, 15), "Generate UV Map", generateUVMap);
            uvSize = EditorGUI.IntField(new Rect(0, 240, position.width, 15), "UV size", uvSize);

            if (GUILayout.Button("Create Cuboid"))
            {
                AbstractCuboidMeshGenerator meshGenerator;
                if (forceVerticalMap)
                {
                    meshGenerator = new VerticalCuboidMeshGenerator(x, y, z);
                }
                else
                {
                    meshGenerator = AbstractCuboidMeshGenerator.ConstructOptimalGenerator(x, y, z);
                }

                meshGenerator.CreateCuboid();
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