using UnityEngine;

public class DoorBroadcaster : MonoBehaviour
{
    [SerializeField, Header("Diagnostics: Do not modify")]
    private bool _forward;
    [SerializeField]
    private Vector3 _direction;

    [SerializeField]
    private Door[] _doors;

    private Vector3 _forwardPattern;

    private void Awake()
    {
        _forwardPattern = transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        _direction =  transform.position - other.transform.position;
        _direction = _direction.ToPlaneXZ().normalized;
        var _forward = CalculateProjectionOfVector(_direction);

        foreach (var door in _doors)
            door.Open(_forward);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        foreach (var door in _doors)
            door.Close();
    }

    private bool CalculateProjectionOfVector(Vector3 vec)
       => Vector3.Dot(_forwardPattern, vec) >= 0;
}
