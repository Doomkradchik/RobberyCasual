using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MainCharacterController : MonoBehaviour
{
    [SerializeField]
    private float _unitsPerSecond = 1f;

    [Tooltip("Inputs")]
    private FixedJoystick _joystick;

    private Animator _characterAnimator;


    private void Awake()
    {
        _joystick = FindObjectOfType<FixedJoystick>();
        _characterAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move(_joystick.Direction);
    }

    private void Move(Vector2 input)
    {
        var direction = new Vector3(input.x, 0, input.y);
        if (direction.magnitude < 0.1f)
        {
            _characterAnimator.SetBool("Moving", false);
            return;
        }

        direction.Normalize();
        _characterAnimator.SetBool("Moving", true);
        transform.position += direction * _unitsPerSecond * Time.fixedDeltaTime;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }
}
