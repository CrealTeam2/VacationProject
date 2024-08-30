using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolIdleBehaviour : PlayerAnimationBehaviour
{

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player.pistolCounter >= player.pistolFireRate && Input.GetMouseButtonDown(0) && player.pistolMag > 0)
        {
            player.pistolCounter = 0;
            player.pistolMag--;
            player.FirePistol();
            animator.SetTrigger("Attack");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("ExitCurrent");
            animator.SetInteger("SwitchingTo", 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && player.hasKnife)
        {
            animator.SetTrigger("ExitCurrent");
            animator.SetInteger("SwitchingTo", 2);
        }
        else if(player.pistolMag < player.pistolMagSize && Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("Reloading", true);
        }
        else if(player.canFocus && Input.GetMouseButton(1))
        {
            animator.SetBool("Stance", true);
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
