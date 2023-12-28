using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HintBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hintBarText;
    [SerializeField] Image hintBarIcon;
    [SerializeField] Animator anim;
    private void Awake()
    {
        hintBarText = GetComponentInChildren<TextMeshProUGUI>();
        hintBarIcon = GetComponentInChildren<Image>();
    }

    public void Show()
    {
        anim.SetBool("Show", true);
    }
    public void Hide()
    {
        anim.SetBool("Show", false);
    }
    public void SetIcon(Image icon)
    {
        hintBarIcon = icon;
    }
    public void SetText(string text)
    {
        hintBarText.text = text;
    }
}
