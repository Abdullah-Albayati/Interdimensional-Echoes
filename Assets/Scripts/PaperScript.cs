using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PaperScript : MonoBehaviour
{
    public GameObject panelContainer;


    public TMP_Text textGo;
    public TMP_Text fakeTxt;

    public string paperText;
    public Color textColor;




    private void Awake()
    {
        panelContainer = GameObject.FindGameObjectWithTag("TextPanel");
        fakeTxt.color = textColor;
    }
    public void ReadPaper()
    {
        panelContainer.transform.SetChildrenActive(true);
        textGo = panelContainer.GetComponentInChildren<TMP_Text>();
        textGo.color = textColor;
        textGo.text = paperText;
    }
    public void LeavePaper()
    {
        panelContainer.transform.SetChildrenActive(false);
        textGo = null;
    }
}
