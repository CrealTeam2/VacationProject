using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnarmedIdleBehaviour : PlayerAnimationBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && player.hasPistol)
        {
            animator.SetTrigger("ExitCurrent");
            animator.SetInteger("SwitchingTo", 1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && player.hasKnife)
        {
            animator.SetTrigger("ExitCurrent");
            animator.SetInteger("SwitchingTo", 2);
        }
        else if(Input.GetKeyDown(KeyCode.E) && player.medicines > 0)
        {
            animator.SetTrigger("ExitCurrent");
            animator.SetInteger("SwitchingTo", 0);
            animator.SetBool("UseItem", true);
            animator.SetInteger("UsingItem", 0);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && player.bandages > 0)
        {
            animator.SetTrigger("ExitCurrent");
            animator.SetInteger("SwitchingTo", 0);
            animator.SetBool("UseItem", true);
            animator.SetInteger("UsingItem", 1);
        }
        else if (player.canFocus && Input.GetMouseButton(1))
        {
            animator.SetBool("Stance", true);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

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
