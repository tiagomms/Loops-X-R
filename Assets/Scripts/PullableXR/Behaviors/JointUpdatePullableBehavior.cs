using UnityEngine;
using System;

namespace PullableXR
{
    /// <summary>
    /// Behavior that updates a joint's position when a pullable is confirmed or cancelled.
    /// This behavior should be attached to the prefab that contains the joint.
    /// </summary>
    public class JointUpdatePullableBehavior : PullableBehavior
    {
        [Header("Joint Settings")]
        [SerializeField, Tooltip("The joint to update. If null, will try to find a joint on this GameObject")]
        private Joint targetJoint;

        protected override void OnInitialize()
        {
            // If no joint is assigned, try to find one on this GameObject
            if (targetJoint == null)
            {
                targetJoint = GetComponent<Joint>();
            }

            if (targetJoint == null)
            {
                Debug.LogError($"[{nameof(JointUpdatePullableBehavior)}] No Joint found on {gameObject.name}. Please assign a joint or ensure this component is on the same GameObject as the joint.");
                enabled = false;
                return;
            }
        }

        public override void OnPullConfirmed(PullableInstance instance)
        {
            UpdateJointTransform();
        }

        public override void OnPullCancelled(PullableInstance instance)
        {
            UpdateJointTransform();
        }

        private void UpdateJointTransform()
        {
            if (targetJoint == null) return;

            // Update joint's connected anchor to maintain the same relative position
            Vector3 localPosition = targetJoint.transform.InverseTransformPoint(targetJoint.transform.position);
            targetJoint.connectedAnchor = localPosition;

            // Update joint's connected anchor rotation to maintain the same relative rotation
            Quaternion localRotation = Quaternion.Inverse(targetJoint.transform.rotation) * targetJoint.transform.rotation;
            
            // Only update rotation for ConfigurableJoint
            if (targetJoint is ConfigurableJoint configJoint)
            {
                configJoint.targetRotation = localRotation;
            }
        }
    }
} 