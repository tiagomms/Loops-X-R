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
    //TODO: In factory, assign micController, wavFileManager, interfaceName
    public MicController micController; 
    public WavFileManager wavFileManager;
    public string interfaceName = "A";

    // TODO: expand Interface to hold multiple AudioClips and track which one on.
    
    private int takeNbr = 0;
    private float _lastVolume = 1f;


    public void StartRecording()
    {
        EnableSound(false);
        micController.WorkStart();
    }

    public void StopRecording()
    {
        AudioClip newClip = micController.WorkStop();
        newClip.name = WriteTakeName();
        AssignClipToAudioSource(newClip);
        EnableSound(true);
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

    private string WriteTakeName()
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

}
