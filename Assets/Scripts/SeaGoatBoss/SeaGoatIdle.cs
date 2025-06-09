using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatIdle : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int idleAnimLength = 110;
    [SerializeField] private int jumpWindUpAnimLength = 181 - 110;

    private Quaternion targetRotation = new();
    private Vector3 targetJumpPosition = new(0, 9.5f, -1);
    private Vector3 distanceChange;

    private bool doJumpWindUp = false;
    private bool doJump = false;
    private bool complete = false;

    public StageType nextAttackType = StageType.None;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;
        doJump = false;
        doJumpWindUp = false;
        complete = false;

        animator.ResetTrigger("DashWarn");
        animator.ResetTrigger("HornMissile");

        if (nextAttackType == StageType.Dash || nextAttackType == StageType.None)
        {
            nextAttackType = StageType.HornMissile;
        }
        else if (nextAttackType == StageType.HornMissile)
        {
            nextAttackType = StageType.Dash;
        }

        distanceChange = (targetJumpPosition - animator.transform.position) / jumpWindUpAnimLength;
        targetRotation = Quaternion.Euler(0, 0, 75);

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

            if (animator.transform.rotation != targetRotation)
            {
                animator.transform.Rotate(0, 0, 1f); // Rotate slightly
            }
        }
        else if (doJump && !complete)
        {
            if (animator.transform.position.x >= targetJumpPosition.x && animator.transform.position.y >= targetJumpPosition.y)
            {
                complete = true;
            }
            else
            {
                animator.transform.Translate(distanceChange, Space.World);
            }
        }

        // Transition to next attack type
        if (complete)
        {
            if (nextAttackType == StageType.Dash)
            {
                animator.SetTrigger("DashWarn");
            }
            else if (nextAttackType == StageType.HornMissile)
            {
                animator.SetTrigger("HornMissile");
            }

            complete = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Idle");
    }
}