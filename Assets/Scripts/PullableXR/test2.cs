using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections.Generic;

namespace PullableXR
{
    public class PullableSpawnerOld01 : MonoBehaviour
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

        private PullableInstanceOld01 activeInstance;

        public void TriggerPull(Transform handTransform)
        {
            if (activeInstance != null) return;

            GameObject spawned = Instantiate(pullablePrefab);
            Transform spawnedT = spawned.transform;

            Vector3 spawnPos = transform.position + spawnOffset;
            spawnedT.position = spawnPos;
            spawnedT.LookAt(Camera.main.transform);
            spawnedT.localScale = Vector3.one * minScale;

            activeInstance = spawned.AddComponent<PullableInstanceOld01>();
            activeInstance.Initialize(this, spawnedT, spawnPos, handTransform, confirmDistance, minScale, maxScale, failedReleaseDuration, failedReleaseEase, temporaryLayerName);
        }

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

    public class PullableInstanceOld01 : MonoBehaviour
    {
        private PullableSpawnerOld01 spawner;
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

        public void Initialize(PullableSpawnerOld01 spawner, Transform instanceT, Vector3 initialPos, Transform handT, float confirmDistance, float minScale, float maxScale, float failedDuration, Ease failedEase, string temporaryLayerName)
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
                this.enabled = false; // stop processing it
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
    }
}