using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Oculus.Interaction.Input;

namespace XRLoopPedal.AudioSystem
{
    /// <summary>
    /// Core controller for each orb that manages its state and coordinates between audio and visual components
    /// </summary>
    public class AudioOrbController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private RecordAudioInterface recordAudioInterface;
        [SerializeField] private OrbParticleController particleController;
        [SerializeField] private OrbIdentifierUIController identifierController;
        [SerializeField] private SoundEmitter soundEmitter;

        [Header("State")]
        [SerializeField] private LoopOrbState currentState;

        // Properties
        public LoopOrbState CurrentState => currentState;

        // Private fields
        private ControlsManager controlsManager;
        private bool isInitialized;


        #region Unity Lifecycle

        private void Awake()
        {
            ValidateDependencies();
            InitializeComponents();
        }

        private void Start()
        {
            controlsManager = ControlsManager.Instance;
            if (controlsManager == null)
            {
                Debug.LogError($"[{nameof(ControlsManager)}] No singleton available. Please add gameobject in scene");
                isInitialized = false;
                return;
            }
            controlsManager.Microgestures.OnTap.AddListener(ToggleRecording);
            controlsManager.PlayPoseController.OnPoseStart.AddListener(hand => SetStateIfPossible(LoopOrbState.Playing));
            controlsManager.StopPoseController.OnPoseStart.AddListener(hand => SetStateIfPossible(LoopOrbState.Pausing));

            if (soundEmitter != null)
            {
                soundEmitter.onEmitSound.AddListener(TogglePlaying);
            }
        }

        private void OnDestroy()
        {
            if (!isInitialized) return;
            controlsManager.Microgestures.OnTap.RemoveListener(ToggleRecording);
            controlsManager.PlayPoseController.OnPoseStart.RemoveListener(hand => SetStateIfPossible(LoopOrbState.Playing));
            controlsManager.StopPoseController.OnPoseStart.RemoveListener(hand => SetStateIfPossible(LoopOrbState.Pausing));

            if (soundEmitter != null)
            {
                soundEmitter.onEmitSound.RemoveListener(TogglePlaying);
            }
        }

        private void Update()
        {
            if (!isInitialized) return;
            particleController.UpdateVolume(recordAudioInterface.audioSource.volume);
        }

        #endregion

        #region Initialization

        private void ValidateDependencies()
        {
            if (recordAudioInterface == null)
            {
                Debug.LogError($"[{nameof(AudioOrbController)}] RecordAudioInterface reference is missing on {gameObject.name}");
                return;
            }

            if (particleController == null)
            {
                particleController = GetComponent<OrbParticleController>();
                if (particleController == null)
                {
                    Debug.LogError($"[{nameof(AudioOrbController)}] OrbParticleController not found on {gameObject.name}");
                    return;
                }
            }

            if (identifierController == null)
            {
                identifierController = GetComponent<OrbIdentifierUIController>();
                if (identifierController == null)
                {
                    Debug.LogError($"[{nameof(AudioOrbController)}] OrbIdentifierUIController not found on {gameObject.name}");
                    return;
                }
            }

            if (soundEmitter == null)
            {
                soundEmitter = GetComponent<SoundEmitter>();
                if (soundEmitter == null)
                {
                    Debug.LogWarning($"[{nameof(AudioOrbController)}] SoundEmitter not found on {gameObject.name}");
                }
            }

            isInitialized = true;
        }

        private void InitializeComponents()
        {
            if (!isInitialized) return;
            SetStateIfPossible(LoopOrbState.ReadyToRecord);
        }

        #endregion

        #region State Management

        public void SetStateIfPossible(LoopOrbState newState)
        {
            if (!isInitialized) return;
            if (newState == currentState) return;
            if (!IsValidStateTransition(newState)) return;

            // Handle state transition
            switch (newState)
            {
                case LoopOrbState.ReadyToRecord:
                    HandleReadyToRecord();
                    break;
                case LoopOrbState.Recording:
                    HandleRecording();
                    break;
                case LoopOrbState.Pausing:
                    HandlePausing();
                    break;
                case LoopOrbState.Playing:
                    HandlePlaying();
                    break;
                case LoopOrbState.Disabled:
                    HandleDisabled();
                    break;
            }

            XRDebugLogViewer.Log($"Orb {gameObject.name} state changed from {currentState} to {newState}");
            currentState = newState;
            particleController.UpdateState(newState);
            identifierController.UpdateVisibility(newState);
        }

        private bool IsValidStateTransition(LoopOrbState newState)
        {
            return (currentState, newState) switch
            {
                // Can only transition to ReadyToRecord from Disabled
                (LoopOrbState.Disabled, LoopOrbState.ReadyToRecord) => true,
                
                // Can only start recording from ReadyToRecord state
                (LoopOrbState.ReadyToRecord, LoopOrbState.Recording) => true,
                
                // Can pause from either Recording or Playing states
                (LoopOrbState.Recording, LoopOrbState.Pausing) => true,
                (LoopOrbState.Playing, LoopOrbState.Pausing) => true,
                
                // Can only start playing from Pausing state
                (LoopOrbState.Pausing, LoopOrbState.Playing) => true,
                
                // Can transition to Disabled from any state
                (_, LoopOrbState.Disabled) => true,
                
                // All other transitions are invalid
                _ => false
            };
        }

        private void HandleReadyToRecord()
        {
            XRDebugLogViewer.Log($"Orb {gameObject.name} ready to record");
        }

        private void HandleRecording()
        {
            XRDebugLogViewer.Log($"Orb {gameObject.name} started recording");
            recordAudioInterface.StartRecording();
        }

        private void HandlePausing()
        {
            if (currentState == LoopOrbState.Recording)
            {
                XRDebugLogViewer.Log($"Orb {gameObject.name} stopped recording");
                recordAudioInterface.StopRecording();
                identifierController.UpdateIdentifierText(recordAudioInterface.WriteTakeName());
            }
            else if (currentState == LoopOrbState.Playing)
            {
                XRDebugLogViewer.Log($"Orb {gameObject.name} paused playback");
                recordAudioInterface.StopAudioClip();
            }
        }

        private void HandlePlaying()
        {
            XRDebugLogViewer.Log($"Orb {gameObject.name} started playback");
            recordAudioInterface.PlayAudioClip();
        }

        private void HandleDisabled()
        {
            if (currentState == LoopOrbState.Recording)
            {
                XRDebugLogViewer.Log($"Orb {gameObject.name} disabled while recording");
                recordAudioInterface.StopRecording();
            }
            else if (currentState == LoopOrbState.Playing)
            {
                XRDebugLogViewer.Log($"Orb {gameObject.name} disabled while playing");
                recordAudioInterface.StopAudioClip();
            }
        }

        #endregion

        #region Event Management
        private void ToggleRecording()
        {
            if (currentState == LoopOrbState.ReadyToRecord)
            {
                SetStateIfPossible(LoopOrbState.Recording);
            }
            else if (currentState == LoopOrbState.Recording)
            {
                SetStateIfPossible(LoopOrbState.Pausing);
            }
        }

        private void TogglePlaying()
        {
            if (currentState == LoopOrbState.Pausing)
            {
                SetStateIfPossible(LoopOrbState.Playing);
            }
            else if (currentState == LoopOrbState.Playing)
            {
                SetStateIfPossible(LoopOrbState.Pausing);
            }
        }
        #endregion
    }
}