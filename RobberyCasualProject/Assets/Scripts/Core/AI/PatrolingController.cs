using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PatrolingController : MonoBehaviour
{
    private Animator _animator;
    private IPointProvider _pointProvider;
    private NavMeshAgent _agent;

    private float CRITICAL_DISTANCE = 1f;

    public void Init(PointFactory factory)
    {
        _pointProvider = factory;
        _agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
        _animator.SetBool("Patroling", true);
    }

    private bool IsReached =>
        _agent.hasPath && _agent.remainingDistance <= _agent.stoppingDistance + CRITICAL_DISTANCE;

    public void ExecutePoint()
    {
        StartCoroutine(DestinateToRoutine(_pointProvider.getNewPoint, () => _animator.SetBool("Patroling", false)));
    }

    private IEnumerator DestinateToRoutine(Vector3 point, Action onDestinated)
    {
        do
        {
            _agent.SetDestination(point);
            yield return null;
        }
        while (IsReached == false);
        onDestinated.Invoke();
    }
}
