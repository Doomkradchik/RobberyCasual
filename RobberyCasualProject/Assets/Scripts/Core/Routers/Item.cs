using UnityEngine;

public class Item : Interactor<MainCharacterController>
{
    [SerializeField]
    private ItemDefinition _config;

    protected override void OnTriggered()
    {
        if (_entity.CanPickUp)
        {
            _entity.PickUp(_config);
            Destroy(gameObject);
        }
    }
}


