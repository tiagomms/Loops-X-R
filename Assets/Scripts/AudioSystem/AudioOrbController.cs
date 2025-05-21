using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        }

        private void OnDestroy()
        {

            controlsManager.Microgestures.OnTap.RemoveListener(ToggleRecording);

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

            isInitialized = true;
        }

        private void InitializeComponents()
        {
            if (!isInitialized) return;
            SetState(LoopOrbState.ReadyToRecord);
        }

        #endregion

        #region State Management

        public void SetState(LoopOrbState newState)
        {
            if (!isInitialized) return;
            if (newState == currentState) return;

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

            currentState = newState;
            particleController.UpdateState(newState);
            identifierController.UpdateVisibility(newState);

        }

        private void HandleReadyToRecord()
        {
            // Can only transition to ReadyToRecord from Disabled
            if (currentState != LoopOrbState.Disabled)
            {
                Debug.LogWarning($"[{nameof(AudioOrbController)}] Invalid state transition to ReadyToRecord from {currentState}");
                return;
            }
        }

        private void HandleRecording()
        {
            // Can only start recording from ReadyToRecord state
            if (currentState != LoopOrbState.ReadyToRecord)
            {
                Debug.LogWarning($"[{nameof(AudioOrbController)}] Invalid state transition to Recording from {currentState}");
                return;
            }

            recordAudioInterface.StartRecording();
        }

        private void HandlePausing()
        {
            // Can pause from either Recording or Playing states
            if (currentState == LoopOrbState.Recording)
            {
                recordAudioInterface.StopRecording();
                identifierController.UpdateIdentifierText(recordAudioInterface.WriteTakeName());
            }
            else if (currentState == LoopOrbState.Playing)
            {
                recordAudioInterface.StopAudioClip();
            }
            else
            {
                Debug.LogWarning($"[{nameof(AudioOrbController)}] Invalid state transition to Pausing from {currentState}");
                return;
            }
        }

        private void HandlePlaying()
        {
            // Can only start playing from Pausing state
            if (currentState != LoopOrbState.Pausing)
            {
                Debug.LogWarning($"[{nameof(AudioOrbController)}] Invalid state transition to Playing from {currentState}");
                return;
            }

            recordAudioInterface.PlayAudioClip();
        }

        private void HandleDisabled()
        {
            // Stop any ongoing operations
            if (currentState == LoopOrbState.Recording)
            {
                recordAudioInterface.StopRecording();
            }
            else if (currentState == LoopOrbState.Playing)
            {
                recordAudioInterface.StopAudioClip();
            }
        }

        #endregion

        #region Event Management
        private void ToggleRecording()
        {
            if (currentState == LoopOrbState.ReadyToRecord)
            {
                SetState(LoopOrbState.Recording);
            }
            else if (currentState == LoopOrbState.Recording)
            {
                SetState(LoopOrbState.Pausing);
            }
        }

        private void TogglePlaying()
        {
            if (currentState == LoopOrbState.Pausing)
            {
                SetState(LoopOrbState.Playing);
            }
            else if (currentState == LoopOrbState.Playing)
            {
                SetState(LoopOrbState.Pausing);
            }
        }
        #endregion
    }
}