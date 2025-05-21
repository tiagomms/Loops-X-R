using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Makes an object follow another object's height (y-axis) with smooth interpolation.
    /// Useful for UI elements or objects that should maintain a consistent height relative to a target object.
    /// </summary>
    public class FollowObjectHeight : MonoBehaviour
    {
        [Header("Height Settings")]
        [SerializeField] private Transform targetObject;
        [SerializeField, Range(0f, 1f)] private float heightLerpSpeed = 0.1f;
        [SerializeField] private float heightOffset = 0f;
        [SerializeField] private bool maintainLocalXZ = true;

        private Vector3 _targetPosition;

        private void Start()
        {
            if (targetObject == null)
            {
                Debug.LogError($"Target object not assigned on {gameObject.name}");
                enabled = false;
                return;
            }
            _targetPosition = transform.position;
        }

        private void Update()
        {
            if (targetObject == null) return;

            // Get target position based on object height
            _targetPosition = transform.position;
            _targetPosition.y = targetObject.position.y + heightOffset;

            // Apply smooth height transition
            transform.position = Vector3.Lerp(transform.position, _targetPosition, heightLerpSpeed);

            // Optionally maintain local XZ position
            if (maintainLocalXZ)
            {
                Vector3 localPos = transform.localPosition;
                localPos.y = transform.position.y;
                transform.localPosition = localPos;
            }
        }
    }
} 