using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGrabBehaviour : PlayerAnimationBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    Zombie origin;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(origin == null) origin = animator.gameObject.GetComponent<Zombie>();
        Vector3 pos = animator.transform.position;
        Vector3 pos2 = player.transform.position;
        Vector2 pos3 = Vector2.ClampMagnitude(new Vector2(pos2.x, pos2.z) - new Vector2(pos.x, pos.z), Vector2.Distance(new Vector2(pos2.x, pos2.z), new Vector2(pos.x, pos.z))-0.4f);
        origin.navMeshAgent.Warp(animator.transform.position + new Vector3(pos3.x, 0, pos3.y));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
