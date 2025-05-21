using UnityEngine;
using TMPro;

namespace XRLoopPedal.AudioSystem
{
    /// <summary>
    /// Controls the visibility and content of the orb identifier UI
    /// </summary>
    public class OrbIdentifierUIController : MonoBehaviour
    {
        [SerializeField] private Canvas orbIdentifierUI;
        [SerializeField] private TextMeshProUGUI identifierText;

        private void Awake()
        {
            ValidateDependencies();
        }

        private void ValidateDependencies()
        {
            if (orbIdentifierUI == null)
            {
                Debug.LogError($"[{nameof(OrbIdentifierUIController)}] Canvas reference is missing on {gameObject.name}");
                return;
            }

            if (identifierText == null)
            {
                identifierText = orbIdentifierUI.GetComponentInChildren<TextMeshProUGUI>();
                if (identifierText == null)
                {
                    Debug.LogError($"[{nameof(OrbIdentifierUIController)}] TextMeshProUGUI not found in Canvas");
                    return;
                }
            }
        }

        /// <summary>
        /// Updates the UI visibility based on the current state
        /// </summary>
        /// <param name="newState">The current state of the orb</param>
        public void UpdateVisibility(LoopOrbState newState)
        {
            bool isNotInRecordingStage = newState != LoopOrbState.ReadyToRecord && newState != LoopOrbState.Recording;
            orbIdentifierUI.gameObject.SetActive(isNotInRecordingStage);
        }

        /// <summary>
        /// Updates the identifier text with the new take name
        /// </summary>
        /// <param name="takeName">The name of the current take</param>
        public void UpdateIdentifierText(string takeName)
        {
            identifierText.text = takeName;
        }
    }
} 