using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatDashWarn : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int totalAnimLength = 1000;

    [SerializeField] private Vector3 leftPos;
    [SerializeField] private Vector3 rightPos;
    private int direction = 1; // 1 for right, -1 for left

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;

        direction = Random.Range(0, 2) == 0 ? 1 : -1; // Randomly choose direction

        SeaGoatManager.instance.ChangeStage(StageType.Dash);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime++;

        if (currentTime >= totalAnimLength)
        {
            animator.SetTrigger("DashAttack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("direction", direction);
        animator.ResetTrigger("DashWarn");
    }
}
