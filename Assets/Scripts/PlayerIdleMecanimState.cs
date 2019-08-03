using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleMecanimState : SpineMecanimState
{
    PlayerMovementScript PlayerMovement;
    PlayerShootProjectile PlayerShooting;
    public AnimationSet WithWeapon;
    public AnimationSet WithoutWeapon;

    public override void Initialize(Animator animator)
    {
        base.Initialize(animator);
        PlayerMovement = animator.transform.root.GetComponent<PlayerMovementScript>();
        PlayerShooting = animator.transform.root.GetComponent<PlayerShootProjectile>();
    }

    public override AnimationReferenceAsset GetAnimmationAsset(Animator animator)
    {
        AnimationSet currentSet;
        if (PlayerShooting.HasProjectile)
        {
            currentSet = WithWeapon;
        }
        else
        {
            currentSet = WithoutWeapon;
        }

        switch (PlayerMovement.PlayerDirection)
        {
            case PlayerMovementScript.EnumPlayerDirection.Up:
                return currentSet.UpAnimation;
            case PlayerMovementScript.EnumPlayerDirection.Down:
                return currentSet.DownAnimation;
            case PlayerMovementScript.EnumPlayerDirection.Left:
            case PlayerMovementScript.EnumPlayerDirection.Right:
                return currentSet.Animation;
        }
        return null;
    }

}
