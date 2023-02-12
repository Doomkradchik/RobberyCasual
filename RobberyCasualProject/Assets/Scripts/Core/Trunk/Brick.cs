using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Brick : DragDropHandler
{
    [SerializeField]
    internal BrickParamsConfig _config;

    private readonly Vector3 _verticalOffset = Vector3.up * 0.3f;
    [SerializeField, Header("Diagnostics")]
    internal bool _inGrid;

    private Cell _cell;

    public Cell Cell
    {
        get => _cell;

        set
        {
            _cell = value;
            _inGrid = value != null;
        }
    }

    private int _layer;

    private int _ignoreRayLayer => LayerMask.NameToLayer("Ignore Raycast");

    private void Awake()
    {
        _layer = gameObject.layer;
    }

    private void OnMouseDrag()
    {
        if (_inGrid)
        {
            TrunkGrid.Instance.ExtrudeFromGrid(this);
            _inGrid = false;
        }

        gameObject.layer = _ignoreRayLayer;

        var cast = TrunkGrid.Instance.getPlane.Raycast(getRay, out float enter);
        ShiftBrick(enter, cast);

        Cursor.visible = false;
    }

    private void ShiftBrick(float distance, bool succesfully = true)
    {
        if (succesfully == false)
            throw new System.InvalidOperationException();

        transform.position = getRay.GetPoint(distance) + _verticalOffset;
    }

    private void OnMouseUp()
    {
        TrunkGrid.Instance.getPlane.Raycast(getRay, out float enter);

        if (Physics.Raycast(getRay, out RaycastHit hit) && hit.collider.TryGetComponent(out Cell cell))
        {
            if (TrunkGrid.Instance.TryPlaceBrick(cell, this) == false)
                Return();
        }
        else
        {
            Return();
        }
        Cursor.visible = true;
        gameObject.layer = _layer;
    }

    private void Return()
    {
        if(TrunkGrid.Instance.TryPlaceBrick(Cell, this) == false)
           transform.localPosition = Vector3.zero;
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
