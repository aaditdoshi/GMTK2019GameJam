using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine;
using Spine.Unity;

public class DataMecanimState : SpineMecanimState
{



    #region Inspector
    public AnimationReferenceAsset animation;

	#endregion


    public override AnimationReferenceAsset GetAnimmationAsset(Animator animator)
    {
        return animation;
    }
}
