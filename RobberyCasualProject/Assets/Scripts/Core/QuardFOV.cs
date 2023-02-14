using UnityEngine;

public class QuardFOV : FOV
{
    protected override void OnCollided(RaycastHit hit)
    {
        if(hit.collider.TryGetComponent(out MainCharacterController controller))
        {
            controller.OnDie();
        }
    }
}
