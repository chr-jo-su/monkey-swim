using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollider : StateMachineBehaviour
{
    // Variables
    public bool isIdle;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player object
        GameObject player = GameObject.Find("Player");

        if (isIdle)
        {
            // Change the player's cupsule collider to be vertical
            player.GetComponent<CapsuleCollider2D>().direction = CapsuleDirection2D.Vertical;
            player.GetComponent<CapsuleCollider2D>().offset = new Vector2(-0.3f, 0.3f);
            player.GetComponent<CapsuleCollider2D>().size = new Vector2(1.25f, 3.25f);
        }
        else
        {
            // Change the player's cupsule collider to be horizontal
            player.GetComponent<CapsuleCollider2D>().direction = CapsuleDirection2D.Horizontal;
            player.GetComponent<CapsuleCollider2D>().offset = new Vector2(-0.2f, -0.15f);
            player.GetComponent<CapsuleCollider2D>().size = new Vector2(3.25f, 1f);
        }
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
