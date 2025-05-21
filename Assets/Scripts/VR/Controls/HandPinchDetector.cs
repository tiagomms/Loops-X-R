using UnityEngine;
using Oculus.Interaction.Input;

namespace PullableXR
{
    /// <summary>
    /// Detects and manages pinch gestures from the Oculus SDK.
    /// Provides a single set of callbacks for pinch start and end events.
    /// </summary>
    public class HandPinchDetector : MonoBehaviour
    {
        #region Fields
        [Header("Pinch Detection")]
        [SerializeField, Tooltip("Collider used for pinch detection")]
        private Collider pinchCollider;
        
        [SerializeField, Tooltip("Transform representing the end point of the pinch")]
        private Transform pinchEndTransform;

        [Header("Configuration")]
        [SerializeField, Tooltip("Threshold for pinch detection (0-1)")]
        private const float pinchThreshold = 0.8f;

        private Hand hand;
        private bool wasPinching;
        private System.Action onPinch;
        private System.Action onRelease;

        private const HandFinger pinchFinger = HandFinger.Index;
        private const HandFinger thumbFinger = HandFinger.Thumb;
        #endregion

        #region Properties
        /// <summary>
        /// Gets whether the hand was pinching in the last frame.
        /// </summary>
        public bool WasPinching => wasPinching;

        /// <summary>
        /// Gets the collider used for pinch detection.
        /// </summary>
        public Collider PinchCollider => pinchCollider;

        /// <summary>
        /// Gets the transform representing the end point of the pinch.
        /// </summary>
        public Transform PinchEndTransform => pinchEndTransform;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            hand = GetComponent<Hand>();
            if (hand == null)
            {
                Debug.LogError($"[{nameof(HandPinchDetector)}] No Hand component found on {gameObject.name}");
            }
        }

        private void Update()
        {
            if (hand == null || !hand.IsConnected) return;

            bool isPinching = hand.GetFingerIsPinching(pinchFinger);
            if (isPinching && !wasPinching)
            {
                try
                {
                    onPinch?.Invoke();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[{nameof(HandPinchDetector)}] Error invoking pinch callback: {e.Message}");
                }
            }
            else if (!isPinching && wasPinching)
            {
                try
                {
                    onRelease?.Invoke();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[{nameof(HandPinchDetector)}] Error invoking release callback: {e.Message}");
                }
            }

            wasPinching = isPinching;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets callbacks for pinch and release events.
        /// </summary>
        /// <param name="onPinch">Callback to invoke when pinch starts</param>
        /// <param name="onRelease">Callback to invoke when pinch ends</param>
        public void SetCallbacks(System.Action onPinch, System.Action onRelease)
        {
            this.onPinch = onPinch;
            this.onRelease = onRelease;
        }

        /// <summary>
        /// Clears all pinch-related callbacks.
        /// </summary>
        public void ClearCallbacks()
        {
            onPinch = null;
            onRelease = null;
        }
        #endregion
    }
}
