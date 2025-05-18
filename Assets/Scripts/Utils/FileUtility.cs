using System.IO;
using UnityEngine;

namespace Utilities
{
    public static class FileUtility
    {
        public static string GetFullPath(string relativePath)
        {
            return Path.Combine(Application.persistentDataPath, relativePath);
        }

        public static void CreateDirectoryIfNotExists(string relativePath)
        {
            string fullPath = GetFullPath(relativePath);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                Debug.Log($"[FileUtility] Created directory: {fullPath}");
            }
        }

        public static void DeleteDirectory(string relativePath)
        {
            string fullPath = GetFullPath(relativePath);
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                Debug.Log($"[FileUtility] Deleted directory: {fullPath}");
            }
        }

        public static void WriteFile(string fullPath, byte[] data)
        {
            File.WriteAllBytes(fullPath, data);
            Debug.Log($"[FileUtility] Wrote file: {fullPath}");
        }

        public static byte[] ReadFile(string fullPath)
        {
            return File.Exists(fullPath) ? File.ReadAllBytes(fullPath) : null;
        }

        public static void DeleteFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                Debug.Log($"[FileUtility] Deleted file: {fullPath}");
            }
        }
    }
}
