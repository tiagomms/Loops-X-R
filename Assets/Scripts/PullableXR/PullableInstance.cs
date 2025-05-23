using UnityEngine;
using DG.Tweening;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections.Generic;

namespace PullableXR
{
    /// <summary>
    /// Represents the actual spawned object during a pull gesture.
    /// Handles grabbing, releasing, scale animation, and interaction disabling logic.
    /// Implements the Observer pattern for behavior notifications.
    /// </summary>
    public class PullableInstance : MonoBehaviour
    {
        private PullableSpawner spawner;
        private Transform instanceTransform;
        private Transform handTransform;
        private HandGrabInteractor interactor;

        private float confirmDistance;
        private float minScale;
        private float maxScale;
        private float failedDuration;
        private Ease failedEase;

        private string originalLayerName;
        private int[] originalLayers;
        private string temporaryLayerName;
        private bool hasBeenConfirmed = false;

        private InteractableUnityEventWrapper _eventWrapper;
        private HandGrabInteractable _interactable;
        private readonly List<PullableBehavior> _behaviors = new();

        private void Start()
        {
            _interactable = GetComponentInChildren<HandGrabInteractable>();

            // Get all behaviors from this GameObject and its children
            _behaviors.AddRange(GetComponentsInChildren<PullableBehavior>());

            AttachToHand();
        }

        /// <summary>
        /// Initializes this instance with all required references and parameters.
        /// </summary>
        public void Initialize(
            PullableSpawner spawner,
            Transform instanceT,
            Vector3 initialPos,
            Transform handT,
            HandGrabInteractor interactor,
            float confirmDistance,
            float minScale,
            float maxScale,
            float failedDuration,
            Ease failedEase,
            string temporaryLayerName
        )
        {
            this.spawner = spawner;
            this.instanceTransform = instanceT;
            this.handTransform = handT;
            this.interactor = interactor;
            this.confirmDistance = confirmDistance;
            this.minScale = minScale;
            this.maxScale = maxScale;
            this.failedDuration = failedDuration;
            this.failedEase = failedEase;
            this.temporaryLayerName = temporaryLayerName;

            //StoreAndReplaceLayers();
        }

        private void Update()
        {
            if (hasBeenConfirmed)
            {
                // NOTE: On confirmation, Ensure the scale is set to max and disable
                instanceTransform.localScale = Vector3.one * maxScale;
                // Notify all behaviors (Observer pattern)
                foreach (var behavior in _behaviors)
                {
                    try
                    {
                        behavior.OnPullConfirmed(this);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[{nameof(PullableInstance)}] Error in pullable behavior {behavior.GetType().Name} during OnPullConfirmed: {e.Message}\nStack Trace: {e.StackTrace}");
                        throw; // Re-throw to maintain error context
                    }
                }

                enabled = false;
                return;
            }

            instanceTransform.position = handTransform.position;
            float distance = Vector3.Distance(instanceTransform.position, spawner.transform.position);

            float t = Mathf.Clamp01(distance / confirmDistance);
            float scaleValue = Mathf.Lerp(minScale, maxScale, t);
            instanceTransform.localScale = Vector3.one * scaleValue;
        }

        /// <summary>
        /// Called when the user stops pinching.
        /// Determines whether the object is confirmed or cancelled.
        /// </summary>
        public bool Release()
        {
            float dist = Vector3.Distance(instanceTransform.position, spawner.transform.position);
            XRDebugLogViewer.Log($"Pullable: Release - {dist >= confirmDistance}");

            if (dist >= confirmDistance)
            {
                Confirm();
            }
            else
            {
                Cancel();
            }
            //ResetEventWrapper();
            return dist >= confirmDistance;
        }

        /// <summary>
        /// Finalizes the pull, re-enabling interaction and calling confirm logic.
        /// Notifies all registered behaviors of the confirmation.
        /// </summary>
        private void Confirm()
        {
            XRDebugLogViewer.Log($"Pullable: Confirm - HasBeenConfirmed {hasBeenConfirmed}");

            if (hasBeenConfirmed) return;
            hasBeenConfirmed = true;

            // NOTE: due to properties of HandGrabInteractable, I need to perform the confirm operations on the next frame in update
            /*
            // Ensure the scale is set to max
            instanceTransform.localScale = Vector3.one * maxScale;

            if (_rb)
            {
                _rb.isKinematic = false;
            }
            */
            //RestoreOriginalLayers();

            spawner.HandleConfirm();

            XRDebugLogViewer.Log($"Pullable: Confirm - Scale set to {instanceTransform.localScale}");
        }

        /// <summary>
        /// Cancels the pull with animation and destroys the instance.
        /// Notifies all registered behaviors of the cancellation.
        /// </summary>
        private void Cancel()
        {
            // Notify all behaviors before cancellation (Observer pattern)
            foreach (var behavior in _behaviors)
            {
                try
                {
                    behavior.OnPullCancelled(this);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[{nameof(PullableInstance)}] Error in pullable behavior {behavior.GetType().Name} during OnPullCancelled: {e.Message}\nStack Trace: {e.StackTrace}");
                    throw; // Re-throw to maintain error context
                }
            }

            Sequence cancelSeq = DOTween.Sequence();
            cancelSeq.Join(instanceTransform.DOMove(spawner.transform.position, failedDuration).SetEase(failedEase));
            cancelSeq.Join(instanceTransform.DOScale(Vector3.one * minScale, failedDuration).SetEase(failedEase));
            cancelSeq.OnComplete(() => Destroy(gameObject));

            XRDebugLogViewer.Log($"Pullable: Cancel");

            spawner.HandleCancel();
        }

        /// <summary>
        /// Attempts to attach this instance to the interactor hand.
        /// </summary>
        private void AttachToHand()
        {
            if (!_interactable)
            {
                XRDebugLogViewer.LogWarning($"Pullable: Attach to Hand - Can't attach to hand - no hand interactable in object");
                return;
            }

            // true to allow normal release
            interactor.ForceSelect(_interactable, true);
            XRDebugLogViewer.Log($"Pullable: Attach to Hand - {interactor.gameObject.name} selects {_interactable.gameObject.name}");
        }

        /// <summary>
        /// Stores all original layers and replaces them with a temporary uninteractive one.
        /// </summary>
        private void StoreAndReplaceLayers()
        {
            int tempLayer = LayerMask.NameToLayer(temporaryLayerName);
            if (tempLayer < 0) tempLayer = 0;

            var all = GetComponentsInChildren<Transform>(true);
            originalLayers = new int[all.Length];
            for (int i = 0; i < all.Length; i++)
            {
                originalLayers[i] = all[i].gameObject.layer;
                all[i].gameObject.layer = tempLayer;
            }
        }

        /// <summary>
        /// Restores the original layers on all child objects.
        /// </summary>
        private void RestoreOriginalLayers()
        {
            var all = GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < all.Length; i++)
            {
                all[i].gameObject.layer = originalLayers[i];
            }
        }

        private void ResetEventWrapper()
        {
            if (!_eventWrapper) return;
            _eventWrapper.WhenSelect.RemoveListener(() => { });
            _eventWrapper.WhenUnselect.RemoveListener(() => { });
        }
    }
}
