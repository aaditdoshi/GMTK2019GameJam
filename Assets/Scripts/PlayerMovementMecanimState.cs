using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[System.Serializable]
public class AnimationSet
{
    public AnimationReferenceAsset UpAnimation;
    public AnimationReferenceAsset DownAnimation;
    public AnimationReferenceAsset Animation;
}


public class PlayerMovementMecanimState : PlayerIdleMecanimState
{
 
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (trackEntry.Animation != GetAnimmationAsset(animator).Animation)
        {
            trackEntry = state.SetAnimation(layerIndex, GetAnimmationAsset(animator).Animation, bIsLoop);
        }
    }
}
