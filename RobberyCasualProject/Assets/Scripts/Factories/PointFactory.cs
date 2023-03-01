using Random = UnityEngine.Random;
using UnityEngine;
using System.Linq;


public interface IPointProvider
{
    Vector3 getNewPoint { get; }
}

[CreateAssetMenu]
public class PointFactory : ScriptableObject, IPointProvider
{
    public Vector3[] _points;
    private int[] _indexes;
    private int _ind;

    public void CreateAndShuffle()
    {
        _indexes = new int[_points.Length];
        for (int i = 0; i < _indexes.Length; i++)
        {
            _indexes[i] = i;
        }

        _indexes = _indexes.OrderBy(_ => Random.value).ToArray();
        _ind = _indexes.Length - 1;
    }

    public Vector3 getNewPoint
    {
        get
        {
            if(_ind < 0)
            {
                ApplyShuffling();
                _ind = _indexes.Length - 1;
            }
            return _points[_indexes[_ind--]];
        }
    }

    private void ApplyShuffling()
    {
        for (int i = 0; i < _indexes.Length; i++)
        {
            _indexes[i] = (_indexes[i] + 1) % _indexes.Length;
        }
        Reverse();
    }

    private void Reverse()
    {
        int start = 0;
        int end = _indexes.Length - 1;

        while (start < end)
        {
            int temp = _indexes[start];
            _indexes[start] = _indexes[end];
            _indexes[end] = temp;

            start++;
            end--;
        }
    }
}
