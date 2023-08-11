using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Subtitles
{
    [TextArea(5, 50)]
    public string sentenceTxt;
    public float sentenceTime;
}
public class RecordingObject : MonoBehaviour
{
    public List<Subtitles> sub = new List<Subtitles>();

    [SerializeField]
    private AudioClip recording;
    public string recordingName;
    PickableObject currentItem;
    private void Start()
    {
        currentItem = GetComponent<PickableObject>();
    }
    private void Update()
    {
        
    }
    public AudioClip GetRecording()
    {
        return recording;
    }
}
