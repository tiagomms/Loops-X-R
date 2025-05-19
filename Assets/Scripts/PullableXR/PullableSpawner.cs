using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;

namespace PullableXR
{
    /// <summary>
    /// Responsible for spawning pullable instances when a pinch is detected near the spawner.
    /// The spawner itself is static and does not move. Tracks and manages active pullable instance.
    /// </summary>
    public class PullableSpawner : MonoBehaviour
    {
        [Header("Prefab Settings")]
        [SerializeField] private GameObject pullablePrefab;
        [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.1f, 0);

        [Header("Distance Settings")]
        [SerializeField] private float confirmDistance = 0.3f;
        [SerializeField] private float failedReleaseDuration = 0.3f;
        [SerializeField] private Ease failedReleaseEase = Ease.OutSine;

        [Header("Scale Settings")]
        [SerializeField] private float minScale = 0.2f;
        [SerializeField] private float maxScale = 1f;

        [Header("Layer Settings")]
        [SerializeField] private string temporaryLayerName = "Uninteractive";

        [Header("Events")]
        public UnityEvent onConfirm;
        public UnityEvent onCancel;

        private PullableInstance activeInstance;

        /// <summary>
        /// Called when a pinch gesture is detected inside this spawner's trigger collider.
        /// Instantiates and initializes a new pullable instance.
        /// </summary>
        public void TriggerPull(Transform handTransform, HandGrabInteractor interactor)
        {
            if (activeInstance != null) return;

            GameObject spawned = Instantiate(pullablePrefab);
            Transform spawnedT = spawned.transform;

            Vector3 spawnPos = transform.position + spawnOffset;
            spawnedT.position = spawnPos;
            spawnedT.LookAt(Camera.main.transform);
            spawnedT.localScale = Vector3.one * minScale;

            // Attach the PullableInstance logic and initialize behavior.
            activeInstance = spawned.AddComponent<PullableInstance>();
            activeInstance.Initialize(
                spawner: this,
                instanceT: spawnedT,
                initialPos: spawnPos,
                handT: handTransform,
                interactor: interactor,
                confirmDistance: confirmDistance,
                minScale: minScale,
                maxScale: maxScale,
                failedDuration: failedReleaseDuration,
                failedEase: failedReleaseEase,
                temporaryLayerName: temporaryLayerName
            );
        }

        /// <summary>
        /// Called externally by the active PullableInstance when the user releases their pinch.
        /// </summary>
        public void Release()
        {
            if (activeInstance != null)
            {
                activeInstance.Release();
                activeInstance = null;
            }
        }

        /// <summary>
        /// Called by the PullableInstance to confirm the pull succeeded.
        /// </summary>
        public void HandleConfirm()
        {
            onConfirm?.Invoke();
        }

        /// <summary>
        /// Called by the PullableInstance to notify pull was cancelled.
        /// </summary>
        public void HandleCancel()
        {
            onCancel?.Invoke();
        }

        /// <summary>
        /// When a collider enters the trigger, try to detect the Hand and register pinch callbacks.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            Hand hand = other.GetComponentInParent<Hand>();
            if (hand != null)
            {
                HandPinchDetector pinchDetector = hand.GetComponent<HandPinchDetector>();
                if (pinchDetector == null)
                {
                    pinchDetector = hand.gameObject.AddComponent<HandPinchDetector>();
                }

                HandGrabInteractor interactor = hand.GetComponentInChildren<HandGrabInteractor>();
                if (interactor != null)
                {
                    pinchDetector.SetCallbacks(
                        onPinch: () => TriggerPull(hand.transform, interactor),
                        onRelease: () => Release()
                    );
                }
            }
        }

        /// <summary>
        /// Unregister pinch callbacks when the hand exits the trigger collider.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            Hand hand = other.GetComponentInParent<Hand>();
            if (hand != null)
            {
                HandPinchDetector pinchDetector = hand.GetComponent<HandPinchDetector>();
                if (pinchDetector != null)
                {
                    pinchDetector.ClearCallbacks();
                }
            }
        }
    }
}
