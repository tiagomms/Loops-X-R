using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Oculus.Interaction;

namespace PullableXR
{

    /// <summary>
    /// Runtime logic for a single pull instance. Manages scaling, layer handling, and confirmation logic.
    /// </summary>
    public class PullableInstance : MonoBehaviour
    {
        private PullableSpawner spawner;
        private Transform instanceT;
        private Transform handT;
        private Vector3 initialPos;
        private float confirmDistance;
        private float minScale;
        private float maxScale;
        private float failedDuration;
        private Ease failedEase;
        private bool isReleased;
        private int tempLayer;
        private Dictionary<Transform, int> originalLayers = new Dictionary<Transform, int>();

        /// <summary>
        /// Initializes the pull instance with parameters from the spawner.
        /// </summary>
        public void Initialize(PullableSpawner spawner, Transform instanceT, Vector3 initialPos, Transform handT, float confirmDistance, float minScale, float maxScale, float failedDuration, Ease failedEase, string temporaryLayerName)
        {
            this.spawner = spawner;
            this.instanceT = instanceT;
            this.initialPos = initialPos;
            this.handT = handT;
            this.confirmDistance = confirmDistance;
            this.minScale = minScale;
            this.maxScale = maxScale;
            this.failedDuration = failedDuration;
            this.failedEase = failedEase;
            isReleased = false;

            tempLayer = LayerMask.NameToLayer(temporaryLayerName);
            StoreAndSetLayerRecursive(instanceT);

            // Optional: Hook into Meta XR SDK's GrabInteractable events
            InteractableUnityEventWrapper grab = instanceT.GetComponentInChildren<InteractableUnityEventWrapper>();
            if (grab != null)
            {
                grab.WhenSelect.AddListener(OnGrab);
                grab.WhenUnselect.AddListener(OnRelease);
            }
        }

        private void StoreAndSetLayerRecursive(Transform root)
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                originalLayers[child] = child.gameObject.layer;
                child.gameObject.layer = tempLayer;
            }
        }

        private void RestoreOriginalLayers()
        {
            foreach (var kvp in originalLayers)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.gameObject.layer = kvp.Value;
                }
            }
        }

        private void Update()
        {
            if (isReleased) return;

            instanceT.position = handT.position;
            float distance = Vector3.Distance(instanceT.position, spawner.transform.position);

            float t = Mathf.Clamp01(distance / confirmDistance);
            float scaleValue = Mathf.Lerp(minScale, maxScale, t);
            instanceT.localScale = Vector3.one * scaleValue;
        }

        /// <summary>
        /// Called when the object is released. Handles confirm or cancel logic.
        /// </summary>
        public void Release()
        {
            if (isReleased) return;
            isReleased = true;

            float distance = Vector3.Distance(instanceT.position, spawner.transform.position);

            if (distance >= confirmDistance)
            {
                RestoreOriginalLayers();
                instanceT.localScale = Vector3.one * maxScale;
                spawner.HandleConfirm();
            }
            else
            {
                Sequence cancelSeq = DOTween.Sequence();
                cancelSeq.Join(instanceT.DOMove(initialPos, failedDuration).SetEase(failedEase));
                cancelSeq.Join(instanceT.DOScale(Vector3.one * minScale, failedDuration).SetEase(failedEase));
                cancelSeq.OnComplete(() => Destroy(gameObject));

                spawner.HandleCancel();
            }
        }

        /// <summary>
        /// Optional: Hooks for Meta XR's GrabInteractable. If supported, these can automatically drive pull and release.
        /// </summary>
        private void OnGrab() => spawner.TriggerPull(handT);
        private void OnRelease() => spawner.Release();
    }
}