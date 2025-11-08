using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NevermoreStudios.Core;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace NevermoreStudios.GameState
{
    
#if UNITY_EDITOR
    public class PersistentDebugDraw
    {
        private Vector3 _centerPosition;
        private Quaternion _rotation;
        private Vector3 _scale;
        private readonly System.Action<Vector3, Quaternion, Vector3> _debugDrawCallback;
        private float _lifeTime;

        // Constructor receives a callback
        public PersistentDebugDraw(System.Action<Vector3, Quaternion, Vector3> debugDrawCallback, Vector3 centerPosition, Quaternion rotation, Vector3 scale, float lifeTime)
        {
            _debugDrawCallback = debugDrawCallback;
            this._lifeTime = lifeTime;
            _centerPosition = centerPosition;
            _rotation = rotation;
            _scale = scale;
        }

        public void Update(float deltaTime)
        {
            _lifeTime -= deltaTime;
        }

        public bool HasExpired
        {
            get
            {
                return _lifeTime <= Mathf.Epsilon;
            }
        }

        public void Draw()
        {
            _debugDrawCallback(_centerPosition, _rotation, _scale);
        }
    }
#endif
    
    public class GameInstance<T> : Singleton<T> where T : MonoBehaviour
    {
        [SerializeField] private Mesh debugCubeMesh;
        [SerializeField] private Mesh debugSphereMesh;
        [SerializeField] private Material wireFrameMaterial;
#if UNITY_EDITOR
        private readonly List<PersistentDebugDraw> persistentDebugDraws = new();
#endif       
        
        protected override void OnAwake()
        {
            InitializeGameInstanceAssets();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            InitializeGameInstanceAssets();
        }
#endif

        private void InitializeGameInstanceAssets()
        {
            if (debugCubeMesh == null)
            {
                debugCubeMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
            }

            if (debugSphereMesh == null)
            {
                debugSphereMesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
            }

            if (wireFrameMaterial == null)
            {
                wireFrameMaterial = Resources.Load<Material>("M_WireFrame");
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            for(int persistentDebugIndex = 0; persistentDebugIndex < persistentDebugDraws.Count; ++persistentDebugIndex)
            {
                persistentDebugDraws.ElementAt(persistentDebugIndex).Draw();
                persistentDebugDraws.ElementAt(persistentDebugIndex).Update(Time.deltaTime);
            }
            persistentDebugDraws.RemoveAll(debugDraw => debugDraw.HasExpired);
#endif
        }
        
#if UNITY_EDITOR
        public void DebugDrawBox(Vector3 boxCenter, Quaternion boxRotation, Vector3 boxSize, float duration)
        {
            persistentDebugDraws.Add(new PersistentDebugDraw(DebugDrawBox, boxCenter, boxRotation, boxSize, duration));
        }
        public void DebugDrawSphere(Vector3 sphereCenter, Quaternion sphereRotation, Vector3 sphereSize, float duration)
        {
            persistentDebugDraws.Add(new PersistentDebugDraw(DebugDrawSphere, sphereCenter, sphereRotation, sphereSize, duration));
        }
        
        public void DebugDrawBox(Vector3 boxCenter, Quaternion boxRotation, Vector3 boxSize)
        {
            Graphics.DrawMesh(
                mesh: debugCubeMesh,
                matrix: Matrix4x4.TRS(boxCenter, boxRotation, boxSize),
                material: wireFrameMaterial,
                layer: 0
            );
        }
        public void DebugDrawSphere(Vector3 sphereCenter, Quaternion sphereRotation, Vector3 sphereSize)
        {
            Graphics.DrawMesh(
                mesh: debugSphereMesh,
                matrix: Matrix4x4.TRS(sphereCenter, sphereRotation, sphereSize),
                material: wireFrameMaterial,
                layer: 0
            );
        }
        
        public void DrawSphere(Vector3 center, float radius, Color color, int segments = 16)
        {
            float step = 360f / segments;

            for (int segmentIndex = 0; segmentIndex < segments; segmentIndex++)
            {
                float angle0 = Mathf.Deg2Rad * step * segmentIndex;
                float angle1 = Mathf.Deg2Rad * step * (segmentIndex + 1);

                // XY plane
                Debug.DrawLine(center + new Vector3(Mathf.Cos(angle0), Mathf.Sin(angle0), 0) * radius,
                    center + new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0) * radius,
                    color);

                // XZ plane
                Debug.DrawLine(center + new Vector3(Mathf.Cos(angle0), 0, Mathf.Sin(angle0)) * radius,
                    center + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius,
                    color);

                // YZ plane
                Debug.DrawLine(center + new Vector3(0, Mathf.Cos(angle0), Mathf.Sin(angle0)) * radius,
                    center + new Vector3(0, Mathf.Cos(angle1), Mathf.Sin(angle1)) * radius,
                    color);
            }
        }
#endif
    }
}
