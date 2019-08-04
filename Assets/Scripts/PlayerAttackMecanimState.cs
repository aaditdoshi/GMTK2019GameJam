using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class PlayerAttackMecanimState : SpineMecanimState
{
    public float AttackTime;
    public AnimationSet AnimationSet;
    bool bEventSent = false;
    PlayerMovementScript PlayerMovement;


    public override void Initialize(Animator animator)
    {
        base.Initialize(animator);
        PlayerMovement = animator.transform.root.GetComponent<PlayerMovementScript>();
    }
    public override AnimationReferenceAsset GetAnimmationAsset(Animator animator)
    {
        switch (PlayerMovement.PlayerDirection)
        {
            case PlayerMovementScript.EnumPlayerDirection.Up:
                return AnimationSet.UpAnimation;
            case PlayerMovementScript.EnumPlayerDirection.Down:
                return AnimationSet.DownAnimation;
            case PlayerMovementScript.EnumPlayerDirection.Left:
            case PlayerMovementScript.EnumPlayerDirection.Right:
                return AnimationSet.Animation;
        }
        return null;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        bEventSent = false;
        PlayerMovement.SetInAttack(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        PlayerMovement.SetInAttack(false);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (stateInfo.normalizedTime > AttackTime && !bEventSent)
        {
            bEventSent= true;
            PlayerShootProjectile playershoot = animator.transform.root.GetComponent<PlayerShootProjectile>();
            if (playershoot)
            {
                playershoot.Attack();
            }
        }
    }
}
