using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _cellPrefab;

    [SerializeField]
    internal Camera _renderer;

    public Vector2Int _gridSize;
    public Vector3 _padding;
    public Vector3 cellSize;
    private Cell[] _cells;

    public static GridManager Instance;

    public Plane getPlane => new Plane(Vector3.up, transform.position);

    [System.Serializable]
    public class Cell
    {
        public Brick _owner = null;
        public Vector2Int _position;
        public GameObject _prefab;
        public bool _captured;
    }

    private void Awake()
    {
        Instance = this;

        CreateGrid();
    }

    private void CreateGrid()
    {
        _cells = new Cell[_gridSize.x * _gridSize.y];

        for (int i = 0, x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++, i++)
            {
                var offset = new Vector3(x * cellSize.x, 0f, y * cellSize.z);
                var cellPosition = transform.position + offset;
                _cells[i] = new Cell
                {
                    _position = new Vector2Int(x, y),
                    _prefab = Instantiate(_cellPrefab, cellPosition, Quaternion.identity),                 
                    _captured = false,
                };
                _cells[i]._prefab.transform.parent = transform;
                _cells[i]._prefab.layer = gameObject.layer;
            }
        }
    }

    

    public bool TryPlaceBrick(GameObject cellInstance, Brick brick)
    {
        if (HasInstance(cellInstance) == false)
            return false;

        var cell = _cells.First(cell => cell._prefab == cellInstance);
        if (cell._captured) { return false; }

        var centerPosition = cell._position;
        var cells = new List<Cell>() { cell };

        foreach (Vector2Int segmentPos in brick._config)
        {
            var newPos = centerPosition + segmentPos;

            if (newPos.x > -1 && newPos.x < _gridSize.x && newPos.y > -1 && newPos.y < _gridSize.y)
            {
                var newCell = _cells.First(c => c._position == newPos);
                if (newCell._captured) { return false; }


                cells.Add(newCell);
            }
            else return false; 
        }

        foreach(var c in cells)
            c._captured = true;

        cell._owner = brick;
        brick.transform.position = cellInstance.transform.position;
        brick._inGrid = true;
        return true;
    } 


    private bool HasInstance(GameObject cellInstance)
    {
        var contains = false;
        foreach (var cell in _cells)
        {
            if (cell._prefab.Equals(cellInstance))
            {
                contains = true;
                break;
            } 
        }

        return contains;
    }

    public void ExtrudeFromGrid(Brick brick)
    {
        var cell = _cells.First(c => c._owner == brick);
        if (cell == null) { return; }        

        cell._captured = false;
        cell._owner = null;

        var centerPosition = cell._position;

        foreach (Vector2Int segmentPos in brick._config)
            _cells.First(c => c._position == centerPosition + segmentPos)._captured = false;
    }
}

