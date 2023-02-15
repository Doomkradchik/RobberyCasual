using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FOV : MonoBehaviour
{
    [SerializeField]
    private Properties _properties;

    private Vector3[] _vertices;
    private int[] _triangles;
    private float _startingAngle;
    private Mesh _viewField;


    private void GenerateMesh()
    {
        var ti = 0;
        var vi = 1;
        var distance = _properties._viewDistance;
        var offset = _properties._angleOfView * 0.5f;

        for (int i = 0; i < _properties._rayCount + 1; i++)
        {
            var angle = _startingAngle + offset;
            var worldRay = new Ray(transform.position, transform.forward.RotateVector(offset));

            var length = Physics.Raycast(worldRay, out RaycastHit hit, distance)
                ? hit.distance
                : distance;

            var localPoint = Vector3.one.GetDirectionOffsetFromAngle(angle) * length;

            // convert to local position
            _vertices[vi] = localPoint;
            offset -= _properties.DeltaAngle;
            if (i > 0)
            {
                _triangles[ti] = 0;
                _triangles[ti + 1] = vi - 1;
                _triangles[ti + 2] = vi;
                ti += 3;
            }
           
            vi++;
        }
    }

    private void ApplyModifications()
    {
        _viewField.vertices = _vertices;
        _viewField.triangles = _triangles;

        _viewField.RecalculateNormals();
    }

    private Vector3 GetPoint(RaycastHit hit)
    {
        OnCollided(hit);
        return hit.point;
    }

    protected virtual void OnCollided(RaycastHit hit) { }


    [Serializable]
    public struct Properties
    {
        public float _angleOfView;
        public int _rayCount;
        public float _viewDistance;
        public LayerMask _layerMask;
        public float DeltaAngle => _angleOfView / _rayCount;
    }

    private void Start()
    {
        _startingAngle = 0f;

        _viewField = new Mesh();
        GetComponent<MeshFilter>().mesh = _viewField;

        _vertices = new Vector3[_properties._rayCount + 2];
        _triangles = new int[_properties._rayCount * 3];
        _startingAngle = _properties._angleOfView / 2;

        _startingAngle = transform.forward.GetAngleByDirection();
    }

    private void Update()
    {
        GenerateMesh();
        ApplyModifications();
    }
}


public static class Vector3Extension
{
    public static Vector3 RotateVector(this Vector3 direction, float angleOffset)
    {
        var angle = GetAngleByDirection(direction);
        angle += angleOffset;
        return GetDirectionOffsetFromAngle(direction, angle);
    }

    public static float GetAngleByDirection(this Vector3 dir)
    {
        dir = dir.normalized;
        var angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        if (angle < 0f)
            angle += 360f;

        return angle;
    }

    public static Vector3 ConvertToTopDown(this Vector3 _, Vector2 xy_plane)
    {
        return new Vector3(xy_plane.x, 0f, xy_plane.y);
    }

    public static Vector3 GetDirectionOffsetFromAngle(this Vector3 _, float angle)
    {
        var rad = angle * (MathF.PI / 180f);
        var direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        return ConvertToTopDown(direction,direction);
    }

}
