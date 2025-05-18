using UnityEngine;

namespace AudioSystem
{
    public interface IWavFileService
    {
        void SaveClip(AudioClip clip, string folderPath);
        AudioClip LoadClip(string filePath);
        void DeleteClip(string filePath);
    }
}
