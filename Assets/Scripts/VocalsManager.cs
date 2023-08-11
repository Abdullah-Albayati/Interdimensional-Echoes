using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VocalsManager : MonoBehaviour
{
    public static VocalsManager Instance;
    public AudioSource source;
    public TMPro.TextMeshProUGUI subText;
    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        subText.text = string.Empty;
    }

    private void Update()
    {
        
    }
    public void Say(AudioClip clip)
    {
        if (source.isPlaying)
            source.Stop();

        source.PlayOneShot(clip);
    }

    public IEnumerator TheSequence(RecordingObject obj)
    {
        for (int i = 0; i < obj.sub.Count; i++)
        {
            if (source.isPlaying)
            {
                subText.text = obj.sub[i].sentenceTxt;

                yield return new WaitForSeconds(obj.sub[i].sentenceTime);
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
