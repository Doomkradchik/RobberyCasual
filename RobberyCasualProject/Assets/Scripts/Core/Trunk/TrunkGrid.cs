using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TrunkGrid : MonoBehaviour
{
    [SerializeField]
    private Cell _cellPrefab;

    [SerializeField]
    internal Camera _renderer;

    public Vector3 _padding;
    public Vector3 cellSize;

    public static TrunkGrid Instance;

    public Plane getPlane => new Plane(Vector3.up, transform.position);

    private Dictionary<Vector2Int, Cell> _cells;


    public Vector2Int _gridSize;

    private void Awake()
    {
        Instance = this;

        CreateGrid();
    }

    private void CreateGrid()
    {
        _cells = new Dictionary<Vector2Int, Cell>();

        for (int i = 0, x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++, i++)
            {
                var offset = new Vector3(x * cellSize.x, 0f, y * cellSize.z);
                var cellPosition = transform.position + offset;
                var newCell = Instantiate(_cellPrefab, cellPosition, Quaternion.identity);
                newCell.position = new Vector2Int(x, y);

                _cells.Add(newCell.position,  newCell);


                newCell.transform.parent = transform;
                newCell.gameObject.layer = gameObject.layer;
            }
        }
    }

    public bool TryPlaceBrick(Cell cell, Brick brick)
    {
        if (_cells.ContainsValue(cell) == false) { return false; } 

        if (cell.captured) { return false; }

        var centerPosition = cell.position;
        var pendingCells = new List<Cell>() { cell };

        foreach (Vector2Int segmentPos in brick._config)
        {
            if (_cells.TryGetValue(centerPosition + segmentPos, out Cell next) && next.captured == false)
                pendingCells.Add(next);
            else return false; 
        }

        foreach (var c in pendingCells)
            c.captured = true;

        brick.Cell = cell;
        brick.transform.position = cell.transform.position;
        return true;
    } 

    public void ExtrudeFromGrid(Brick brick)
    {
        if (brick._inGrid == false) { return; }

        var cell = brick.Cell;
        cell.captured = false;

        var centerPosition = cell.position;

        foreach (Vector2Int segmentPos in brick._config)
            _cells[centerPosition + segmentPos].captured = false;
    }
}

