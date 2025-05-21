using UnityEngine;
using VR.Controls;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance { get; private set; }
    [SerializeField] private MicrogesturesController microgestures;
    public MicrogesturesController Microgestures => microgestures;

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
