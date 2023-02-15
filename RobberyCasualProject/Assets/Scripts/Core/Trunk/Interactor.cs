using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public class Interactor<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Params")]
    [SerializeField, Min(0.2f)]
    private float _duration = 0.25f;

    [SerializeField]
    private RadialProgressBarUI _progressBar;

    private bool _inSpot;

    public UnityEvent<T> Triggered;
    protected T _entity;

    protected virtual bool CanInteract => true;

    protected virtual void Awake()
    {
        _entity = FindObjectOfType<T>();
        if (_entity == null)
            throw new System.InvalidOperationException();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<T>() == _entity && CanInteract)
        {
            _inSpot = true;
            StartCoroutine(PrepareForInteraction());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _inSpot = false;
    }

    private IEnumerator PrepareForInteraction()
    {
        var progress = 0f;
        var expiredTime = 0f;
        while(progress < 1f && _inSpot)
        {
            expiredTime += Time.deltaTime;
            progress = expiredTime / _duration;
            _progressBar.Progress = progress;
            yield return null;
        }

        if (progress >= 1f)
        {
            Triggered?.Invoke(_entity);
            OnTriggered();
        }

        _progressBar.Progress = 0f;
    }

    protected virtual void OnTriggered() { }
}
