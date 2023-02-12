using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private Vector3 _relativePosition;

    private void Start()
    {
        _relativePosition =  transform.position - _target.position;
    }

    private void LateUpdate()
    {
        transform.position = _target.position + _relativePosition;
    }
}
