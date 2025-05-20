using UnityEngine;
using System;

namespace PullableXR
{
    /// <summary>
    /// Base class for all pullable behaviors.
    /// Provides common functionality and a clear structure for behavior implementations.
    /// </summary>
    public abstract class PullableBehavior : MonoBehaviour
    {
        /// <summary>
        /// Called when the pull is confirmed.
        /// </summary>
        /// <param name="instance">The pullable instance that was confirmed</param>
        public virtual void OnPullConfirmed(PullableInstance instance)
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Called when the pull is cancelled.
        /// </summary>
        /// <param name="instance">The pullable instance that was cancelled</param>
        public virtual void OnPullCancelled(PullableInstance instance)
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Called when the behavior is initialized.
        /// Override this to perform any setup needed.
        /// </summary>
        protected virtual void OnInitialize()
        {
            // Default implementation does nothing
        }

        private void Start()
        {
            OnInitialize();
        }
    }
} 