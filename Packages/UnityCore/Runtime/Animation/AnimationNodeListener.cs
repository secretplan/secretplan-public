using UnityEngine;

namespace SecretPlan.Core.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimationNodeListener : MonoBehaviour
    {
        public delegate void StateEvent(StateTransition transitionType, int stateHash, int layerIndex, bool isLooping);

        public int MostRecentlyEnteredHash { get; private set; }

        public void UpdateState(StateTransition transitionType, int stateHash, int layerIndex, bool isLooping)
        {
            if (transitionType == StateTransition.Enter)
            {
                MostRecentlyEnteredHash = stateHash;
            }

            StateChanged?.Invoke(transitionType, stateHash, layerIndex, isLooping);
        }

        public event StateEvent? StateChanged;
    }
}