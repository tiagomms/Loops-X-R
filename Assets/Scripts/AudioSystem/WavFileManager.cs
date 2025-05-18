using UnityEngine;
using UnityEngine.UI;
using System.IO;
using eToile;

namespace AudioSystem
{
    public class WavFileManager : MonoBehaviour
    {
        private string _directoryPath;

        // TODO: make directory per project
        [SerializeField] private string directoryName = "Recordings";

        private void Awake()
        {
            // Set up the directory path using persistentDataPath
            _directoryPath = Path.Combine(Application.persistentDataPath, directoryName);

            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
                Debug.Log($"Created recordings directory at: {_directoryPath}");
            }

        }

        public void SaveClipAsWavFile(AudioClip clip)
        {
            string wavName = AddWavFileExtension(clip.name);

            byte[] wavFile = OpenWavParser.AudioClipToByteArray(clip);
            File.WriteAllBytes(Path.Combine(_directoryPath, wavName), wavFile);
            Debug.Log($"File {wavName} created from clip {clip.name}");
        }

        private string AddWavFileExtension(string fileName)
        {
            if (!fileName.EndsWith(".wav"))
            {
                fileName += ".wav";
            }
            return fileName;
        }

        // File control:
        public void DeleteWavFile(string wavName)
        {
            File.Delete(Path.Combine(_directoryPath, AddWavFileExtension(wavName)));
            Debug.Log($"File {wavName} deleted");
        }

        public AudioClip LoadWavFile(string wavName)
        {
            string filePath = Path.Combine(_directoryPath, AddWavFileExtension(wavName));
            if (File.Exists(filePath))
            {
                byte[] wavFile = File.ReadAllBytes(filePath);
                Debug.Log($"File {wavName} loaded");
                return OpenWavParser.ByteArrayToAudioClip(wavFile);
            }

            Debug.Log($"LoadWavFile not possible - File {wavName} not found");
            return null;
        }

        // TODO: for prototype only (to not overflow) - in the future create a management system
        void OnDestroy()
        {
            if (!Directory.Exists(_directoryPath)) return;
            Directory.Delete(_directoryPath);
        }
    }
}
