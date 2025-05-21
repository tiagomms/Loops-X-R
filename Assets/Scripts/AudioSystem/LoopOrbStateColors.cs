using UnityEngine;

namespace XRLoopPedal.AudioSystem
{
    /// <summary>
    /// ScriptableObject that manages colors for different orb states
    /// </summary>
    [CreateAssetMenu(fileName = "LoopOrbStateColors", menuName = "XR Loop Pedal/Orb State Colors")]
    public class LoopOrbStateColors : ScriptableObject
    {
        private static LoopOrbStateColors instance;
        public static LoopOrbStateColors Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<LoopOrbStateColors>("LoopOrbStateColors");
                    if (instance == null)
                    {
                        Debug.LogError("LoopOrbStateColors asset not found in Resources folder! Creating default instance.");
                        instance = CreateInstance<LoopOrbStateColors>();
                        instance.InitializeDefaultColors();
                    }
                }
                return instance;
            }
        }

        [Header("State Colors")]
        [SerializeField] private Color readyToRecordColor = new Color(0.2f, 0.8f, 0.2f); // Green
        [SerializeField] private Color recordingColor = new Color(0.8f, 0.2f, 0.2f);    // Red
        [SerializeField] private Color pausingColor = new Color(0.8f, 0.8f, 0.2f);      // Yellow
        [SerializeField] private Color playingColor = new Color(0.2f, 0.2f, 0.8f);      // Blue
        [SerializeField] private Color disabledColor = new Color(0.5f, 0.5f, 0.5f);     // Gray

        private void InitializeDefaultColors()
        {
            readyToRecordColor = new Color(0.2f, 0.8f, 0.2f);
            recordingColor = new Color(0.8f, 0.2f, 0.2f);
            pausingColor = new Color(0.8f, 0.8f, 0.2f);
            playingColor = new Color(0.2f, 0.2f, 0.8f);
            disabledColor = new Color(0.5f, 0.5f, 0.5f);
        }

        public Color GetColorForState(LoopOrbState state)
        {
            return state switch
            {
                LoopOrbState.ReadyToRecord => readyToRecordColor,
                LoopOrbState.Recording => recordingColor,
                LoopOrbState.Pausing => pausingColor,
                LoopOrbState.Playing => playingColor,
                LoopOrbState.Disabled => disabledColor,
                _ => Color.white
            };
        }
    }
} 