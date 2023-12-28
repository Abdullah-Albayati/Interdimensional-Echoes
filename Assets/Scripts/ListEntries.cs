using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ListEntries : MonoBehaviour
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform entriesParent; 
    [SerializeField] private RecordingPlayer recordingPlayer; 

    private PickableObject recordingObj;
    private List<GameObject> instantiatedEntries = new List<GameObject>();

    private void Awake()
    {
        if (!recordingPlayer)
            recordingPlayer = FindObjectOfType<RecordingPlayer>();

        if (recordingPlayer)
            recordingObj = recordingPlayer.GetComponent<PickableObject>();
    }

    private void Update()
    {
        if (recordingObj && recordingObj.isPickedUp && Input.GetButtonDown(GameManager.instance.openVoiceRecordingsButton))
        {
            UIManager.Instance.ToggleGameUI(UIManager.UIType.VoiceRecordings);
        }
    }
    public void ListRecordings()
    {
        // Clear existing entries
        foreach (GameObject entry in instantiatedEntries)
        {
            Destroy(entry);
        }
        instantiatedEntries.Clear();

        // Instantiate entries for each recording
        foreach (SavedRecording recording in recordingPlayer.savedRecordings)
        {
            GameObject entryObject = Instantiate(entryPrefab, entriesParent);

            // Add RecordingButton component and associate the audio clip
            RecordingButton recordingButton = entryObject.GetComponent<RecordingButton>();
            if (recordingButton != null)
            {
                recordingButton.Initialize(recording.clip, recording.sub);
            }
            else
            {
                recordingButton = entryObject.AddComponent<RecordingButton>();
                recordingButton.Initialize(recording.clip,recording.sub);
            }

            TextMeshProUGUI entryText = entryObject.GetComponentInChildren<TextMeshProUGUI>();
            if (entryText != null)
            {
                entryText.text = recording.name;
            }

            instantiatedEntries.Add(entryObject);
        }
    }

}
