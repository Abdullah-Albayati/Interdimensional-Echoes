using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityPills : MonoBehaviour
{
    PickableObject currentItem;
    SanityController sanityScript;

    private void Awake()
    {
        currentItem = GetComponent<PickableObject>();
    }

    private void Update()
    {
        if(sanityScript == null)
        {
            
            sanityScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SanityController>();
           
        }
        if (sanityScript.currentSanity == 100)
            return;

        if (currentItem.isPickedUp)
        {
            if (Input.GetButtonDown(GameManager.instance.useItemButton))
            {
                sanityScript.TakingMeds(100);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPickAndPlace>().UseObject(currentItem);
            }
        }
    }
}
