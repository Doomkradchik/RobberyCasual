using UnityEngine;

public class QuardCompositeRoot : MonoBehaviour
{
    [SerializeField]
    private PointFactory _pointProvider;

    [SerializeField]
    private PatrolingController.AgentProperties _calmState;

    [SerializeField]
    private PatrolingController.AgentProperties _hurryState;


    //[SerializeField]
    //private DefaultTrunk _defaultTrunk;

    //private Trunk _trunk;

    //public ITrunkSizeProvider CurrentTrunk
    //{
    //    get => _trunk == null ? _defaultTrunk : _trunk;
    //    set => _trunk = (Trunk)value;
    //}

    private void Awake()
    {
        _pointProvider.CreateAndShuffle();

        var quards = FindObjectsOfType<PatrolingController>();
        if(quards.Length > _pointProvider._points.Length)
        {
            var exception = new System.InvalidOperationException("Quards more than points on map!!");
            Debug.LogException(exception);
            throw exception;
        }    
        foreach (var q in quards)
        {
            q.Init(_pointProvider, _calmState, _hurryState);
        }
        //Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
         //TrunkGrid.Instance.CreateGrid(new Vector2Int(3 , 3));
      
    }

    private void OnDrawGizmos()
    {
        foreach(var point in _pointProvider._points)
        {
            Gizmos.DrawWireSphere(point, 1f);
        }
    }
}


