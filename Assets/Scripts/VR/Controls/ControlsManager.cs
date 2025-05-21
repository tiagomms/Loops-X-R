using UnityEngine;
using VR.Controls;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance { get; private set; }
    [SerializeField] private MicrogesturesController microgestures;

    [SerializeField] private HandPoseController playPoseController;
    [SerializeField] private HandPoseController stopPoseController;
    public MicrogesturesController Microgestures => microgestures;
    public HandPoseController PlayPoseController => playPoseController;

    public HandPoseController StopPoseController => stopPoseController;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
