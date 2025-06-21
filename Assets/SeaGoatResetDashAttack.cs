using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatResetDashAttack : StateMachineBehaviour
{
    // Variables
    [SerializeField] private float totalAnimLength = 150;

    [SerializeField] private Vector3 targetResetPos = new(-5f, -2f, -1f);
    private Vector3 trueTargetResetPos = new(-5f, -2f, -1f);
    private float movementDelta;

    private int direction = 0;  // 1 for right, -1 for left

    private GameObject seaGoatBoss;
    [SerializeField] private float bossScale = 0.35f;

    private bool complete = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the game object to manipulate visuals
        if (seaGoatBoss == null)
        {
            seaGoatBoss = GameObject.Find("SeaGoat");
        }
        seaGoatBoss.GetComponent<PolygonCollider2D>().enabled = false;

        direction = animator.GetInteger("direction");
        complete = false;
        trueTargetResetPos.x = targetResetPos.x * -direction;

        movementDelta = (trueTargetResetPos.x - animator.transform.position.x) / totalAnimLength;

        animator.transform.position = new Vector3(Mathf.Round(animator.transform.position.x), -2, -1);
        animator.transform.localScale = new Vector3(-direction * bossScale, bossScale, bossScale);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (complete)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            //animator.transform.Translate(movementDelta, 0, 0);
            animator.transform.position = new Vector3(animator.transform.position.x + movementDelta, targetResetPos.y, targetResetPos.z);

            if (Mathf.Abs(animator.transform.position.x - trueTargetResetPos.x) < 0.1f)
            {
                animator.transform.position = trueTargetResetPos;
                complete = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("ResetDash");
    }
}
