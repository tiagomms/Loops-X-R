using UnityEngine;
using System;

namespace PullableXR
{
    /// <summary>
    /// Behavior that updates a joint's position when a pullable is confirmed or cancelled.
    /// This behavior should be attached to the prefab that contains the joint.
    /// </summary>
    public class AudioOrbPullableBehavior : PullableBehavior
    {
        [Header("Joint Settings")]
        [SerializeField, Tooltip("The joint to update. If null, throws error.")]
        private Joint targetJoint;

        [SerializeField, Tooltip("The Rigidbody to update. If null, If null, throws error.")]
        private Rigidbody rigidbody;


        protected override void OnInitialize()
        {
            if (targetJoint == null || rigidbody == null)
            {
                Debug.LogError($"[{nameof(AudioOrbPullableBehavior)}] No Joint and/or Rigidbody found on {gameObject.name}. Please assign a joint and rigidbody.");
                enabled = false;
                return;
            }
            rigidbody.isKinematic = true; // force to kinematic
        }

        public override void OnPullConfirmed(PullableInstance instance)
        {
            /*
            UpdateJointTransform();
            rigidbody.isKinematic = false; // force to not be kinematic
            */
            XRDebugLogViewer.Log($"[{nameof(AudioOrbPullableBehavior)}] Pull confirmed");
        }

        public override void OnPullCancelled(PullableInstance instance)
        {
            //UpdateJointTransform();
        }

        private void UpdateJointTransform()
        {
            if (targetJoint == null) return;

            // Update joint's connected anchor to maintain the same relative position
            /*
            Vector3 localPosition = targetJoint.transform.InverseTransformPoint(targetJoint.transform.position);
            targetJoint.connectedAnchor = localPosition;
            */
            //targetJoint.connectedAnchor = rigidbody.transform.position;
            /*
            // Update joint's connected anchor rotation to maintain the same relative rotation
            Quaternion localRotation = Quaternion.Inverse(targetJoint.transform.rotation) * targetJoint.transform.rotation;
            
            // Only update rotation for ConfigurableJoint
            if (targetJoint is ConfigurableJoint configJoint)
            {
                configJoint.targetRotation = localRotation;
            }
            */
        }
    }
} 