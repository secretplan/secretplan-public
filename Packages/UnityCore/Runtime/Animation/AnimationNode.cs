using UnityEngine;
using UnityEngine.Animations;

namespace SecretPlan.Core.Animation
{
    public class AnimationNode : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            animator.GetOrAddComponent<AnimationNodeListener>()
                .UpdateState(StateTransition.Enter, stateInfo.shortNameHash, layerIndex, stateInfo.loop);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            animator.GetOrAddComponent<AnimationNodeListener>()
                .UpdateState(StateTransition.Exit, stateInfo.shortNameHash, layerIndex, stateInfo.loop);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                animator.GetOrAddComponent<AnimationNodeListener>()
                    .UpdateState(StateTransition.Finish, stateInfo.shortNameHash, layerIndex, stateInfo.loop);
            }
        }
    }
}