using TMPro;
using UnityEngine;

namespace AudioSystem
{
    /// <summary>
    /// Detects and displays tempo information from audio recordings.
    /// TODO: Consider dependency injection for OnsetBpmAndTimeSignatureEstimator in the future.
    /// </summary>
    public class TempoController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshPro tempoText;

        private OnsetBpmAndTimeSignatureEstimator _estimator;
        private bool _isInitialized;
        private float _disableCollisionTimer = 0f;

        // To reset so that it does not invoke too many times in a row.
        private void Update()
        {
            if (_disableCollisionTimer < 2f)
                _disableCollisionTimer += Time.deltaTime;
        }

        private void Awake()
        {
            if (tempoText == null)
            {
                Debug.LogError($"Tempo text not assigned on {gameObject.name}");
                enabled = false;
                return;
            }

            // Initialize the estimator service
            // TODO: Replace with dependency injection
            _estimator = new OnsetBpmAndTimeSignatureEstimator();

            // Ensure text is disabled initially
            tempoText.gameObject.SetActive(false);
            _isInitialized = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isInitialized) return;
            // disable collisions at start
            if (_disableCollisionTimer < 2f) return;

            // Check if the colliding object has a RecordAudioInterface in its hierarchy
            var audioInterface = other.transform.parent.GetComponent<RecordAudioInterface>();
            if (audioInterface == null) return;

            // Get the audio source and verify it has a clip
            var audioSource = audioInterface.audioSource;
            if (audioSource == null || audioSource.clip == null) return;

            // Get the audio data
            float[] samples = new float[audioSource.clip.samples];
            audioSource.clip.GetData(samples, 0);

            // Estimate tempo and time signature
            var (bpm, beatsPerBar) = _estimator.Estimate(samples);

            // Update the text display
            
            if (bpm > 0)
            {
                tempoText.text = $"4/4\n{bpm} bpm";
                tempoText.gameObject.SetActive(true);
            }
            
            // FIXME: does not work estimate
            tempoText.text = "Coming Soon";//$"4/4\n{bpm} bpm"; 
            tempoText.gameObject.SetActive(true);
            XRDebugLogViewer.Log($"[{nameof(TempoController)}] bpm: {bpm}; beats per bar: {beatsPerBar}");
            _disableCollisionTimer = 0f; // reset timer
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_isInitialized) return;
            // Check if the colliding object has a RecordAudioInterface in its hierarchy
            var audioInterface = other.transform.parent.GetComponent<RecordAudioInterface>();
            if (audioInterface == null) return;
            _disableCollisionTimer = 0f; // reset timer


            // Hide the text when the object leaves the trigger
            //tempoText.gameObject.SetActive(false);
        }
    }
}