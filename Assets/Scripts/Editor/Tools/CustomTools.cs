using System.IO;
using UnityEditor;

public class CustomTools : EditorWindow
{
    [MenuItem("Custom Tools/Clean App Cache")]
    public static void CleanAppCache()
    {
        if (Directory.Exists(DataManager.BaseDataPath))
        {
            var allCacheFiles = Directory.GetFiles(DataManager.BaseDataPath);

            foreach (var file in allCacheFiles)
            {
                File.Delete(file);
            }
        }
    }
}