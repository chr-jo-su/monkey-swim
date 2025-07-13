using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatStart : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int pauseAnimLength = 68;
    [SerializeField] private int rotationAnimLength = 136 - 68;
    [SerializeField] private int angeredAnimLength = 349 - 136;
    [SerializeField] private int totalAnimLength = 418;
    private Quaternion targetRotation = new();

    private bool doRotation = false;
    private bool doReverseRotation = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetRotation = Quaternion.Euler(0, 0, 60);

        SeaGoatManager.instance.ChangeStage(StageType.Start);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime++;

        if (currentTime >= angeredAnimLength + rotationAnimLength + pauseAnimLength)
        {
            doRotation = false;
            doReverseRotation = true;
        }
        else if (currentTime >= rotationAnimLength + pauseAnimLength)
        {
            doReverseRotation = false;
            doRotation = false;
        }
        else if (currentTime >= pauseAnimLength)
        {
            doReverseRotation = false;
            doRotation = true;
        }

        // Do rotation for angered state
        if (doRotation && animator.transform.rotation != targetRotation)
        {
            animator.transform.Rotate(0, 0, 1f);
        }
        else if (doReverseRotation && animator.transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            animator.transform.Rotate(0, 0, -1f);
        }

        if (currentTime >= totalAnimLength)
        {
            animator.SetTrigger("Idle");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
