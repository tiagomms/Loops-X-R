using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

namespace PullableXR
{
    /// <summary>
    /// Behavior that updates a joint's position when an interactable is unselected.
    /// This behavior should be attached to the prefab that has the joint.
    /// </summary>
    public class JointUpdateOnUnselectBehavior : MonoBehaviour
    {
        [Header("Joint Settings")]
        [SerializeField, Tooltip("The joint to update")]
        private Joint targetJoint;

        private InteractableUnityEventWrapper _eventWrapper;

        private void Start()
        {
            if (targetJoint == null)
            {
                targetJoint = GetComponent<Joint>();
            }

            if (targetJoint == null)
            {
                Debug.LogError($"[{nameof(JointUpdateOnUnselectBehavior)}] No Joint found on {gameObject.name}");
                enabled = false;
                return;
            }

            _eventWrapper = GetComponent<InteractableUnityEventWrapper>();
            if (_eventWrapper == null)
            {
                Debug.LogError($"[{nameof(JointUpdateOnUnselectBehavior)}] No InteractableUnityEventWrapper found on {gameObject.name}");
                enabled = false;
                return;
            }
            // Subscribe to unselect event
            _eventWrapper.WhenUnselect.AddListener(OnUnselected);
        }

        private void OnDestroy()
        {
            if (_eventWrapper != null)
            {
                _eventWrapper.WhenUnselect.RemoveListener(OnUnselected);
            }
        }

        /// <summary>
        /// Called when the object is unselected. Updates the joint position.
        /// </summary>
        private void OnUnselected()
        {
            UpdateJointTransform();
            XRDebugLogViewer.Log($"[{nameof(JointUpdateOnUnselectBehavior)}] Joint updated on unselect");
        }

        /// <summary>
        /// Updates the joint's transform to match the current object position.
        /// </summary>
        private void UpdateJointTransform()
        {
            if (targetJoint == null) return;

            // Update joint's connected anchor to maintain the same relative position
            Vector3 localPosition = transform.InverseTransformPoint(targetJoint.transform.position);
            targetJoint.connectedAnchor = localPosition;

            // Update joint's connected anchor rotation to maintain the same relative rotation
            Quaternion localRotation = Quaternion.Inverse(transform.rotation) * targetJoint.transform.rotation;
            
            // Only update rotation for ConfigurableJoint
            if (targetJoint is ConfigurableJoint configJoint)
            {
                configJoint.targetRotation = localRotation;
            }

            XRDebugLogViewer.Log($"[{nameof(JointUpdateOnUnselectBehavior)}] Joint transform updated - Position: {localPosition}, Rotation: {localRotation}");
        }
    }
} 