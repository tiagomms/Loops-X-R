using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace PullableXR
{
    public class PullableSpawnerOld02 : MonoBehaviour
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

        [Header("Events")]
        public UnityEvent onConfirm;
        public UnityEvent onCancel;

        private PullableInstanceOld02 activeInstance;

        public void TriggerPull(Transform handTransform)
        {
            if (activeInstance != null) return;

            GameObject spawned = Instantiate(pullablePrefab);
            Transform spawnedT = spawned.transform;

            // Set initial position and billboard
            Vector3 spawnPos = transform.position + spawnOffset;
            spawnedT.position = spawnPos;
            spawnedT.LookAt(Camera.main.transform);
            spawnedT.localScale = Vector3.one * minScale;

            activeInstance = spawned.AddComponent<PullableInstanceOld02>();
            activeInstance.Initialize(this, spawnedT, spawnPos, handTransform, confirmDistance, minScale, maxScale, failedReleaseDuration, failedReleaseEase);
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

    public class PullableInstanceOld02 : MonoBehaviour
    {
        private PullableSpawnerOld02 spawner;
        private Transform instanceT;
        private Transform handT;
        private Vector3 initialPos;
        private float confirmDistance;
        private float minScale;
        private float maxScale;
        private float failedDuration;
        private Ease failedEase;
        private bool isReleased;

        public void Initialize(PullableSpawnerOld02 spawner, Transform instanceT, Vector3 initialPos, Transform handT, float confirmDistance, float minScale, float maxScale, float failedDuration, Ease failedEase)
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
        }

        private void Update()
        {
            if (isReleased) return;

            instanceT.position = handT.position;
            float distance = Vector3.Distance(instanceT.position, spawner.transform.position);

            // Lerp scale based on distance
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
                // Confirmed placement
                instanceT.localScale = Vector3.one * maxScale;
                spawner.HandleConfirm();
            }
            else
            {
                // Animate back to spawn position and shrink
                Sequence cancelSeq = DOTween.Sequence();
                cancelSeq.Join(instanceT.DOMove(initialPos, failedDuration).SetEase(failedEase));
                cancelSeq.Join(instanceT.DOScale(Vector3.one * minScale, failedDuration).SetEase(failedEase));
                cancelSeq.OnComplete(() => Destroy(gameObject));

                spawner.HandleCancel();
            }
        }
    }
}
