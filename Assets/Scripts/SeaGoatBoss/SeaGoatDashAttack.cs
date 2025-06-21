using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatDashAttack : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int totalAnimLength = 50;

    [SerializeField] private float leftPos = -16;
    [SerializeField] private float rightPos = 16;

    private float movementDelta;
    private int direction = 0;  // 1 for right, -1 for left

    private GameObject seaGoatBoss;
    [SerializeField] private float bossDashScale = 0.50f;

    [SerializeField] private int cameraShakeSpeed = 10;
    [SerializeField] private int cameraShakeIntensity = 10;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;

        direction = animator.GetInteger("direction");
        movementDelta = (leftPos - rightPos) * -direction / totalAnimLength;

        // Find the game object to manipulate visuals
        if (seaGoatBoss == null)
        {
            seaGoatBoss = GameObject.Find("SeaGoat");
        }

        seaGoatBoss.GetComponent<SpriteRenderer>().sortingOrder = 2;
        seaGoatBoss.GetComponent<PolygonCollider2D>().enabled = true;
        seaGoatBoss.transform.localScale = new Vector3(direction * bossDashScale, bossDashScale, bossDashScale);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime++;

        if (currentTime >= totalAnimLength)
        {
            animator.SetTrigger("ResetDash");
            Camera.main.transform.position = new Vector3(0, 0, -10);
        }
        else
        {
            animator.transform.Translate(movementDelta, 0, 0, Space.World);

            // Camera shake
            if (Random.Range(0, cameraShakeSpeed) == 0)
            {
                Camera.main.transform.position += new Vector3(Random.Range(-cameraShakeIntensity, cameraShakeIntensity), Random.Range(-cameraShakeIntensity, cameraShakeIntensity), 0) * Time.deltaTime;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("DashAttack");
    }
}
