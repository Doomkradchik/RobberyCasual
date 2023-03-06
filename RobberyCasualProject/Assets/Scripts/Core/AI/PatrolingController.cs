using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PatrolingController : MonoBehaviour
{
    private AgentProperties _calmState, _hurryState;
    private Animator _animator;
    private IPointProvider _pointProvider;
    private NavMeshAgent _agent;

    private float CRITICAL_DISTANCE = 1f;

    [Serializable]
    public struct AgentProperties
    {
        public float speed;
        public float angularSpeed;
        public float acceleration;
        public float stoppingDistance;
    }

    public bool Stopped
    {
        set
        {
            if (value)
            {
                StopAllCoroutines();
            }

            _agent.isStopped = value;
        }
    }

    public void Init(PointFactory factory, AgentProperties calm, AgentProperties hurry)
    {
        _calmState = calm;
        _hurryState = hurry;
        _pointProvider = factory;
        _agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
        _animator.SetBool("Patroling", true);
    }

    public void ExecutePoint()
    {
        PrepareForExecution(_calmState);
        _animator.SetBool("Patroling", true);
        StartCoroutine(DestinateToRoutine(() => _pointProvider.getNewPoint, false, () => _animator.SetBool("Patroling", false)));
    }

    public void FollowPlayer()
    {
        var player = FindObjectOfType<MainCharacterController>();
        PrepareForExecution(_hurryState);
        StartCoroutine(DestinateToRoutine(() => player.transform.position, true));
    }

    private void PrepareForExecution(AgentProperties state)
    {
        Stopped = false;
        state.Apply(_agent);
        StopAllCoroutines();
    }

    private IEnumerator DestinateToRoutine(Func<Vector3> pointProvider, bool updatePath = false, Action onDestinated = null)
    {
        var path = new NavMeshPath();
        var succesfully = NavMesh.CalculatePath(transform.position, pointProvider.Invoke(), NavMesh.AllAreas, path);

        if (succesfully == false)
            throw new System.InvalidOperationException();

        _agent.SetPath(path);

        do
        {
            yield return null;
            if (updatePath)
                _agent.SetDestination(pointProvider.Invoke());
        }
        while (PathCompleted(pointProvider.Invoke()) == false);
        onDestinated?.Invoke();
    }

    protected bool PathCompleted(Vector3 point)
    {
        return !_agent.pathPending &&
            _agent.remainingDistance <= _agent.stoppingDistance + CRITICAL_DISTANCE;
    }
}

public static class AgentProertiesExtension
{
    public static void Apply(this PatrolingController.AgentProperties properties, NavMeshAgent agent)
    {
        agent.speed = properties.speed;
        agent.angularSpeed = properties.angularSpeed;
        agent.acceleration = properties.acceleration;
        agent.stoppingDistance = properties.stoppingDistance;
    }
}