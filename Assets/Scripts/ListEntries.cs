using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ListEntries : MonoBehaviour
{
    public GameObject entryPrefab; // Prefab for the entry game object
    public Transform entriesParent; // Parent transform for the instantiated entries
    [SerializeField] private RecordingPlayer recordingPlayer; // Reference to the RecordingPlayer script

    private List<GameObject> instantiatedEntries = new List<GameObject>();

    private void Start()
    {
        recordingPlayer = FindObjectOfType<RecordingPlayer>();
        ListRecordings();
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
                recordingButton.Initialize(recording.clip);
            }
            else
            {
                recordingButton = entryObject.AddComponent<RecordingButton>();
                recordingButton.Initialize(recording.clip);
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
