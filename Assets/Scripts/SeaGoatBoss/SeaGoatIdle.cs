using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatIdle : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int idleAnimLength = 110;
    [SerializeField] private int jumpWindUpAnimLength = 181;
    [SerializeField] private int totalAnimLength = 200;
    private Vector3 targetJumpPosition = new Vector3(0, 8, -1);
    private Vector3 distanceChange;

    private bool doJumpWindUp = false;
    private bool doJump = false;

    public StageType nextAttackType = StageType.None;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;

        jumpWindUpAnimLength -= idleAnimLength;

        distanceChange = (targetJumpPosition - animator.transform.position) / jumpWindUpAnimLength;

        SeaGoatManager.instance.ChangeStage(StageType.Idle);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime++;

        if (currentTime >= idleAnimLength + jumpWindUpAnimLength)
        {
            doJumpWindUp = false;
            doJump = true;
        }
        else if (currentTime >= idleAnimLength)
        {
            doJumpWindUp = true;
            doJump = false;
        }

        // Do jump wind up and jump
        if (doJumpWindUp)
        {
            animator.transform.Translate(Vector3.down * 0.01f, Space.World); // Move down slightly
            animator.transform.Rotate(0, 0, 1f); // Rotate slightly
        }
        else if (doJump)
        {
            animator.transform.Translate(distanceChange, Space.World);
        }

        // Transition to next attack type
        if (currentTime >= totalAnimLength)
        {
            animator.SetBool("Idle", false);
            if (nextAttackType == StageType.Dash || nextAttackType == StageType.None)
            {
                nextAttackType = StageType.HornMissile;
                animator.SetBool("Dash", true);
            }
            else if (nextAttackType == StageType.HornMissile)
            {
                nextAttackType = StageType.Dash;
                animator.SetBool("HornMissile", true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}