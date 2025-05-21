using UnityEngine;

namespace AudioSystem
{
    public class MicController : MonoBehaviour
    {
        public static MicController Instance { get; private set; }

        private float _recordingLength;
        private AudioClip _recordedClip;
        private float _startTime;

        public AudioClip RecordedClip => _recordedClip;

        [Header("Debug")]
        [SerializeField] private bool isWorking = false;
        [Tooltip("Set respective index - wait for Awake method to know which one to select. In production - this value is ignored to default value 0")]
        [SerializeField] private int micDeviceIndex = 1;
        private int _micIndex = 0;

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
                Debug.Log($"Device name {i}: {Microphone.devices[i]}");
            }

            #if UNITY_EDITOR
            _micIndex = micDeviceIndex; // NOTE: whatever you set on debug, usually is 0 (for deployment)
            #endif
        }

        public void WorkStart()
        {
            if (isWorking) return;

#if !UNITY_WEBGL
            isWorking = true;

            string device = Microphone.devices[_micIndex];
            int sampleRate = 44100;
            int lengthSec = 3599;

            _recordedClip = Microphone.Start(device, true, lengthSec, sampleRate);
            _startTime = Time.realtimeSinceStartup;
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
                Debug.LogWarning($"Invalid sample count: {samples}. Using original clip.");
                return clip;
            }

            // Create arrays for each channel
            float[] data = new float[samples * clip.channels];

            // Get the audio data
            if (!clip.GetData(data, 0))
            {
                Debug.LogError("Failed to get audio data from clip");
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
                Debug.LogError("Failed to set audio data to trimmed clip");
                return clip;
            }

            return trimmedClip;
        }
    }
}
