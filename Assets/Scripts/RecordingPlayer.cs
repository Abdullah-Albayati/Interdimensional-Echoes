using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[System.Serializable]
public class SavedRecording
{
    public AudioClip clip;
    public string name;
    public List<Subtitles> sub = new List<Subtitles>();
}

public class RecordingPlayer : MonoBehaviour,IGameObserver
{
    public List<SavedRecording> savedRecordings = new List<SavedRecording>();
    private bool hasPlayed = false;
    PickableObject currentItem;
    private RecordingObject recordingObject;
    public ListEntries vocalRecordingsCanvas;
    private void Start()
    {
        currentItem = GetComponent<PickableObject>();
        vocalRecordingsCanvas = FindObjectOfType<ListEntries>();
        GameManager.instance.AddObserver(this);
    }

    private void Update()
    {
        if(!hasPlayed && currentItem.IsPickedUp)
        {
            SaveRecording();
        }
    }

    private void SaveRecording()
    {
        recordingObject = transform.parent.GetComponentInChildren<RecordingObject>();

        if (recordingObject != null)
        {
            AudioClip recording = recordingObject.GetRecording();
            if (recording != null)
            {

                savedRecordings.Add(new SavedRecording { clip = recording, name = recordingObject.recordingName,sub = recordingObject.sub});
                vocalRecordingsCanvas.ListRecordings();
                Destroy(recordingObject.gameObject);
                hasPlayed = false;
            }
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from GameManager's observer list
        GameManager.instance.RemoveObserver(this);
    }
    public void OnGamePaused()
    {

    }
    public void OnGameResumed()
    {

    }

}
