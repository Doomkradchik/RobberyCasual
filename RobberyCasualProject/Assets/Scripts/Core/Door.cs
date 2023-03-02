using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private DoorData _data;

    [SerializeField, Header("Transition animation duration")]
    private float _duration;

    [System.Serializable]
    public struct DoorData
    {
        public float _forwardYRotation;
        public float _backwardYRotation;
        public float _idleYRotation;
    }

    private bool _opened;
    private Transform transform_m;

    private void Awake()
    {
        transform_m = transform;
    }

    public void Open(bool forward)
    {
        if (_opened) { return; }

        StopAllCoroutines();
        StartCoroutine(TranslateRotationTo(forward ? _data._forwardYRotation : _data._backwardYRotation));

        _opened = true;
    }
    
    public void Close()
    {
        if (_opened == false) { return; }

        StopAllCoroutines();
        StartCoroutine(TranslateRotationTo(_data._idleYRotation));

        _opened = false;
    }

    protected IEnumerator TranslateRotationTo(float to)
    {
        var expiredSeconds = 0f;
        var progress = 0f;
        var startYAngle = transform_m.eulerAngles.y;

        while (progress < 1f)
        {
            expiredSeconds += Time.fixedDeltaTime;
            progress = (float)(expiredSeconds / _duration);

            var newYAngle = Mathf.LerpAngle(startYAngle, to, progress);

            transform_m.rotation = Quaternion.Euler(
                transform_m.eulerAngles.x,
                newYAngle,
                transform_m.eulerAngles.z);

            yield return null;
        }
    }

}


public static class Vector3Extesion
{
    public static Vector3 ToPlaneXZ(this Vector3 vec)
    {
        return new Vector3(vec.x, 0f, vec.z);
    }
}