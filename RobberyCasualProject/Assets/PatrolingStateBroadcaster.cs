using UnityEngine;

public class PatrolingStateBroadcaster : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<PatrolingController>()
            .ExecutePoint();
    }
}
