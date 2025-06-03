using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatStart : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int pauseAnimLength = 68;
    [SerializeField] private int rotationAnimLength = 136;
    [SerializeField] private int angeredAnimLength = 337;
    [SerializeField] private int totalAnimLength = 417;

    private bool doRotation = false;
    private bool doReverseRotation = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        angeredAnimLength -= rotationAnimLength;
        rotationAnimLength -= pauseAnimLength;

        SeaGoatManager.instance.ChangeStage(StageType.Start);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime++;

        if (currentTime >= angeredAnimLength + rotationAnimLength + pauseAnimLength)
        {
            doReverseRotation = true;
        }
        else if (currentTime >= rotationAnimLength + pauseAnimLength)
        {
            doRotation = false;
        }
        else if (currentTime >= pauseAnimLength)
        {
            doRotation = true;
        }

        // Do rotation for angered state
        if (doRotation)
        {
            animator.transform.Rotate(0, 0, 1f);
        }
        else if (doReverseRotation)
        {
            animator.transform.Rotate(0, 0, -1f);
        }

        if (currentTime >= totalAnimLength)
        {
            animator.SetBool("Idle", true);
            currentTime = 0;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
