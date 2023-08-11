using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateJournal : MonoBehaviour
{
    PickableObject currentItem;

    private void Start()
    {
        currentItem = GetComponent<PickableObject>();
    }
    private void Update()
    {
        var journalCanvas = GameObject.Find("Journal Canvas").GetComponent<JournalController>();
        
        if (currentItem.IsPickedUp && journalCanvas.isJournalPickedUp == false)
        {
            journalCanvas.isJournalPickedUp = true;
            Destroy(gameObject);
        }
    }
}
