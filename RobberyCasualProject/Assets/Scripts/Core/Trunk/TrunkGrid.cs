using UnityEngine;
using System.Collections.Generic;

public class TrunkGrid : MonoBehaviour
{
    [SerializeField]
    private GridInfo _info;

    [SerializeField]
    internal Camera _renderer;

    [System.Serializable]
    public struct GridInfo
    {
        public Cell[] contents;
        public Vector2Int size;
    }

    public static TrunkGrid Instance;
    public Plane getPlane => new Plane(Vector3.up, transform.position);
    private Dictionary<Vector2Int, Cell> _cellByPositionProvider;

    private void Awake()
    {
        _cellByPositionProvider = new Dictionary<Vector2Int, Cell>();

        for (int i = 0, v = 0; i < _info.size.x; i++)
        {
            for (int j = 0; j < _info.size.y; j++, v++)
            {
                var position = new Vector2Int(j, i);
                _cellByPositionProvider.Add(position, _info.contents[v]);
                _info.contents[v].Init(position);
            }
        }
        Instance = this;
    }

    public bool TryPlaceBrick(Cell cell, Brick brick)
    {
        if (_cellByPositionProvider.ContainsValue(cell) == false) { return false; } 

        if (cell._captured) { return false; }

        var centerPosition = cell._position;
        var pendingCells = new List<Cell>() { cell };

        foreach (Vector2Int offset in brick._config)
        {
            var hasValue = _cellByPositionProvider.TryGetValue(centerPosition + offset, out Cell next);
            if (hasValue && next._captured == false)
                pendingCells.Add(next);
            else return false; 
        }

        foreach (var c in pendingCells)
            c._captured = true;

        brick.Cell = cell;
        brick.transform.position = cell.transform.position;
        return true;
    } 

    public void ExtrudeFromGrid(Brick brick)
    {
        if (brick._inGrid == false) { return; }

        var cell = brick.Cell;
        cell._captured = false;

        var centerPosition = cell._position;

        foreach (Vector2Int segmentPos in brick._config)
            _cellByPositionProvider[centerPosition + segmentPos]._captured = false;
    }
}

