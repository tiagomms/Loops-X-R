using UnityEngine;
using UnityEngine.Events;
using System;

namespace Core
{
    /// <summary>
    /// Manages global application states and provides events for state changes.
    /// States: Default, Recording, Spawning, Select, Edit, Move, Play
    /// </summary>
    [Flags]
    public enum AppState
    {
        None = 0,
        Default = 1 << 0,    // 1
        Spawning = 1 << 1,   // 2
        Recording = 1 << 2,  // 4
        Select = 1 << 3,     // 8
        Edit = 1 << 4,       // 16
        Move = 1 << 5,       // 32
        Play = 1 << 6        // 64
    }
    public class GlobalEventManager : MonoBehaviour
    {
        #region Singleton
        private static GlobalEventManager _instance;
        public static GlobalEventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GlobalEventManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GlobalEventManager");
                        _instance = go.AddComponent<GlobalEventManager>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region States

        [Header("Current State")]
        [SerializeField] private AppState _currentState = AppState.Default;
        [SerializeField] private AppState _previousState = AppState.Default;
        public AppState CurrentState => _currentState;
        public AppState PreviousState => _previousState;

        // Event triggered when state changes
        [Serializable] public class StateChangeEvent : UnityEvent<AppState, AppState> { }
        [Header("State Events")]
        public StateChangeEvent OnStateChanged;

        // Individual state events
        [Header("State-Specific Events")]
        public UnityEvent OnRecordingStarted;
        public UnityEvent OnRecordingStopped;
        public UnityEvent OnSpawningStarted;
        public UnityEvent OnSpawningStopped;
        public UnityEvent OnSelectStarted;
        public UnityEvent OnSelectStopped;
        public UnityEvent OnEditStarted;
        public UnityEvent OnEditStopped;
        public UnityEvent OnMoveStarted;
        public UnityEvent OnMoveStopped;
        public UnityEvent OnPlayStarted;
        public UnityEvent OnPlayStopped;
        #endregion

        #region Public Methods
        /// <summary>
        /// Changes the current application state and triggers appropriate events
        /// </summary>
        /// <param name="newState">The state to change to</param>
        public void ChangeState(AppState newState)
        {
            if (_currentState == newState) return;

            _previousState = _currentState;
            _currentState = newState;

            // Trigger general state change event
            OnStateChanged?.Invoke(_previousState, _currentState);

            // Trigger state-specific events
            switch (_previousState)
            {
                case AppState.Recording:
                    OnRecordingStopped?.Invoke();
                    break;
                case AppState.Spawning:
                    OnSpawningStopped?.Invoke();
                    break;
                case AppState.Select:
                    OnSelectStopped?.Invoke();
                    break;
                case AppState.Edit:
                    OnEditStopped?.Invoke();
                    break;
                case AppState.Move:
                    OnMoveStopped?.Invoke();
                    break;
                case AppState.Play:
                    OnPlayStopped?.Invoke();
                    break;
            }

            switch (_currentState)
            {
                case AppState.Recording:
                    OnRecordingStarted?.Invoke();
                    break;
                case AppState.Spawning:
                    OnSpawningStarted?.Invoke();
                    break;
                case AppState.Select:
                    OnSelectStarted?.Invoke();
                    break;
                case AppState.Edit:
                    OnEditStarted?.Invoke();
                    break;
                case AppState.Move:
                    OnMoveStarted?.Invoke();
                    break;
                case AppState.Play:
                    OnPlayStarted?.Invoke();
                    break;
            }
        }

        /// <summary>
        /// Returns to the default state
        /// </summary>
        public void ResetToDefault()
        {
            ChangeState(AppState.Default);
        }
        #endregion
    }
}