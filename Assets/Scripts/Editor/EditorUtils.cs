using System.IO;
using UnityEditor;
using UnityEngine;


public class EditorUtils : MonoBehaviour
{
    [MenuItem("Utils/Open Persistent Data Path")]
    public static void OpenFolder()
    {
        string path = Application.persistentDataPath;

        if (!Directory.Exists(path))
        {
            Debug.LogWarning($"Persistent Data Path does not exist yet:\n{path}");
            Directory.CreateDirectory(path);
        }

        EditorUtility.RevealInFinder(path);
    }
}
