using UnityEngine;
using UnityEngine.UI;

public class RecordingButton : MonoBehaviour
{
    private AudioClip recording;

    public void Initialize(AudioClip recording)
    {
        this.recording = recording;
        GetComponent<Button>().onClick.AddListener(PlayRecording);
    }

    public void PlayRecording()
    {
        // Assuming you have an AudioSource component on the player or another object,
        // you can play the recording using the following code:
        AudioSource audioSource = FindObjectOfType<AudioSource>();
        audioSource.clip = recording;
        audioSource.Play();
    }
}
