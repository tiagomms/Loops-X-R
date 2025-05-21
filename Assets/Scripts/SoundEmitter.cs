using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Events;

public class SoundEmitter : MonoBehaviour
{    
    [SerializeField] private float impulseThreshold = 1f;

    // TODO : check when it is being grabbed (do not activate on grab)
    public UnityEvent onEmitSound;
    
    private float _disableCollisionTimer = 0f;

    // To reset so that it does not invoke too many times in a row.
    private void Update()
    {
        if (_disableCollisionTimer < 2f)
            _disableCollisionTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        // disable collisions at start
        if (_disableCollisionTimer < 2f) return;
        
        // if hit by hand continue
        Hand hand = GetComponentInParent<Hand>();
        if (!hand) return;
        float magnitude = other.impulse.magnitude;
        if (magnitude > impulseThreshold)
        {
            XRDebugLogViewer.Log($"Sound emitted from {gameObject.name} with impulse {magnitude:F2}");
            onEmitSound.Invoke();
            _disableCollisionTimer = 0f; // disable collisions for a while
        }
    }

}
