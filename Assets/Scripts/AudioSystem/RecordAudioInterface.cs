using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AudioSystem;
using Meta.WitAi.Lib;
using UnityEngine;

public class RecordAudioInterface : MonoBehaviour
{
    public AudioSource audioSource;
    //TODO: In factory, assign micController, wavFileManager, interfaceName (later)
    private MicController micController; 
    private WavFileManager wavFileManager;
    public string interfaceName = "";  // Start with empty name

    [SerializeField] private float volumeNudgeFactor = 0.2f;

    // TODO: expand Interface to hold multiple AudioClips and track which one on.
    
    private int takeNbr = 0;
    private float _lastVolume = 1f;
    private bool isRecording = false;

    private void Start()
    {
        micController = MicController.Instance;
        wavFileManager = WavFileManager.Instance;
        
        if (micController == null || wavFileManager == null)
        {
            Debug.LogError("MicController or WavFileManager singleton not found in scene");
            return;
        }
    }

    public void SetInterfaceName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            XRDebugLogViewer.LogWarning($"Attempted to set empty interface name on {gameObject.name}");
            return;
        }
        interfaceName = name;
        //XRDebugLogViewer.Log($"Interface name set to {name} on {gameObject.name}");
    }

    public void StartRecording()
    {
        EnableSound(false);
        micController.WorkStart();
        isRecording = true;
    }

    public void StopRecording()
    {
        AudioClip newClip = micController.WorkStop();
        newClip.name = WriteTakeName();
        AssignClipToAudioSource(newClip);
        EnableSound(true);
        isRecording = false;
    }
    
    public void PlayAudioClip()
    {
        StopAudioClip();
        audioSource.Play();
    }

    public void StopAudioClip()
    {
        audioSource.Stop();
    }

    public string WriteTakeName()
    {
        string clipName = String.Format("{0}-{1:00}", interfaceName, takeNbr);
        takeNbr++;
        return clipName;
    }

    public void SaveRecording()
    {
        wavFileManager.SaveAudio(audioSource.clip);
    }

    public void DeleteRecording()
    {
        wavFileManager.DeleteAudio(audioSource.clip.name);
    }

    public void LoadRecordingIntoAudioSource(string fileName)
    {
        AssignClipToAudioSource(wavFileManager.LoadAudio(fileName));
    }

    public void EnableSound(bool isSoundOn)
    {
        if (isSoundOn)
        {
            if (_lastVolume > 0)
            {
                audioSource.volume = _lastVolume;
            }
            else
            {
                audioSource.volume = 1f;
            }
        }
        else
        {
            _lastVolume = audioSource.volume;
            audioSource.volume = 0f;
        }
    }

    public void AssignClipToAudioSource(AudioClip recordedClip)
    {
        StopAudioClip();
        audioSource.clip = recordedClip;
    }

    public void SetAudioSourceLooping(bool isLooping)
    {
        audioSource.loop = isLooping;
    }

    public bool IsAudioLooping()
    {
        return audioSource.loop;
    }

    public void NudgeVolume(bool isGoingUp)
    {
        float multFactor = isGoingUp ? 1 : -1;
        _lastVolume = audioSource.volume;
        audioSource.volume = Mathf.Clamp01(_lastVolume + multFactor * volumeNudgeFactor);
    }
}
