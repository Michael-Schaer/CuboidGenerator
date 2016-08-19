using System.Collections.Generic;
using UnityEngine;

namespace GeneratedCuboids
{
    public class GeneratedRect : MonoBehaviour
    {
        private Vector3 colliderCenter = Vector3.zero;
        [SerializeField]
        private List<Vector2> uvs; // needs to be serialized - UVMap generation needs the information also after scene reload
        [SerializeField]
        private float x, z;
        [SerializeField]
        private float colliderHeight;
        [SerializeField]
        private int uvSize = 512;
        [SerializeField]
        private string output = string.Empty;

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

        public float ColliderHeight
        {
            get
            {
                return colliderHeight;
            }

            set
            {
                colliderHeight = value;
            }
        }

        public void AddMaterial(GameObject newObj)
        {
            MeshRenderer renderer = newObj.AddComponent<MeshRenderer>();
            Material[] materials = new Material[1];
            materials[0] = new Material(Shader.Find("Standard"));
            renderer.sharedMaterials = materials;
        }

        public void MoveToColliderCenter(Vector3 newCenter)
        {
            transform.position = new Vector3(newCenter.x, transform.position.y, newCenter.z);
        }
    }
}

