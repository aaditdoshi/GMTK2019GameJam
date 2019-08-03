using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine;
using Spine.Unity;

public class SpineMecanimState : StateMachineBehaviour {
    
    #region Inspector
    [System.Serializable]
	public struct AnimationTransition {
		public AnimationReferenceAsset from;
		public AnimationReferenceAsset transition;
        public float Value;
	}

    public AnimationReferenceAsset DefaultTransition;

    [UnityEngine.Serialization.FormerlySerializedAs("transitions")]
	public List<AnimationTransition> fromTransitions = new List<AnimationTransition>();

    public bool bIsLoop;
	#endregion

	protected Spine.AnimationState state;
    protected TrackEntry trackEntry;

    public virtual void Initialize (Animator animator) {
		if (state == null) {
			var animationStateComponent = (animator.GetComponent(typeof(IAnimationStateComponent))) as IAnimationStateComponent;
			state = animationStateComponent.AnimationState;
		}
	}

    public virtual bool SetTransition(Animator animator, TrackEntry current, float timeScale, int layerIndex)
    {
        foreach (var t in fromTransitions)
        {
            if (t.from.Animation == current.Animation)
            {
                var transitionEntry = state.SetAnimation(layerIndex, t.transition.Animation, false);
                transitionEntry.TimeScale = timeScale;
                return true;
            }
        }
        return false;
    }

    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (state == null) {
			Initialize(animator);
		}
		
		float timeScale = stateInfo.speed;
		TrackEntry current = state.GetCurrent(layerIndex);

		bool transitionPlayed = false;
		if (current != null && fromTransitions.Count > 0) {
            transitionPlayed = SetTransition(animator, current, timeScale, layerIndex);

        }

        if(!transitionPlayed && DefaultTransition)
        {
            var transitionEntry = state.SetAnimation(layerIndex, DefaultTransition, false);
            transitionEntry.TimeScale = timeScale;
            transitionPlayed = true;
        }
	
		if (transitionPlayed) {
			trackEntry = state.AddAnimation(layerIndex, GetAnimmationAsset(animator).Animation, bIsLoop, 0);
		} else {
			trackEntry = state.SetAnimation(layerIndex, GetAnimmationAsset(animator).Animation, bIsLoop);
		}
		trackEntry.TimeScale = timeScale;

	}

    public virtual AnimationReferenceAsset GetAnimmationAsset(Animator animator)
    {
        return null;
    }
}
