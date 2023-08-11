using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class JournalEntry
{
    public string date;
    public string title;
    [TextArea(25, 200)]
    public string content;
}

public class JournalController : MonoBehaviour, IGameObserver
{
    public List<JournalEntry> entries = new List<JournalEntry>();

    public TextMeshProUGUI dateText1;
    public TextMeshProUGUI titleText1;
    public TextMeshProUGUI contentText1;
    public TextMeshProUGUI dateText2;
    public TextMeshProUGUI titleText2;
    public TextMeshProUGUI contentText2;
    public GameObject journalGo,journalCanvas;
    public bool isJournalPickedUp = false;
    private GameObject player;
    private int currentPageIndex;
    private float pageNavigation;
    private bool isPageNavigationInputActive = false;


    private void Start()
    {
        // Display the first page
        currentPageIndex = 0;
        DisplayJournalPage(currentPageIndex);

        journalGo.SetActive(false);
        GameManager.instance.AddObserver(this);
    }

    private void Update()
    {
        if (isJournalPickedUp == false)
            return;

        if (journalGo.activeInHierarchy)
        {
            // Check if the page navigation input is active
            if (!isPageNavigationInputActive)
            {
                // Get the input for page navigation
                float pageNavigation = Input.GetAxisRaw(GameManager.instance.browseJournalButton);

                // Navigate to the next or previous page based on input
                if (pageNavigation > 0)
                {
                    NextPage();
                    isPageNavigationInputActive = true;
                }
                else if (pageNavigation < 0)
                {
                    PreviousPage();
                    isPageNavigationInputActive = true;
                }
            }
            else
            {
                // Check if the page navigation input has been released
                float pageNavigation = Input.GetAxisRaw(GameManager.instance.browseJournalButton);
                if (Mathf.Approximately(pageNavigation, 0f))
                {
                    isPageNavigationInputActive = false;
                }
            }
        }

        if (Input.GetButtonDown(GameManager.instance.openJournalButton))
        {
            UIManager.Instance.OpenJournalMenu();
        }
    }
    private void DisplayJournalPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < entries.Count)
        {
            JournalEntry entry1 = entries[pageIndex];
            dateText1.text = entry1.date;
            titleText1.text = entry1.title;
            contentText1.text = entry1.content;

            if (pageIndex + 1 < entries.Count)
            {
                JournalEntry entry2 = entries[pageIndex + 1];
                dateText2.text = entry2.date;
                titleText2.text = entry2.title;
                contentText2.text = entry2.content;
            }
            else
            {
                // Clear the second page if there are no more entries
                dateText2.text = string.Empty;
                titleText2.text = string.Empty;
                contentText2.text = string.Empty;
            }
        }
    }

    public void NextPage()
    {
        currentPageIndex += 2;
        if (currentPageIndex >= entries.Count)
        {
            currentPageIndex = 0;
        }
        DisplayJournalPage(currentPageIndex);
    }

    public void PreviousPage()
    {
        currentPageIndex -= 2;
        if (currentPageIndex < 0)
        {
            currentPageIndex = Mathf.Max(entries.Count - 2, 0);
        }
        DisplayJournalPage(currentPageIndex);
    }
    public void OnGamePaused()
    {
        
    }
    public void OnGameResumed()
    {
        
    }
    private void OnDisable()
    {
        // Unsubscribe from GameManager's observer list
        GameManager.instance.RemoveObserver(this);
    }
}
