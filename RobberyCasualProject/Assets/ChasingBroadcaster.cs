using UnityEngine;

public class ChasingBroadcaster : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var controller = animator.GetComponent<PatrolingController>();
        controller.FollowPlayer();
    }
}
