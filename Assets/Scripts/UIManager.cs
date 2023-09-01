using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("UIManager");
                instance = obj.AddComponent<UIManager>();
            }

            return instance;
        }
    }

    public List<GameObject> externalUI = new List<GameObject>();
    public List<GameObject> playerUi = new List<GameObject>();

    public enum UIType
    {
        VoiceRecordings,
        Journal,
        Paper
    }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        foreach (GameObject obj in externalUI)
        {
            obj.SetActive(false);
        }
    }

    public void TogglePlayerUI(bool toggle)
    {
        foreach (GameObject ui in playerUi)
        {
            ui.SetActive(toggle);
        }
    }

    public void ToggleGameUI(UIType uiType)
    {
        for (int i = 0; i < externalUI.Count; i++)
        {
            if (i != (int)uiType)
            {
                externalUI[i].SetActive(false);
               
            }
            else
            {
                bool isActive = externalUI[i].activeSelf;
                externalUI[i].SetActive(!isActive);
                StartCoroutine(FadeIn(externalUI[i].gameObject.GetComponent<CanvasGroup>()));
                if (!isActive)
                {
                    GameManager.instance.PauseGame();
                    SetCursorState(CursorLockMode.Confined);
                    TogglePlayerUI(false);

                }
                else
                {
                    GameManager.instance.ResumeGame();
                    TogglePlayerUI(true);
                    SetCursorState(CursorLockMode.Locked);
                }
            }
        }
    }

    private void SetCursorState(CursorLockMode state)
    {
        Cursor.lockState = state;
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        float duration = 0.2f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0,1, elapsed / duration);
            group.alpha = alpha;
            yield return null;
        }
        group.alpha = 1.0f;
    }


    
}
