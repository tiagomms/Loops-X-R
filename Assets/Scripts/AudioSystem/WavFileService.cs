using UnityEngine;
using Utils;
using System.IO;
using eToile;

namespace AudioSystem
{
    public class WavFileService : IWavFileService
    {
        public void SaveClip(AudioClip clip, string folderPath)
        {
            FileUtility.CreateDirectoryIfNotExists(folderPath);

            string fileName = AddWavExtension(clip.name);
            string fullPath = Path.Combine(Application.persistentDataPath, folderPath, fileName);

            byte[] wavData = OpenWavParser.AudioClipToByteArray(clip);
            FileUtility.WriteFile(fullPath, wavData);
        }

        public AudioClip LoadClip(string filePath)
        {
            byte[] data = FileUtility.ReadFile(AddWavExtension(filePath));
            return data != null ? OpenWavParser.ByteArrayToAudioClip(data) : null;
        }

        public void DeleteClip(string filePath)
        {
            FileUtility.DeleteFile(AddWavExtension(filePath));
        }

        private string AddWavExtension(string name)
        {
            return name.EndsWith(".wav") ? name : name + ".wav";
        }
    }
}
