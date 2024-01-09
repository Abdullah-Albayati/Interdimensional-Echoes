using UnityEditor;
using UnityEngine;
using System.IO;
using NUnit.Framework.Constraints;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
public class ScreenshotToolWindow : EditorWindow
{
    private Camera selectedCamera;
    private RenderTexture previewTexture; // Keep a reference to the RenderTexture
    private Vector2 previewSize = new Vector2(750, 500); // Initial preview size
    private Vector2 scrollPosition;

    // Variables from CameraScreenShot script
    private string _path = "Assets/";
    private string fileName;
    private int height = 512;
    private int width = 512;
    private const string WidthKey = "ScreenshotTool_Width";
    private const string HeightKey = "ScreenshotTool_Height";
    private string cameraIdentifier;
    public ImageFormat imageFormat = ImageFormat.PNG;
    public enum ImageFormat
    {
        PNG,
        JPEG,
        EXR,
        TGA
        
    }

    private GUIStyle HeaderStyle;
    private GUIStyle smallHeader;
    private GUIStyle TitleStyle;

    [MenuItem("Tools/Screenshot Tool")]
    public static void ShowWindow()
    {
        GetWindow<ScreenshotToolWindow>("Screenshot Tool");
        
    }
    private void OnEnable()
    {
        HeaderStyle = new GUIStyle
        {
            fontSize = 50,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            
        };
        HeaderStyle.normal.textColor = Color.white;

        smallHeader = new GUIStyle
        {
            fontSize = 30,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        smallHeader.normal.textColor = Color.white;

        TitleStyle = new GUIStyle
        {
            fontSize = 30,
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold
        };
        TitleStyle.normal.textColor = Color.white;

        width = EditorPrefs.GetInt(WidthKey, 512);
        height = EditorPrefs.GetInt(HeightKey, 512);
        _path = LoadPath();
       
        cameraIdentifier = LoadCamera();
        UpdateSelectedCamera();
       imageFormat = LoadImageFormat();
       fileName = LoadFileName();

    }
    private void OnDisable()
    {
        EditorPrefs.SetInt(WidthKey, width);
        EditorPrefs.SetInt(HeightKey, height);
        SaveImageFormat(imageFormat);
        SavePath(_path);
       SaveCamera(selectedCamera);
       SaveFileName(fileName);
       
    }
    private void SaveFileName(string fileName){
    EditorPrefs.SetString("fileName",fileName);
    }
    private string LoadFileName(){
      return EditorPrefs.GetString("fileName", ""); 
    }
  private void SaveCamera(Camera camera){
     if(camera != null){
        EditorPrefs.SetString("CameraIdentifier", camera.GetInstanceID().ToString());
     }
     else{
        EditorPrefs.DeleteKey("CameraIdentifier");
     }
  }

  private string LoadCamera(){
    return EditorPrefs.GetString("CameraIdentifier","");
  }
    private void SavePath(string path)
    {
        EditorPrefs.SetString("SavePath", path);
    }
   
    private string LoadPath()
    {
        return EditorPrefs.GetString("SavePath", "Assets/");
    }
    private void SaveImageFormat(ImageFormat format)
    {
        int formatAsInt = (int)format;
        EditorPrefs.SetInt("ImageFormat", formatAsInt);
    }
   private void UpdateSelectedCamera()
{
    if (!string.IsNullOrEmpty(cameraIdentifier))
    {
        int instanceID;
        if (int.TryParse(cameraIdentifier, out instanceID))
        {
            selectedCamera = EditorUtility.InstanceIDToObject(instanceID) as Camera;
        }
    }
}
    

    // Load ImageFormat from EditorPrefs
    private ImageFormat LoadImageFormat()
    {
        int formatAsInt = EditorPrefs.GetInt("ImageFormat", 0);
        return (ImageFormat)formatAsInt;
    }
    private void OnGUI()
    {
         scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label("Settings", HeaderStyle);
        GUILayout.Space(10);
        // Display a dropdown to choose the camera
        selectedCamera = EditorGUILayout.ObjectField("Select Camera", selectedCamera, typeof(Camera), true) as Camera;
        
        GUILayout.BeginHorizontal();
        _path = EditorGUILayout.TextField("Camera Path:", _path);
        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            _path = EditorUtility.OpenFolderPanel("Select Save Path:", _path, "");
        }
        GUILayout.EndHorizontal();
        fileName = EditorGUILayout.TextField("File Name:", fileName);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image Format:");
        GUILayout.FlexibleSpace();
        imageFormat = (ImageFormat)EditorGUILayout.EnumPopup(imageFormat);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("Resolution", smallHeader);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Width:", TitleStyle);
        GUILayout.FlexibleSpace();
        GUILayout.Label(width.ToString(), TitleStyle, GUILayout.Width(150));
        GUILayout.EndHorizontal();
        width = EditorGUILayout.IntSlider(width, 768, 2048);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Height:", TitleStyle);
        GUILayout.FlexibleSpace();
        GUILayout.Label(height.ToString(), TitleStyle, GUILayout.Width(150));
        GUILayout.EndHorizontal();
        height = EditorGUILayout.IntSlider(height, 1024, 2048);


        // Update the previewTexture if the selectedCamera changes
        if (selectedCamera != null && (previewTexture == null || previewTexture.width != (int)previewSize.x || previewTexture.height != (int)previewSize.y))
        {
            previewTexture = RenderPreview(selectedCamera);
            previewSize.x = width;
            previewSize.y = height;
        }
        if (selectedCamera != null)
        {
            EditorGUILayout.HelpBox("Save the scene to update the preview of the camera, or press Refresh Preview.", MessageType.Info);
        }
            

        // Display a live preview of the selected camera's view
        if (selectedCamera != null && previewTexture != null)
        {
            GUILayout.Label("Camera Preview:");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawTexture(previewTexture, GUILayout.Width(previewSize.x), GUILayout.Height(previewSize.y));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        // Add other settings and buttons for capturing screenshots
        GUILayout.Space(10);

        // Add a refresh button to manually update the preview
        if(selectedCamera != null && previewTexture != null){
         if (GUILayout.Button("Refresh Preview"))
        {
            if (selectedCamera != null)
            {
                previewTexture = RenderPreview(selectedCamera);
            previewSize.x = width;
            previewSize.y = height;
            }
        }
        }
        

        GUILayout.Space(10);

        if(selectedCamera!= null)
        {
            if (GUILayout.Button("Screen Shot"))
            {
               if(!string.IsNullOrEmpty(fileName)){

                TakeScreenShot(_path,imageFormat);
               }
               else{
                Debug.LogError("Please choose a name for the image");
               }
              
            }
        }
         EditorGUILayout.EndScrollView();
        Repaint();
    }

    private RenderTexture RenderPreview(Camera camera)
    {
        int width = Mathf.RoundToInt(previewSize.x);
        int height = Mathf.RoundToInt(previewSize.y);

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        camera.targetTexture = renderTexture;
        camera.Render();
        camera.targetTexture = null;

        return renderTexture;
    }

    private void DrawTexture(Texture texture, params GUILayoutOption[] options)
    {
        Rect rect = GUILayoutUtility.GetRect(0, 0, options);
        GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit);
    }

    public void TakeScreenShot(string fullPath, ImageFormat format)
    {
        
        float aspectRatio = (float)width / height;

        RenderTexture rt = new RenderTexture(width, Mathf.RoundToInt(width / aspectRatio), 24);
        selectedCamera.targetTexture = rt;

        Texture2D screenShot = new Texture2D(width, Mathf.RoundToInt(width / aspectRatio), TextureFormat.RGBA32, false);
        selectedCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, Mathf.RoundToInt(width / aspectRatio)), 0, 0);
        selectedCamera.targetTexture = null;
        RenderTexture.active = null;

        Destroy(rt);

        byte[] bytes;
        string extension;

        switch (imageFormat)
        {
            case ImageFormat.PNG:
                bytes = screenShot.EncodeToPNG();
                extension = "png";
                break;
            case ImageFormat.JPEG:
                bytes = screenShot.EncodeToJPG();
                extension = "jpg";
                break;
            case ImageFormat.EXR:
                bytes = screenShot.EncodeToEXR();
                extension = "exr";
                break;
            case ImageFormat.TGA:
                bytes = screenShot.EncodeToTGA();
                extension = "tga";
                break;
            default:
                bytes = screenShot.EncodeToPNG();
                extension = "png";
                break;
        }

int count = 0;
string baseFileName = fileName;
string absolutePath = Path.Combine(_path, $"{baseFileName}.{extension}");

while (File.Exists(absolutePath))
{
    if (count > 0)
    {
        baseFileName = $"{fileName}_{count}";
    }

    absolutePath = Path.Combine(_path, $"{baseFileName}.{extension}");
    count++;
}

          File.WriteAllBytes(absolutePath, bytes);

        Debug.Log($"Screenshot taken by {selectedCamera.name} have been saved to: {fullPath}_{fileName}.{extension} with resolution {width}x{height} and format {imageFormat}");
    }
}
