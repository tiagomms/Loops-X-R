using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using XRLoopPedal.AudioSystem;

namespace AudioSystem
{
    public class MicController : MonoBehaviour
    {
        public static MicController Instance { get; private set; }

        private float _recordingLength;
        private AudioClip _recordedClip;
        private float _startTime;
        private char _currentInterfaceLetter = 'A';
        private HashSet<AudioOrbController> _activeOrbs = new();

        public AudioClip RecordedClip => _recordedClip;

        [Header("Debug")]
        [SerializeField] private bool isWorking = false;
        [Tooltip("Set respective index - wait for Awake method to know which one to select. In production - this value is ignored to default value 0")]
        [SerializeField] private int micDeviceIndex = 1;
        private int _micIndex = 0;

        // Event for recording state changes
        public UnityEvent<bool> OnRecordingStateChanged = new UnityEvent<bool>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;

            for (int i = 0; i < Microphone.devices.Length; i++)
            {
                XRDebugLogViewer.Log($"Microphone device {i}: {Microphone.devices[i]}");
            }

            #if UNITY_EDITOR
            _micIndex = micDeviceIndex;
            #endif
        }

        private void Start()
        {
            // Subscribe to tap gesture
            ControlsManager.Instance.Microgestures.OnTap.AddListener(HandleTap);
        }

        private void OnDestroy()
        {
            if (ControlsManager.Instance != null)
            {
                ControlsManager.Instance.Microgestures.OnTap.RemoveListener(HandleTap);
            }
            OnRecordingStateChanged.RemoveAllListeners();
        }

        private void HandleTap()
        {
            if (isWorking)
            {
                WorkStop();
                OnRecordingStateChanged.Invoke(false);
            }
            else
            {
                WorkStart();
                OnRecordingStateChanged.Invoke(true);
            }
        }

        public void RegisterOrb(AudioOrbController orb)
        {
            if (orb == null) return;
            _activeOrbs.Add(orb);
        }

        public void UnregisterOrb(AudioOrbController orb)
        {
            if (orb == null) return;
            _activeOrbs.Remove(orb);
        }

        public void WorkStart()
        {
            if (isWorking) return;

            // Find all orbs ready to record
            var readyOrbs = _activeOrbs.Where(orb => orb.CurrentState == LoopOrbState.ReadyToRecord).ToList();
            if (readyOrbs.Count == 0) return;

            // Set interface name for all ready orbs
            foreach (var orb in readyOrbs)
            {
                orb.recordAudioInterface.SetInterfaceName(_currentInterfaceLetter.ToString());
            }

#if !UNITY_WEBGL
            isWorking = true;

            string device = Microphone.devices[_micIndex];
            int sampleRate = 44100;
            int lengthSec = 3599;

            _recordedClip = Microphone.Start(device, true, lengthSec, sampleRate);
            _startTime = Time.realtimeSinceStartup;
            XRDebugLogViewer.Log($"Started recording with device: {device}");
#endif
        }

        public AudioClip WorkStop()
        {
            if (!isWorking) return _recordedClip;

#if !UNITY_WEBGL
            isWorking = false;
            Microphone.End(null);
            _recordingLength = Time.realtimeSinceStartup - _startTime;
            _recordedClip = TrimClip(_recordedClip, _recordingLength);
            XRDebugLogViewer.Log($"Stopped recording. Length: {_recordingLength:F2}s");
            
            // Increment interface letter for next recording
            _currentInterfaceLetter++;
            
            return _recordedClip;
#endif
            return null;
        }

        private AudioClip TrimClip(AudioClip clip, float length)
        {
            // Calculate the number of samples to keep
            int samples = Mathf.CeilToInt(clip.frequency * length);
            if (samples <= 0 || samples > clip.samples)
            {
                XRDebugLogViewer.LogWarning($"Invalid sample count: {samples}. Using original clip.");
                return clip;
            }

            // Create arrays for each channel
            float[] data = new float[samples * clip.channels];

            // Get the audio data
            if (!clip.GetData(data, 0))
            {
                XRDebugLogViewer.LogError("Failed to get audio data from clip");
                return clip;
            }

            // Create the new clip
            AudioClip trimmedClip = AudioClip.Create(
                $"{clip.name}_trimmed",
                samples,
                clip.channels,
                clip.frequency,
                false
            );

            // Set the data
            if (!trimmedClip.SetData(data, 0))
            {
                XRDebugLogViewer.LogError("Failed to set audio data to trimmed clip");
                return clip;
            }

            return trimmedClip;
        }
    }
}
