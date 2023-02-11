using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField]
    internal BrickParamsConfig _config;

    private readonly Vector3 _verticalOffset = Vector3.up * 0.3f;
    [SerializeField, Header("Diagnostics")]
    internal bool _inGrid;

    private int _layer;

    private void Awake()
    {
        _layer = gameObject.layer;
    }

    private Ray getRay
    {
        get
        {
            var camera = GridManager.Instance._renderer;
            Vector3 mousePosFar, mousePosNear;
            mousePosFar = mousePosNear = Input.mousePosition;
            mousePosFar.z = camera.farClipPlane;
            mousePosNear.z = camera.nearClipPlane;
            var worldFar = camera.ScreenToWorldPoint(mousePosFar);
            var worldNear = camera.ScreenToWorldPoint(mousePosNear);
            return new Ray(worldNear, worldFar - worldNear);
        }
    }

    private void OnMouseDrag()
    {
        if (_inGrid)
        {
            GridManager.Instance.ExtrudeFromGrid(this);
            _inGrid = false;
        }

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        GridManager.Instance.getPlane.Raycast(getRay, out float enter);
        transform.position = getRay.GetPoint(enter) + _verticalOffset;

        Cursor.visible = false;
    }

    private void OnMouseUp()
    {
        GridManager.Instance.getPlane.Raycast(getRay, out float enter);

        if (Physics.Raycast(getRay, out RaycastHit hit))
        {
            if (hit.collider.GetComponentInParent<GridManager>() != null)
            {
                var fit = GridManager.Instance.TryPlaceBrick(hit.collider.gameObject, this);
                if (fit == false) transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
        Cursor.visible = true;
        gameObject.layer = _layer;
    }
}

[System.Serializable]
public struct BrickParamsConfig : IEnumerable<Vector2Int>
{
    public Vector2Int[] segmentsFromCenter;

    public IEnumerator GetEnumerator()
    {
        return segmentsFromCenter.GetEnumerator();
    }

    IEnumerator<Vector2Int> IEnumerable<Vector2Int>.GetEnumerator()
    {
        return (IEnumerator<Vector2Int>)segmentsFromCenter.GetEnumerator();
    }
}
