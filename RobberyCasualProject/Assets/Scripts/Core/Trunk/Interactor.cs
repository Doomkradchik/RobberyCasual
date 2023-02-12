using UnityEngine.Events;
using UnityEngine;
using System.Collections;


public interface ISleep
{
    public void Sleep();
}

public class Interactor<T> : MonoBehaviour, ISleep where T : MonoBehaviour
{
    [Header("Params")]
    public bool _sleepAfterInteraction;

    public float _cooldown = 1.2f;

    private bool _canInteract;
    private bool CanInteract
    {
        get
        {
            if (_sleepAfterInteraction == false)
                return true;

            return _canInteract;
        }
        set
        {
            _canInteract = value;
        }
    }

    public UnityEvent<T> Triggered;
    protected T _entity;
    protected virtual void Awake()
    {
        _entity = FindObjectOfType<T>();
        if (_entity == null)
            throw new System.InvalidOperationException();

        CanInteract = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<T>() == _entity && CanInteract)
        {
            Triggered?.Invoke(_entity);
            OnTriggered();

            CanInteract = false;
        }
    }

    protected virtual void OnTriggered() { }

    private IEnumerator RaiseCooldownRoutine()
    {
        yield return new WaitForSeconds(_cooldown);
        CanInteract = true;
    }

    public void Sleep() => StartCoroutine(RaiseCooldownRoutine());
}
