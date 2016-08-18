using System.Collections.Generic;
using UnityEngine;

namespace GeneratedCuboids
{
    public class GeneratedCuboid : MonoBehaviour
    {
        private Vector3 colliderCenter = Vector3.zero;
        [SerializeField]
        private List<Vector2> uvs; // needs to be serialized - UVMap generation needs the information also after scene reload
        [SerializeField]
        private float x, y, z;
        [SerializeField]
        private int uvSize = 512;
        [SerializeField]
        private string output = string.Empty;
        [SerializeField]
        private bool forceVerticalMap = false;

        public Vector3 ColliderCenter
        {
            get
            {
                return colliderCenter;
            }

            set
            {
                colliderCenter = value;
            }
        }

        public List<Vector2> Uvs
        {
            get
            {
                return uvs;
            }

            set
            {
                uvs = value;
            }
        }

        public float X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public float Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }

        public string Output
        {
            get
            {
                return output;
            }

            set
            {
                output = value;
            }
        }

        public bool ForceVerticalMap
        {
            get
            {
                return forceVerticalMap;
            }

            set
            {
                forceVerticalMap = value;
            }
        }

        public int UvSize
        {
            get
            {
                return uvSize;
            }

            set
            {
                uvSize = value;
            }
        }

        public void AddMaterial(GameObject newObj)
        {
            MeshRenderer renderer = newObj.AddComponent<MeshRenderer>();
            Material[] materials = new Material[1];
            materials[0] = new Material(Shader.Find("Standard"));
            renderer.sharedMaterials = materials;
        }

        public void SetMaterial(Material material)
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            Material[] materials = new Material[1];
            materials[0] = material;
            renderer.sharedMaterials = materials;
        }

        public Material GetMaterial()
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            return renderer.sharedMaterials[0];
        }

        public void MoveToColliderCenter(Vector3 newCenter)
        {
            transform.position = newCenter;
        }
    }
}

