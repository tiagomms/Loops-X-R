using UnityEngine;
using DG.Tweening;

namespace XRLoopPedal.AudioSystem
{
    /// <summary>
    /// Handles all visual effects for the orb, including color transitions and alpha based on volume
    /// </summary>
    public class OrbParticleController : MonoBehaviour
    {
        [Header("Particle Settings")]
        [SerializeField] private ParticleSystem orbParticles;
        
        [Header("Alpha Settings")]
        [SerializeField, Range(0, 255)] private int alphaMax = 20;
        [SerializeField, Range(0, 255)] private int alphaMin = 2;
        [SerializeField] private float colorTransitionDuration = 0.1f;
        [SerializeField] private float volumeTransitionDuration = 0.1f;
        [SerializeField] private bool useColorTransition = false;
        [SerializeField] private bool useVolumeTransition = false;

        // Private fields
        private Color currentColor;
        private Tweener colorTween;
        private Tweener volumeTween;
        private float previousVolume;
        private bool isInitialized;

        private float _alphaMax01f;
        private float _alphaMin01f;

        public ParticleSystem OrbParticles => orbParticles;

        #region Unity Lifecycle

        private void Awake()
        {
            ValidateDependencies();
            InitializeComponents();
        }

        private void OnDestroy()
        {
            colorTween?.Kill();
            volumeTween?.Kill();
        }

        #endregion

        #region Initialization

        private void ValidateDependencies()
        {
            if (orbParticles == null)
            {
                orbParticles = GetComponentInChildren<ParticleSystem>();
                if (orbParticles == null)
                {
                    Debug.LogError($"[{nameof(OrbParticleController)}] ParticleSystem not found on {gameObject.name} or its children");
                    return;
                }
            }

            isInitialized = true;
        }

        private void InitializeComponents()
        {
            if (!isInitialized) return;

            _alphaMax01f = alphaMax / 255f;
            _alphaMin01f = alphaMin / 255f;
            currentColor = LoopOrbStateColors.Instance.GetColorForState(LoopOrbState.ReadyToRecord);
            UpdateParticleColor(currentColor);
        }

        #endregion

        #region Public Methods

        public void UpdateVolume(float targetVolume)
        {
            if (!isInitialized) return;
            if (Mathf.Approximately(targetVolume, previousVolume)) return;

            // Kill existing volume tween if any
            volumeTween?.Kill();

            if (useVolumeTransition)
            {
                // Create new volume tween
                volumeTween = DOTween.To(
                    () => previousVolume,
                    x => {
                        float alpha = Mathf.Lerp(_alphaMin01f, _alphaMax01f, x);
                        var main = orbParticles.main;
                        Color newColor = main.startColor.color;
                        newColor.a = alpha;
                        main.startColor = newColor;
                    },
                    targetVolume,
                    volumeTransitionDuration
                ).SetEase(Ease.OutQuad);
            }
            else
            {
                // Set volume immediately
                float alpha = Mathf.Lerp(_alphaMin01f, _alphaMax01f, targetVolume);
                var main = orbParticles.main;
                Color newColor = main.startColor.color;
                newColor.a = alpha;
                main.startColor = newColor;
            }

            previousVolume = targetVolume;
        }

        public void UpdateState(LoopOrbState newState)
        {
            if (!isInitialized) return;

            Color stateColor = LoopOrbStateColors.Instance.GetColorForState(newState);
            UpdateParticleColor(stateColor);
        }

        #endregion

        #region Private Methods

        private void UpdateParticleColor(Color newColor)
        {
            if (!isInitialized) return;

            // Kill any existing tween
            colorTween?.Kill();

            // Keep the current alpha
            var main = orbParticles.main;
            float currentAlpha = main.startColor.color.a;

            // Create new color with current alpha
            Color targetColor = newColor;
            targetColor.a = currentAlpha;

            if (useColorTransition)
            {
                // Tween to new color
                colorTween = DOTween.To(
                    () => main.startColor.color,
                    x => main.startColor = x,
                    targetColor,
                    colorTransitionDuration
                ).SetEase(Ease.OutQuad);
            }
            else
            {
                // Set color immediately
                main.startColor = targetColor;
            }
        }

        #endregion
    }
} 