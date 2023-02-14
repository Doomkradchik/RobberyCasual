using UnityEngine;


public interface ITrunk
{
    void Open(ItemDefinition item = null);
    void Close();
}

public interface ITrunkSizeProvider
{
    Vector2Int Size { get; }
}

public abstract class Trunk : Interactor<MainCharacterController>, ITrunk, ITrunkSizeProvider
{
    [SerializeField]
    private Camera _trunkRenderer;

    [SerializeField]
    private Transform _spawnBrickSpot;

    private Brick _brick;

    public abstract Vector2Int Size { get; }

    private void Start()
    {
        Close();
    }

    public void Close()
    {
        _trunkRenderer.gameObject.SetActive(false);

        if (_brick == null) { return; }

        if (_brick._inGrid)
            _entity.OnDrop();
        else
            Clear();
    }

    private void Clear()
    {
        Destroy(_brick.gameObject);
        _brick = null;
    }

    public void Open(ItemDefinition item = null)
    {
        _trunkRenderer.gameObject.SetActive(true);

        if (item != null)
            _brick = Instantiate(item._brickPrefab, _spawnBrickSpot);
    }

    protected override void OnTriggered()
    {
        Open(_entity.CurrentItem);
    }  
}
