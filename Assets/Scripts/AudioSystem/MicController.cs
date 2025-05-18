using UnityEngine;

namespace AudioSystem
{
    public class MicController : MonoBehaviour
    {
        public bool IsWorking = false;
        private float _recordingLength;
        private AudioClip _recordedClip;
        private float _startTime;

        public AudioClip RecordedClip => _recordedClip;

        private void Awake()
        {
            for (int i = 0; i < Microphone.devices.Length; i++)
            {
                Debug.Log($"Device name {i}: {Microphone.devices[i]}");
            }
        }


        public void WorkStart()
        {
            if (IsWorking) return;

#if !UNITY_WEBGL
            IsWorking = true;

            string device = Microphone.devices[0];
            int sampleRate = 44100;
            int lengthSec = 3599;

            _recordedClip = Microphone.Start(device, true, lengthSec, sampleRate);
            _startTime = Time.realtimeSinceStartup;


#endif
        }

        public AudioClip WorkStop()
        {
            if (!IsWorking) return _recordedClip;

#if !UNITY_WEBGL
            IsWorking = false;
            Microphone.End(null);
            _recordingLength = Time.realtimeSinceStartup - _startTime;
            _recordedClip = TrimClip(_recordedClip, _recordingLength);
            return _recordedClip;
#endif
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
