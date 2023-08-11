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

    public GameObject voiceRecordingsMenuUI, journalMenuUI,paperMenuUI;
    

    private void Start()
    {
        // Close both menus when the game starts
        CloseVoiceRecordingsMenu();
        CloseJournalMenu();
        ClosePaperMenu();
    }

    private void Update()
    {
        if (voiceRecordingsMenuUI.activeInHierarchy || journalMenuUI.activeInHierarchy || paperMenuUI.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenVoiceRecordingsMenu()
    {
        if (!voiceRecordingsMenuUI.activeSelf)
        {
            voiceRecordingsMenuUI.SetActive(true);
            journalMenuUI.SetActive(false);
            paperMenuUI.SetActive(false);
            GameManager.instance.PauseGame();
        }
        else
            CloseVoiceRecordingsMenu();
    }

    public void CloseVoiceRecordingsMenu()
    {
        if (voiceRecordingsMenuUI.activeSelf)
        {
            voiceRecordingsMenuUI.SetActive(false);
            GameManager.instance.ResumeGame();
        }
    }

    public void OpenJournalMenu()
    {
        if (!journalMenuUI.activeSelf)
        {
            journalMenuUI.SetActive(true);
            voiceRecordingsMenuUI.SetActive(false);
            paperMenuUI.SetActive(false);

            GameManager.instance.PauseGame();
        }
        else CloseJournalMenu();
    }

    public void CloseJournalMenu()
    {
        if (journalMenuUI.activeSelf)
        {
            journalMenuUI.SetActive(false);
            GameManager.instance.ResumeGame();
        }
    }
    public void OpenPaperMenu()
    {
        if (!paperMenuUI.activeSelf)
        {
            paperMenuUI.SetActive(true);
            journalMenuUI.SetActive(false);
            voiceRecordingsMenuUI.SetActive(false);

            GameManager.instance.PauseGame();
        }
        else ClosePaperMenu();
    }

    public void ClosePaperMenu()
    {
        if (paperMenuUI.activeSelf)
        {
        paperMenuUI.SetActive(false);
        GameManager.instance.ResumeGame();
        }
    }
}
