#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraScreenShot))]
public class CameraScreenShotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraScreenShot cameraScreenShot = (CameraScreenShot)target;

        if (GUILayout.Button("Capture Screenshot"))
        {
            if (!string.IsNullOrEmpty(cameraScreenShot.fileName))
            {
                string fullPath = "Assets/" + cameraScreenShot.fileName + ".png";
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
