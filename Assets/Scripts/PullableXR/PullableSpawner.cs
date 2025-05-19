using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace PullableXR
{
    /// <summary>
    /// Main class attached to the spawner. Handles instantiation and pull interaction of prefab objects.
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
        [SerializeField] private string temporaryLayerName = "TempUninteractive";

        [Header("Events")]
        public UnityEvent onConfirm;
        public UnityEvent onCancel;

        private PullableInstance activeInstance;

        /// <summary>
        /// Call this method when the user pinches or grabs the spawner to trigger the pull interaction.
        /// </summary>
        public void TriggerPull(Transform handTransform)
        {
            if (activeInstance != null) return;

            GameObject spawned = Instantiate(pullablePrefab);
            Transform spawnedT = spawned.transform;

            Vector3 spawnPos = transform.position + spawnOffset;
            spawnedT.position = spawnPos;
            spawnedT.LookAt(Camera.main.transform);
            spawnedT.localScale = Vector3.one * minScale;

            activeInstance = spawned.AddComponent<PullableInstance>();
            activeInstance.Initialize(this, spawnedT, spawnPos, handTransform, confirmDistance, minScale, maxScale, failedReleaseDuration, failedReleaseEase, temporaryLayerName);
        }

        /// <summary>
        /// Call this when the user releases the pinch. It finalizes the interaction.
        /// </summary>
        public void Release()
        {
            if (activeInstance != null)
            {
                activeInstance.Release();
                activeInstance = null;
            }
        }

        public void HandleConfirm()
        {
            onConfirm?.Invoke();
        }

        public void HandleCancel()
        {
            onCancel?.Invoke();
        }
    }
}
