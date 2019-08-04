using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMecanimState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BossBehaviour bossBehaviour = animator.transform.root.GetComponent<BossBehaviour>();
        if(bossBehaviour)
        {
            bossBehaviour.SetAllowMovement(true);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BossBehaviour bossBehaviour = animator.transform.root.GetComponent<BossBehaviour>();
        if (bossBehaviour)
        {

            bossBehaviour.SetAllowMovement(false);
        }
    }
}
