using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatHornMissiles : StateMachineBehaviour
{
    // Variables
    private int currentTime = 0;
    [SerializeField] private int hornMissileDetachAnimLength = 60;
    [SerializeField] private int totalAnimLength = 160;

    private bool detached = false;

    [SerializeField] private Vector3 hiddenPos = new(0, -8, -1);
    [SerializeField] private Vector3 visiblePos = new(0, -3.25f, -1);
    [SerializeField] private Vector3 resetPos = new(15, 2.5f, -1);
    private float movementDelta;

    private Vector3 leftHornMissilePos = new(-0.62f, -3.75f, -1);
    private Vector3 rightHornMissilePos = new(0.29f, -3.75f, -1);

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;
        detached = false;

        movementDelta = (hiddenPos.y - visiblePos.y) / (totalAnimLength - hornMissileDetachAnimLength);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime++;

        //! Change it so that if checks if the horns are destroyed
        if (!detached)
        {
            // Instantiate the horn game objects into the scene
            GameObject.Instantiate(SeaGoatManager.instance.leftHornMissilePrefab, leftHornMissilePos, Quaternion.Euler(0, 0, 180));
            GameObject.Instantiate(SeaGoatManager.instance.rightHornMissilePrefab, rightHornMissilePos, Quaternion.Euler(0, 0, 180));

            detached = true;
        }
        else if (currentTime > hornMissileDetachAnimLength && currentTime < totalAnimLength)
        {
            animator.transform.Translate(0, movementDelta, 0, Space.World);
        }
        else if (currentTime > totalAnimLength && GameObject.Find("LeftHornMissile(Clone)") == null && GameObject.Find("RightHornMissile(Clone)") == null)
        {
            animator.transform.SetPositionAndRotation(animator.GetInteger("direction") == 1 ? resetPos : new Vector3(-resetPos.x, resetPos.y, 1), Quaternion.Euler(0, 0, 0));
            animator.SetTrigger("Reset");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("HornMissile");
    }
}
