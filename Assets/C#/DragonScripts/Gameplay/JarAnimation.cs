using System;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

namespace Dragon.Gameplay
{

    class JarAnimation : MonoBehaviour
    {
        [SpineAnimation]
        public string anim2_6;

        [SpineAnimation]
        public string anim2_5;

        SkeletonAnimation skeletonAnimation;

        public Spine.AnimationState spineAnimationState;
        public Spine.Skeleton skeleton;
        void Start()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.Skeleton;

        }


        public void PlayAnimation(int index = 0)
        {
            spineAnimationState.SetAnimation(0, anim2_6, false);
        }
    }
}