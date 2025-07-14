using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatIdle : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int idleAnimLength = 110;
    [SerializeField] private int jumpWindUpAnimLength = 181 - 110;

    [SerializeField] private int rotation = 75;
    private Quaternion targetRotation = new();
    private Vector3 targetJumpPosition = new(0, 9.5f, -1);
    private Vector3 distanceChange;

    private int direction;

    private GameObject seaGoatBoss;

    private bool doJumpWindUp = false;
    private bool doJump = false;
    private bool complete = false;

    public StageType nextAttackType = StageType.None;
    [SerializeField] private int dashCount = 3;
    private int totalDashes = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        direction = animator.GetInteger("direction");

        // Find the game object to manipulate visuals
        if (seaGoatBoss == null)
        {
            seaGoatBoss = GameObject.Find("SeaGoat");
        }
        SeaGoatManager.instance.SetCanBeHurt(false);    // The boss can't take damage but can still hurt the player
        SeaGoatManager.instance.SetCanDamage(true);
        seaGoatBoss.transform.localScale = new Vector3(SeaGoatManager.instance.regularBossScale * (direction == 0 ? 1 : -direction), SeaGoatManager.instance.regularBossScale, 1);

        currentTime = 0;
        doJump = false;
        doJumpWindUp = false;
        complete = false;

        animator.ResetTrigger("DashWarn");
        animator.ResetTrigger("HornMissileWarn");

        if ((nextAttackType == StageType.Dash || nextAttackType == StageType.None) && totalDashes >= dashCount)
        {
            nextAttackType = StageType.HornMissile;
            totalDashes = 0;
        }
        else
        {
            totalDashes++;
            nextAttackType = StageType.Dash;
        }

        distanceChange = (targetJumpPosition - animator.transform.position) / jumpWindUpAnimLength;
        targetRotation = Quaternion.Euler(0, 0, (direction >= 0 ? -rotation : rotation));

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
                animator.transform.Rotate(0, 0, (direction > 0 ? -1 : 1)); // Rotate slightly
            }
        }
        else if (doJump && !complete)
        {
            if (animator.transform.position.y >= targetJumpPosition.y)
            {
                complete = true;
            }
            else
            {
                animator.transform.Translate(distanceChange, Space.World);  // Do jump
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
                animator.SetTrigger("HornMissileWarn");
            }

            complete = false;

            // Reset the scale for other animations to control
            seaGoatBoss.transform.localScale = new Vector3(SeaGoatManager.instance.regularBossScale, SeaGoatManager.instance.regularBossScale, 1);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Idle");
    }
}