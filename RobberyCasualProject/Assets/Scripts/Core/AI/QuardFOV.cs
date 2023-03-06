using UnityEngine;

public class QuardFOV : FOV
{
    private Animator _animator;
    private PatrolingController _controller;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        _controller = GetComponentInParent<PatrolingController>();
    }
    protected override void OnCollided(Collider collider)
    { 
        if(collider.TryGetComponent(out MainCharacterController controller))
        {
            _animator.SetTrigger("Detected");
            _controller.Stopped = true;
            gameObject.SetActive(false);
        }
    }
}
