using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatDashWarn : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int pauseAnimLength = 50;
    [SerializeField] private int totalAnimLength = 200;

    [SerializeField] private Vector3 leftPos;
    [SerializeField] private Vector3 rightPos;
    [SerializeField] private float leftX = -15;
    [SerializeField] private float rightX = 15;
    [SerializeField] private float topY = 2.5f;
    [SerializeField] private float bottomY = -2.5f;

    private float movementDelta;
    private int enterDirection = 1; // 1 for right, -1 for left
    private int section = 1; // 1 for top, -1 for bottom

    private GameObject seaGoatBoss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;

        // Calculate any movement stuff and set the initial position
        enterDirection = Random.Range(0, 2) % 2 == 0 ? 1 : -1;  // To choose the direction
        section = Random.Range(0, 2) % 2 == 0 ? 1 : -1;         // To choose the section

        leftPos = new(leftX, (section == 1 ? topY : bottomY), -1);
        rightPos = new(rightX, (section == 1 ? topY : bottomY), -1);

        movementDelta = (leftPos.x - rightPos.x) * enterDirection / totalAnimLength;

        animator.transform.SetPositionAndRotation(enterDirection == 1 ? rightPos : leftPos, Quaternion.Euler(0, 0, 0));

        // Find the game object to manipulate visuals
        if (seaGoatBoss == null)
        {
            seaGoatBoss = GameObject.Find("SeaGoat");
        }

        seaGoatBoss.GetComponent<SpriteRenderer>().sortingOrder = 0;
        seaGoatBoss.GetComponent<PolygonCollider2D>().enabled = false;
        seaGoatBoss.transform.localScale = new Vector3(-enterDirection * seaGoatBoss.transform.localScale.x, seaGoatBoss.transform.localScale.y, seaGoatBoss.transform.localScale.z);

        SeaGoatManager.instance.ChangeStage(StageType.Dash);
        animator.SetInteger("direction", enterDirection);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime++;

        if (Mathf.Abs(animator.transform.position.x - (enterDirection == -1 ? rightPos.x : leftPos.x)) < 0.1f)
        {
            animator.SetTrigger("DashAttack");
        }
        else if (currentTime >= pauseAnimLength)
        {
            animator.transform.position = new Vector3(animator.transform.position.x, leftPos.y, -1);
            animator.transform.Translate(movementDelta, 0, 0, Space.World);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("DashWarn");
    }
}
