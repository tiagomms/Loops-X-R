using UnityEngine;
using UnityEngine.Events;
using Core;
using AudioSystem;
using System.Collections.Generic;
using Oculus.Interaction;

namespace VR.Controls
{
    public enum HandPose
    {
        Play = 0,
        Stop = 1
    }
    public class HandPoseController : MonoBehaviour
    {

        [SerializeField] private HandPose pose;
        public HandPose Pose => pose;

        [SerializeField]
        private SelectorUnityEventWrapper leftPoseSource;

        [SerializeField]
        private SelectorUnityEventWrapper rightPoseSource;

        [SerializeField] 
        private AppState validOnSelectedStates = AppState.None;
        
        [SerializeField] 
        private AppState validOnUnselectedStates = AppState.None;

        public UnityEvent<HandPose> OnPoseStart;
        public UnityEvent<HandPose> OnPoseEnd;
        
        void Start()
        {
            leftPoseSource.WhenSelected.AddListener(() => OnPoseRecognized(OVRPlugin.Hand.HandLeft, pose));
            rightPoseSource.WhenSelected.AddListener(() => OnPoseRecognized(OVRPlugin.Hand.HandRight, pose));

            leftPoseSource.WhenUnselected.AddListener(() => OnPoseEnded(OVRPlugin.Hand.HandLeft, pose));
            rightPoseSource.WhenUnselected.AddListener(() => OnPoseEnded(OVRPlugin.Hand.HandRight, pose));
        }

        private void OnPoseRecognized(OVRPlugin.Hand hand, HandPose pose)
        {
            AppState currentState = GlobalEventManager.Instance.CurrentState;
            if ((currentState & validOnSelectedStates) == 0) return;

            OnPoseStart.Invoke(pose);

            Debug.Log($"Pose Recognized: {hand.ToString()} : {pose.ToString()}");
        }

        
        private void OnPoseEnded(OVRPlugin.Hand hand, HandPose pose)
        {
            AppState currentState = GlobalEventManager.Instance.CurrentState;
            if ((currentState & validOnUnselectedStates) == 0) return;

            OnPoseEnd.Invoke(pose);

            Debug.Log($"Pose Ended: {hand.ToString()} : {pose.ToString()}");
        }

    }
}
