using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VocalsManager : MonoBehaviour
{
    public static VocalsManager Instance;
    public AudioSource source;
    public TMPro.TextMeshProUGUI subText;
    public GameObject vocalRecordingsPanel;
    private AudioClip currentAudioPlaying;
    public Coroutine currentSubtitleCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        subText.text = string.Empty;
    }
    public void Say(AudioClip clip)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }

        if (currentSubtitleCoroutine != null)
        {
            StopCoroutine(currentSubtitleCoroutine);
            currentSubtitleCoroutine = null;
        }

        currentAudioPlaying = clip;
        source.clip = clip;
        source.Play();
    }

    private void Update()
    {
        if(!vocalRecordingsPanel.activeInHierarchy)
        {
            source.Stop();
            if (currentSubtitleCoroutine != null)
            {
                StopCoroutine(currentSubtitleCoroutine);
                currentSubtitleCoroutine = null;
            }
            subText.text = string.Empty;
            currentAudioPlaying = null;
        }
    }

    public IEnumerator TheSequence(List<Subtitles> sub)
    {
        for (int i = 0; i < sub.Count; i++)
        {
            if (source.isPlaying)
            {
                subText.text = sub[i].sentenceTxt;

                Debug.Log(sub[i].sentenceTxt);
                yield return new WaitForSecondsRealtime(sub[i].sentenceTime);
                subText.text = string.Empty;

            }
            else
            {
                subText.text = string.Empty;
                StopAllCoroutines();
            }
                
        }
    }
}
