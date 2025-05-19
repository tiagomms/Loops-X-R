using UnityEngine;
using UnityEngine.Events;
using Core;
using AudioSystem;
using System.Collections.Generic;

namespace VR.Controls
{
    public class MicrogesturesController : MonoBehaviour
    {
        [SerializeField]
        private ControlsManager controlsManager;

        [SerializeField]
        private OVRMicrogestureEventSource leftGestureSource;

        [SerializeField]
        private OVRMicrogestureEventSource rightGestureSource;

        [SerializeField] 
        private AppState validOnTapStates = AppState.None;
        
        [SerializeField] 
        private AppState validOnSwipeRightLeftStates = AppState.None;
        
        [SerializeField] 
        private AppState validOnSwipeForwardBackwardStates = AppState.None;

        public UnityEvent OnTap;
        public UnityEvent OnSwipeLeft;
        public UnityEvent OnSwipeRight;
        public UnityEvent OnSwipeForward;
        public UnityEvent OnSwipeBackward;

        void Start()
        {
            leftGestureSource.GestureRecognizedEvent.AddListener(gesture => OnGestureRecognized(OVRPlugin.Hand.HandLeft, gesture));
            rightGestureSource.GestureRecognizedEvent.AddListener(gesture => OnGestureRecognized(OVRPlugin.Hand.HandRight, gesture));
        }

        private void OnGestureRecognized(OVRPlugin.Hand hand, OVRHand.MicrogestureType gesture)
        {
            InvokeGestureEvent(hand, gesture);
            Debug.Log($"Gesture Recognized: {hand.ToString()} : {gesture.ToString()}");
        }

        private void InvokeGestureEvent(OVRPlugin.Hand hand, OVRHand.MicrogestureType gesture)
        {
            AppState currentState = GlobalEventManager.Instance.CurrentState;
            
            switch (gesture)
            {
                case OVRHand.MicrogestureType.ThumbTap:
                    if ((currentState & validOnTapStates) == 0) break;
                    OnTap.Invoke();
                    break;
                    
                case OVRHand.MicrogestureType.SwipeLeft:
                    if ((currentState & validOnSwipeRightLeftStates) == 0) break;
                    OnSwipeLeft.Invoke();
                    break;
                    
                case OVRHand.MicrogestureType.SwipeRight:
                    if ((currentState & validOnSwipeRightLeftStates) == 0) break;
                    OnSwipeRight.Invoke();
                    break;
                    
                case OVRHand.MicrogestureType.SwipeForward:
                    if ((currentState & validOnSwipeForwardBackwardStates) == 0) break;
                    OnSwipeForward.Invoke();
                    break;
                    
                case OVRHand.MicrogestureType.SwipeBackward:
                    if ((currentState & validOnSwipeForwardBackwardStates) == 0) break;
                    OnSwipeBackward.Invoke();
                    break;
            }
        }
    }
}
