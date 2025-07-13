using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatHornMissileWarn : StateMachineBehaviour
{
    // Variables
    [SerializeField] private int totalAnimLength = 200;

    private bool complete = false;

    [SerializeField] private Vector3 hiddenPos = new(0, -8, -1);
    [SerializeField] private Vector3 visiblePos = new(0, -3.25f, -1);
    private float movementDelta;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        complete = false;

        movementDelta = (visiblePos.y - hiddenPos.y) / totalAnimLength;

        animator.transform.SetPositionAndRotation(hiddenPos, Quaternion.Euler(0, 0, 0));
        animator.transform.localScale = new Vector3(SeaGoatManager.instance.hornMissileBossScale, SeaGoatManager.instance.hornMissileBossScale, 1);

        SeaGoatManager.instance.SetCanBeHurt(false);
        SeaGoatManager.instance.SetCanDamage(true);

        SeaGoatManager.instance.ChangeStage(StageType.HornMissile);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(animator.transform.position.y - visiblePos.y) < 0.1f && !complete)
        {
            complete = true;
            animator.SetTrigger("HornMissile");
        }
        else
        {
            animator.transform.position = new Vector3(hiddenPos.x, animator.transform.position.y, -1);
            animator.transform.Translate(0, movementDelta, 0, Space.World);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("HornMissileWarn");
    }
}
