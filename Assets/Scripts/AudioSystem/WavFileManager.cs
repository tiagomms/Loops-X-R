using UnityEngine;
using Utils;
using System.IO;

namespace AudioSystem
{
    public class WavFileManager : MonoBehaviour
    {
        [SerializeField] private string projectName = "DefaultProject";

        private IWavFileService _wavService;
        private string _projectFolder;

        private void Awake()
        {
            _wavService = new WavFileService(); // In future, swap to DI if needed
            _projectFolder = Path.Combine("Recordings", projectName);
            FileUtility.CreateDirectoryIfNotExists(_projectFolder);
        }

        public void SaveAudio(AudioClip clip)
        {
            _wavService.SaveClip(clip, _projectFolder);
        }

        public AudioClip LoadAudio(string clipName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, _projectFolder, clipName);
            return _wavService.LoadClip(filePath);
        }

        public void DeleteAudio(string clipName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, _projectFolder, clipName);
            _wavService.DeleteClip(filePath);
        }

        public void ClearProjectFolder()
        {
            FileUtility.DeleteDirectory(_projectFolder);
        }

        private void OnDestroy()
        {
            ClearProjectFolder(); // ⚠️ for prototyping only
        }
    }
}
