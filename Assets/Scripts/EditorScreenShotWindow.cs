#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class EditorScreenShotWindow : EditorWindow
{
    string fileName;

    void OnGUI()
    {
        GUILayout.Label("Save Screenshot", EditorStyles.boldLabel);

        fileName = EditorGUILayout.TextField("File Name:", fileName);

        if (GUILayout.Button("Capture Screenshot"))
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string fullPath = "Assets/" + fileName + ".png";
                CameraScreenShot cameraScreenShot = FindObjectOfType<CameraScreenShot>();
                cameraScreenShot.TakeScreenShot(fullPath);
            }
            else
            {
                Debug.LogWarning("Please enter a file name.");
            }
        }
    }
}
#endif
