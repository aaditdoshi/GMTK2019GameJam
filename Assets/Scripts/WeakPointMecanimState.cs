using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointMecanimState : StateMachineBehaviour
{
    public float StartTime;
    public float EndTime;
    bool ActivateEventSend;
    bool DeactivateEventSend;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        ActivateEventSend = false;
        DeactivateEventSend = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (stateInfo.normalizedTime > StartTime && !ActivateEventSend)
        {
            ActivateEventSend = true;
            BossBehaviour bossBehaviour = animator.transform.root.GetComponent<BossBehaviour>();
            if (bossBehaviour)
            {
                bossBehaviour.WeakPointState(true);
            }
        }

        if (stateInfo.normalizedTime > EndTime && !DeactivateEventSend)
        {
            DeactivateEventSend = true;
            BossBehaviour bossBehaviour = animator.transform.root.GetComponent<BossBehaviour>();
            if (bossBehaviour)
            {
                bossBehaviour.WeakPointState(false);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!DeactivateEventSend)
        {
            DeactivateEventSend = true;
            BossBehaviour bossBehaviour = animator.transform.root.GetComponent<BossBehaviour>();
            if (bossBehaviour)
            {
                bossBehaviour.WeakPointState(false);
            }
        }
    }
}
