using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCameraMovementBehaviour : PlayerAnimationBehaviour
{
    [SerializeField] Vector3 movePos;
    Vector3 defaultPos;
    private void Awake()
    {
        defaultPos = Camera.main.transform.localPosition;
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    IEnumerator moving = null;
    IEnumerator Returning()
    {
        for(int i = 0; i < 20; i++)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, defaultPos, 0.05f);
            yield return null;
        }
        Camera.main.transform.localPosition = defaultPos;
        moving = null;
    }
    IEnumerator Moving()
    {
        for (int i = 0; i < 20; i++)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, movePos, 0.05f);
            yield return null;
        }
        Camera.main.transform.localPosition = movePos;
        moving = null;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    /*override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("StateEnter");
        if(moving != null)
        {
            player.StopCoroutine(moving);
        }
        moving = Moving();
        player.StartCoroutine(moving);
    }*/
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("StateMachineEnter");
        if (moving != null)
        {
            player.StopCoroutine(moving);
        }
        moving = Moving();
        player.StartCoroutine(moving);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    /*override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("StateExit");
        if (moving != null)
        {
            player.StopCoroutine(moving);
        }
        moving = Returning();
        player.StartCoroutine(moving);
    }*/
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("StateMachineExit");
        if (moving != null)
        {
            player.StopCoroutine(moving);
        }
        moving = Returning();
        player.StartCoroutine(moving);
    }

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
