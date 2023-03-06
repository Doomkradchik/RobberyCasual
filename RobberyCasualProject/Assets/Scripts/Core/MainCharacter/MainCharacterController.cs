using UnityEngine;
using UnityEngine.Events;
public interface IPickUpHandler
{
    void PickUp(ItemDefinition itemInfo);
}

public interface IDropHandler
{
    void OnDrop();
}

[RequireComponent(typeof(Animator))]
public class MainCharacterController : MonoBehaviour, IPickUpHandler, IDropHandler
{
    [SerializeField]
    private bool _joystickYFlipped = true;

    [SerializeField]
    private bool _joystickXFlipped = true;

    [SerializeField]
    private float _unitsPerSecond = 1f;

    [SerializeField]
    private Transform _itemRoot;

    [Tooltip("Inputs")]
    private FixedJoystick _joystick;
    private Animator _characterAnimator;
    public ItemDefinition CurrentItem { get; private set; }
    public bool CanPickUp => CurrentItem == null;

    public UnityEvent Died;

    private void Awake()
    {
        _joystick = FindObjectOfType<FixedJoystick>();
        _characterAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        var input = _joystick.Direction;
        input.x *= _joystickXFlipped ? -1f : 1f;
        input.y *= _joystickYFlipped ? -1f : 1f;
        Move(input);
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

    public void PickUp(ItemDefinition itemInfo)
    {
        var itemInstance = Instantiate(itemInfo._pickUpPrefab, _itemRoot);
        itemInstance.transform.localPosition = Vector3.zero;
        CurrentItem = itemInfo;

        _characterAnimator.SetLayerWeight(1, 0.9f);
    }

    public void OnDrop()
    {
        if (CurrentItem == null) { return; }
        CurrentItem = null;
        _characterAnimator.SetLayerWeight(1, 0f);
        Destroy(_itemRoot.GetChild(0).gameObject);
    }

    public void OnDie() => Died?.Invoke();
}
