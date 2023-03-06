using UnityEngine;

public class Cell : MonoBehaviour
{
    public void Init(Vector2Int position, bool captured = false)
    {
        _position = position;
        _captured = captured;
    }

    internal Vector2Int _position;
    internal bool _captured;
}