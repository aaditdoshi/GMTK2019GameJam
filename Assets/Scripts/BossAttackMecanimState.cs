using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackMecanimState : DataMecanimState {

    public float[] AttackTimes;
    int AttackIndex = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        AttackIndex = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (AttackTimes.Length > AttackIndex && stateInfo.normalizedTime > AttackTimes[AttackIndex] )
        {
            AttackIndex++;
            BossBehaviour bossBehaviour = animator.transform.root.GetComponent<BossBehaviour>();
            if (bossBehaviour)
            {
                bossBehaviour.Attack();
            }
        }
    }
}
