using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[System.Serializable]
public class SavedRecording
{
    public AudioClip clip;
    public string name;
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
        if (currentItem.IsPickedUp)
        {
            //Change this code block position later
            if (Input.GetButtonDown(GameManager.instance.openVoiceRecordingsButton))
            {
                UIManager.Instance.OpenVoiceRecordingsMenu();   
            }
            
        }


        if (!hasPlayed && currentItem.IsPickedUp)
        {
            PlayRecording();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            StopRecording();
        }
    }

    private void PlayRecording()
    {
        recordingObject = transform.parent.GetComponentInChildren<RecordingObject>();

        if (recordingObject != null)
        {
            AudioClip recording = recordingObject.GetRecording();
            if (recording != null)
            {
                VocalsManager.Instance.Say(recording);
                StartCoroutine(VocalsManager.Instance.TheSequence(recordingObject));

                StartCoroutine(WaitForClipToEnd(recording));
                hasPlayed = true;
            }
        }
    }

    private void StopRecording()
    {
        if (VocalsManager.Instance.source.isPlaying)
        {
            VocalsManager.Instance.source.Stop();
            VocalsManager.Instance.StopCoroutine(VocalsManager.Instance.TheSequence(recordingObject));

            StartCoroutine(WaitForClipToEnd(recordingObject.GetRecording()));
            StopCoroutine(VocalsManager.Instance.TheSequence(recordingObject));
            savedRecordings.Add(new SavedRecording { clip = VocalsManager.Instance.source.clip, name = recordingObject.recordingName });
            Destroy(recordingObject.gameObject);
            hasPlayed = false;
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

    private System.Collections.IEnumerator WaitForClipToEnd(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);

        if (VocalsManager.Instance.source.isPlaying)
        {
            VocalsManager.Instance.source.Stop();
        }

        savedRecordings.Add(new SavedRecording { clip = clip, name = recordingObject.recordingName });
        vocalRecordingsCanvas.ListRecordings();
        Destroy(recordingObject.gameObject);
        hasPlayed = false;
    }
}
