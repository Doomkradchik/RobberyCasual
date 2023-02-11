using UnityEngine;

public interface IPickUpHandler
{
    void PickUp(ItemConfig itemInfo);
}

public interface IDropHandler
{
    void OnDrop();
}

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemConfig _config;

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<MainCharacterController>();
        if (player == null) { return; }

        player.PickUp(_config);
        Destroy(gameObject);
    }
}


