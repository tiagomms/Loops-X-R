using UnityEngine;
using DG.Tweening;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

namespace PullableXR
{
    /// <summary>
    /// Represents the actual spawned object during a pull gesture.
    /// Handles grabbing, releasing, scale animation, and interaction disabling logic.
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

        private Rigidbody _rb;

        private InteractableUnityEventWrapper _eventWrapper;
        private HandGrabInteractable _interactable;


        private void Start()
        {
            _rb = GetComponentInParent<Rigidbody>();

            _interactable = GetComponentInChildren<HandGrabInteractable>();
            
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
            if (hasBeenConfirmed) return;

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
        public void Release()
        {
            float dist = Vector3.Distance(instanceTransform.position, spawner.transform.position);
            if (dist >= confirmDistance)
            {
                Confirm();
            }
            else
            {
                Cancel();
            }
            //ResetEventWrapper();
        }

        /// <summary>
        /// Finalizes the pull, re-enabling interaction and calling confirm logic.
        /// </summary>
        private void Confirm()
        {
            if (hasBeenConfirmed) return;
            hasBeenConfirmed = true;
            instanceTransform.localScale = Vector3.one * maxScale;
            
            if (_rb) _rb.isKinematic = false;
            //RestoreOriginalLayers();
            spawner.HandleConfirm();
            
            XRDebugLogViewer.Log($"Pullable: Confirm");

            enabled = false;
        }

        /// <summary>
        /// Cancels the pull with animation and destroys the instance.
        /// </summary>
        private void Cancel()
        {
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
            

            /*
            if (TryGetComponent(out _eventWrapper))
            {
                // TODO: add release here, make sure these events are removed on release
                _eventWrapper.WhenSelect.AddListener(() => { });
                _eventWrapper.WhenUnselect.AddListener(() => { });
            }
            */

            if (!_interactable)
            {
                XRDebugLogViewer.LogWarning($"Pullable: Attach to Hand - Can't attach to hand - no hand interactable in object");
                return;
            }
            if (_rb) _rb.isKinematic = true;

            // true to allow normal release
            interactor.ForceSelect(_interactable, true);
            XRDebugLogViewer.Log($"Pullable: Attach to Hand - {interactor.gameObject.name} selects {_interactable.gameObject.name} - Rigidbody kinematic {_rb.isKinematic}");
        }

        /*
        private void DetachFromHand()
        {
            interactor.ForceRelease();
        }
        */

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
