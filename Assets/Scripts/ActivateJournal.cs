using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateJournal : MonoBehaviour
{
    PickableObject currentItem;
    private JournalController journalController;
    private void Start()
    {
        currentItem = GetComponent<PickableObject>();
    }
    private void Update()
    {
        if(journalController == null)
        {
            journalController = GameObject.Find("Journal Canvas").GetComponent<JournalController>();
            return;
        }
        
        
        if (currentItem.IsPickedUp && journalController.isJournalPickedUp == false)
        {
            journalController.isJournalPickedUp = true;
            Destroy(gameObject);
        }
    }
}
