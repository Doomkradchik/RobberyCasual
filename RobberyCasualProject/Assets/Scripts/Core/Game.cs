using UnityEngine;

public class Game : MonoBehaviour
{
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
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TrunkGrid.Instance.CreateGrid(CurrentTrunk.Size);
    }
}


