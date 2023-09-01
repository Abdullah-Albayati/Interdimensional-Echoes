using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingButton : MonoBehaviour
{
   [SerializeField] private AudioClip recording;
    public List<Subtitles> sub = new List<Subtitles>();
    [SerializeField] private GameObject newTag;
    public void Initialize(AudioClip recording,List<Subtitles> sub)
    {
        this.recording = recording;
        this.sub = sub;
        GetComponent<Button>().onClick.AddListener(PlayRecording);
    }
    public void PlayRecording()
    {
        VocalsManager.Instance.Say(recording);

        VocalsManager.Instance.currentSubtitleCoroutine = StartCoroutine(VocalsManager.Instance.TheSequence(sub));
        newTag.gameObject.SetActive(false);

    }
}
