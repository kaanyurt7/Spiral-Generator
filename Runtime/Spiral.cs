using System;
using System.Collections.Generic;
using UnityEngine;

namespace IWI.Tools.Shapes
{
    [Serializable]
    public class Spiral
    {

#region Private Variables
        private List<Vector3> _nodes = new();       
#endregion

#region Public SerializeFields
        [SerializeField] private float totalLength;
        [SerializeField] private Vector3 centralPosition;
        [SerializeField] private float initialRadius;
        [SerializeField] private float initialAngle;
        [SerializeField] private int resolution;
        [SerializeField] private float curvature;
#endregion

#region Public Fields

        public float TotalLength
        {
            private set => totalLength = value;
            get => totalLength;
        }

        public float ArcLength
        {
            get => resolution == 0 ? 0f : totalLength / resolution;
        }

        public Vector3 CentralPosition
        {
            set => centralPosition = value;
            get => centralPosition;
        }

        public float InitialAngle
        {
            get => initialAngle;
            private set => initialAngle = value;
        }
        
        public int Resolution
        {
            get => resolution;
            private set => resolution = value;
        }
        
        public float Curvature
        {
            get => curvature;
            private set => curvature = value;
        }

        public float InitialRadius
        {
            get => initialRadius;
            private set => initialRadius = value;
        }
        
#endregion

#region Constructor
        public Spiral (Vector3 centralPosition, float initialRadius, float initialAngle, float length, int resolution, float curvature)
        {
            this.centralPosition = centralPosition;
            this.initialRadius = initialRadius;
            this.initialAngle = initialAngle;
            totalLength = length;
            this.resolution = resolution;
            this.curvature = curvature;
            Rebuild();
        }

        public Spiral()
        {
            centralPosition = Vector3.zero;
            initialAngle = 0f;
            initialRadius = 3f;
            totalLength = 100f;
            resolution = 90;
            curvature = 40f;
            Rebuild();
        }

#endregion

#region Private Functions

private Vector3 GetPointPosition(float radius, float angleInDegrees)
{
    float angle = angleInDegrees * Mathf.Deg2Rad;
    float x = Mathf.Cos(angle) * radius;
    float y = Mathf.Sin(angle) * radius;
    return new Vector3(x, y, 0f);
}

#endregion

#region Public Functions

        public Vector3 GetLocalPointPosition(int index)
        {
            if (index < 0 || index >= _nodes.Count)
            {
                Debug.LogError("Invalid index");
                return Vector3.zero;
            }

            return _nodes[index];
        }

        public Vector3 GetWorldPointPosition(int index)
        {
            if (index < 0 || index >= _nodes.Count)
            {
                Debug.LogError("Invalid index");
                return Vector3.zero;
            }
            
            return GetLocalPointPosition(index) + CentralPosition;
        }
        
        public void Rebuild()
        {
            _nodes.Clear();

            if (resolution < 2 || totalLength <= 0f) return;
            float arcLength = totalLength / (resolution - 1);
            float angle = initialAngle;

            for (int i = 0; i < resolution; i++)
            {
                float radius = initialRadius + curvature * (angle * Mathf.Deg2Rad); 
                Vector3 point = GetPointPosition(radius, angle);
                _nodes.Add(point);
                float deltaAngleRad = arcLength / radius;
                float deltaAngleDeg = deltaAngleRad * Mathf.Rad2Deg;
                angle += deltaAngleDeg;
            }
        }
        
        public void OnDrawGizmos(Color color, float pointSize)
        {
            if (_nodes == null || _nodes.Count == 0)
            {
                return;
            }

            Gizmos.color = color;

            for (int i = 0; i < _nodes.Count; i++)
            {
                Vector3 nodeWorld = GetWorldPointPosition(i);
                Gizmos.DrawSphere(nodeWorld, pointSize);
            }
        }
        
#endregion

    }
}