using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;

namespace PullableXR
{
    /// <summary>
    /// Manages the spawning and tracking of pullable instances in response to pinch gestures.
    /// Supports multiple simultaneous pulls from different pinch detectors.
    /// Implements the Template Method pattern for behavior management.
    /// </summary>
    public class PullableSpawner : MonoBehaviour
    {
        #region Fields
        [Header("Prefab Settings")]
        [SerializeField, Tooltip("Prefab to instantiate when pulling")]
        private GameObject pullablePrefab;
        
        [SerializeField, Tooltip("Offset from spawner position for new instances")]
        private Vector3 spawnOffset = new Vector3(0, 0.1f, 0);

        [Header("Distance Settings")]
        [SerializeField, Tooltip("Distance required to confirm a successful pull")]
        private float confirmDistance = 0.3f;
        
        [SerializeField, Tooltip("Duration of the failed release animation")]
        private float failedReleaseDuration = 0.3f;
        
        [SerializeField, Tooltip("Easing function for failed release animation")]
        private Ease failedReleaseEase = Ease.OutSine;

        [Header("Scale Settings")]
        [SerializeField, Tooltip("Minimum scale for pullable instances")]
        private float minScale = 0.2f;
        
        [SerializeField, Tooltip("Maximum scale for pullable instances")]
        private float maxScale = 1f;

        [Header("Layer Settings")]
        [SerializeField, Tooltip("Layer name for temporary uninteractive state")]
        private string temporaryLayerName = "Uninteractive";

        [Header("Pull Settings")]
        [SerializeField, Tooltip("Maximum number of simultaneous pulls allowed")]
        private int maxSimultaneousPulls = 2;
        
        [SerializeField, Tooltip("Enable detailed logging of pull events")]
        private bool logPullEvents = true;

        [Header("Events")]
        [Tooltip("Invoked when a pull is successfully confirmed")]
        public UnityEvent onConfirm;
        
        [Tooltip("Invoked when a pull is cancelled")]
        public UnityEvent onCancel;

        private readonly Dictionary<HandPinchDetector, PullableInstance> _activeInstances = new Dictionary<HandPinchDetector, PullableInstance>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Called when a pinch gesture is detected inside this spawner's trigger collider.
        /// Instantiates and initializes a new pullable instance for the specific pinch detector.
        /// </summary>
        public void TriggerPull(Transform handTransform, HandGrabInteractor interactor, HandPinchDetector pullingPinch)
        {
            if (_activeInstances.Count >= maxSimultaneousPulls)
            {
                if (logPullEvents)
                {
                    XRDebugLogViewer.Log($"[{nameof(PullableSpawner)}] Maximum simultaneous pulls ({maxSimultaneousPulls}) reached. Ignoring pull from {pullingPinch.gameObject.name}");
                }
                return;
            }

            if (_activeInstances.ContainsKey(pullingPinch)) return;

            try
            {
                GameObject spawned = Instantiate(pullablePrefab);
                Transform spawnedT = spawned.transform;


                //Vector3 spawnPos = transform.position + spawnOffset;
                //spawnedT.position = spawnPos;
                spawnedT.LookAt(Camera.main.transform);
                //spawnedT.localScale = Vector3.one * minScale;

                var instance = spawned.AddComponent<PullableInstance>();
                instance.Initialize(
                    spawner: this,
                    instanceT: spawnedT,
                    initialPos: transform.position,
                    handT: handTransform,
                    interactor: interactor,
                    confirmDistance: confirmDistance,
                    minScale: minScale,
                    maxScale: maxScale,
                    failedDuration: failedReleaseDuration,
                    failedEase: failedReleaseEase,
                    temporaryLayerName: temporaryLayerName
                );

                _activeInstances[pullingPinch] = instance;
                if (logPullEvents)
                {
                    XRDebugLogViewer.Log($"[{nameof(PullableSpawner)}] Trigger Pull instance from {pullingPinch.gameObject.name}. Active pulls: {_activeInstances.Count}/{maxSimultaneousPulls}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[{nameof(PullableSpawner)}] Error creating pullable instance: {e.Message}\nStack Trace: {e.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Called externally by the active PullableInstance when the user releases their pinch.
        /// </summary>
        public void Release(HandPinchDetector pullingPinch)
        {
            if (!_activeInstances.TryGetValue(pullingPinch, out var instance))
            {
                if (logPullEvents)
                {
                    XRDebugLogViewer.Log($"[{nameof(PullableSpawner)}] No active instance found for {pullingPinch.gameObject.name}");
                }
                return;
            }

            // Store instance reference before any operations that might destroy it
            var instanceRef = instance;
            
            // Clear callbacks first to prevent any further interactions
            ClearPinchDetectorCallbacks(pullingPinch);
            
            // Remove from active instances before calling Release
            _activeInstances.Remove(pullingPinch);

            try
            {
                // Now call Release which might destroy the instance
                instanceRef.Release();

                if (logPullEvents)
                {
                    XRDebugLogViewer.Log($"[{nameof(PullableSpawner)}] Release from {pullingPinch.gameObject.name}. Remaining pulls: {_activeInstances.Count}/{maxSimultaneousPulls}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(PullableSpawner)}] Error in Release for {pullingPinch.gameObject.name}: {e.Message}\nStack Trace: {e.StackTrace}");
                throw; // Re-throw to maintain error context
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
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks if the collider is a valid pinch collider that should be processed.
        /// </summary>
        private bool ShouldProcessTrigger(Collider other, out HandPinchDetector pinchDetector)
        {
            pinchDetector = other.GetComponentInParent<HandPinchDetector>();
            if (pinchDetector == null ||
                _activeInstances.ContainsKey(pinchDetector) ||
                pinchDetector.WasPinching)
            {
                return false;
            }

            return pinchDetector.PinchCollider == other;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!ShouldProcessTrigger(other, out var pinchDetector)) return;

            HandGrabInteractor interactor = pinchDetector.GetComponentInChildren<HandGrabInteractor>();
            if (interactor != null)
            {
                pinchDetector.SetCallbacks(
                    onPinch: () => TriggerPull(pinchDetector.PinchEndTransform, interactor, pinchDetector),
                    onRelease: () => Release(pinchDetector)
                );

                if (logPullEvents)
                {
                    XRDebugLogViewer.Log($"[{nameof(PullableSpawner)}] On Trigger Enter - {interactor.gameObject.name} set pinch callbacks");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!ShouldProcessTrigger(other, out var pinchDetector)) return;

            ClearPinchDetectorCallbacks(pinchDetector);
            
            if (logPullEvents)
            {
                XRDebugLogViewer.Log($"[{nameof(PullableSpawner)}] On Trigger Exit - {pinchDetector.gameObject.name} cleared callbacks");
            }
        }

        private void ClearPinchDetectorCallbacks(HandPinchDetector pinchDetector)
        {
            if (pinchDetector != null)
            {
                pinchDetector.ClearCallbacks();
            }
        }
        #endregion
    }
}
