using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private PointFactory _pointProvider;
    private static Game Instance;

    [SerializeField]
    private DefaultTrunk _defaultTrunk;

    private Trunk _trunk;

    public ITrunkSizeProvider CurrentTrunk
    {
        get => _trunk == null ? _defaultTrunk : _trunk;
        set => _trunk = (Trunk)value;
    }
    
    private void Awake()
    {
        _pointProvider.CreateAndShuffle();

        var quards = FindObjectsOfType<PatrolingController>();
        if(quards.Length >= _pointProvider._points.Length)
        {
            var exception = new System.InvalidOperationException("Quards more than points on map!!");
            Debug.LogException(exception);
            throw exception;
        }    
        foreach (var q in quards)
        {
            q.Init(_pointProvider);
        }
        //Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // TrunkGrid.Instance.CreateGrid(CurrentTrunk.Size);
      
    }

    private void OnDrawGizmos()
    {
        foreach(var point in _pointProvider._points)
        {
            Gizmos.DrawWireSphere(point, 1f);
        }
    }
}


