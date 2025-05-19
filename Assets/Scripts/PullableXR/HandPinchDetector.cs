using UnityEngine;
using Oculus.Interaction.Input;

namespace PullableXR
{



    /// <summary>
    /// Detects pinch gestures from the Oculus SDK by checking finger pinch values.
    /// Allows external registration of pinch start and end events.
    /// </summary>
    public class HandPinchDetector : MonoBehaviour
    {
        public System.Action OnPinch;
        public System.Action OnRelease;

        private Hand hand;
        private bool wasPinching;
        [SerializeField] private const float pinchThreshold = 0.8f;

        private void Awake()
        {
            hand = GetComponent<Hand>();
        }

        private void Update()
        {
            if (hand == null || !hand.IsConnected) return;

            float pinchStrength = hand.GetFingerPinchStrength(HandFinger.Index);
            bool isPinching = pinchStrength > pinchThreshold;

            if (isPinching && !wasPinching)
            {
                OnPinch?.Invoke();
            }
            else if (!isPinching && wasPinching)
            {
                OnRelease?.Invoke();
            }

            wasPinching = isPinching;
        }

        /// <summary>
        /// Assign callbacks for pinch and release.
        /// </summary>
        public void SetCallbacks(System.Action onPinch, System.Action onRelease)
        {
            OnPinch = onPinch;
            OnRelease = onRelease;
        }

        /// <summary>
        /// Clears all pinch-related callbacks.
        /// </summary>
        public void ClearCallbacks()
        {
            OnPinch = null;
            OnRelease = null;
        }
    }
}
