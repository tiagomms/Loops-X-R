using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

namespace PullableXR
{
    /// <summary>
    /// Behavior that updates a joint's position when an interactable is unselected.
    /// This behavior should be attached to the prefab that has the joint. 
    /// </summary>
    public class JointUpdateOnGrabBehavior : MonoBehaviour
    {
        // NOTE: never use is kinematic on grabbable objects, if you want to have control over the joint's position
        [Header("Joint Settings")]
        [SerializeField, Tooltip("The joint to update")]
        private Joint targetJoint;

        [Header("Rigidbody Settings")]
        [SerializeField, Tooltip("The rigidbody to control. If null, will try to find one on the joint's GameObject")]
        private Rigidbody targetRigidbody;

        [SerializeField] private InteractableUnityEventWrapper _eventWrapper;
        private bool _wasKinematic;

        // NOTE: Setting update joint transform on Start is already too late, the joint's position is already set.
        private void Awake()
        {
            if (targetJoint == null)
            {
                targetJoint = GetComponent<Joint>();
            }

            if (targetJoint == null)
            {
                return;
            }
            // NOTE: make sure is false to have control over the joint's position
            targetJoint.autoConfigureConnectedAnchor = false;
            UpdateJointTransform();
        }
        private void Start()
        {
            if (targetJoint == null)
            {
                Debug.LogError($"[{nameof(JointUpdateOnGrabBehavior)}] No Joint found on {gameObject.name}");
                enabled = false;
                return;
            }

            // Try to find rigidbody if not assigned
            if (targetRigidbody == null)
            {
                targetRigidbody = targetJoint.GetComponent<Rigidbody>();
                if (targetRigidbody == null)
                {
                    targetRigidbody = targetJoint.connectedBody;
                }
            }

            if (targetRigidbody == null)
            {
                Debug.LogError($"[{nameof(JointUpdateOnGrabBehavior)}] No Rigidbody found for joint on {gameObject.name}");
                enabled = false;
                return;
            }

            // Store initial kinematic state
            _wasKinematic = targetRigidbody.isKinematic;

            if (_eventWrapper == null)
            {
                _eventWrapper = GetComponent<InteractableUnityEventWrapper>();
            }
            if (_eventWrapper == null)
            {
                Debug.LogError($"[{nameof(JointUpdateOnGrabBehavior)}] No InteractableUnityEventWrapper found on {gameObject.name}");
                enabled = false;
                return;
            }

            // Subscribe to events
            _eventWrapper.WhenSelect.AddListener(OnSelected);
            _eventWrapper.WhenUnselect.AddListener(OnUnselected);
        }

        private void OnDestroy()
        {
            if (_eventWrapper != null)
            {
                _eventWrapper.WhenSelect.RemoveListener(OnSelected);
                _eventWrapper.WhenUnselect.RemoveListener(OnUnselected);
            }
        }

        private void OnSelected()
        {
            if (targetRigidbody == null) return;

            // Store current state and make kinematic
            _wasKinematic = targetRigidbody.isKinematic;
            targetRigidbody.isKinematic = true;
            // NOTE: if I don't set to false, it does not update after grab


            XRDebugLogViewer.Log($"[{nameof(JointUpdateOnGrabBehavior)}] On Select - Made kinematic");
        }

        /// <summary>
        /// Called when the object is unselected. Updates the joint position.
        /// </summary>
        private void OnUnselected()
        {
            if (targetRigidbody == null) return;

            // Update joint first
            UpdateJointTransform();

            // Force physics update
            //Physics.SyncTransforms(); // not doing anything

            // Restore kinematic state
            // TODO: delay setting is kinematic back to original state 
            targetRigidbody.isKinematic = _wasKinematic;

            XRDebugLogViewer.Log($"[{nameof(JointUpdateOnGrabBehavior)}] On Unselect - Restored kinematic state to {_wasKinematic}");
        }

        /// <summary>
        /// Updates the joint's transform to match the current object position.
        /// </summary>
        private void UpdateJointTransform()
        {
            if (targetJoint == null) return;

            // Update joint's connected anchor to maintain the same relative position
            //Vector3 localPosition = transform.InverseTransformPoint(targetJoint.transform.position);
            targetJoint.connectedAnchor = targetJoint.transform.position;

            /*
            // Update joint's connected anchor rotation to maintain the same relative rotation
            Quaternion localRotation = Quaternion.Inverse(transform.rotation) * targetJoint.transform.rotation;
            
            // Only update rotation for ConfigurableJoint
            if (targetJoint is ConfigurableJoint configJoint)
            {
                configJoint.targetRotation = localRotation;
            }
            */

            //XRDebugLogViewer.Log($"[{nameof(JointUpdateOnUnselectBehavior)}] Joint transform updated");// - Position: {localPosition}, Rotation: {localRotation}");
        }
    }
}