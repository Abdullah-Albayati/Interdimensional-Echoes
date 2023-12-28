using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batteries : MonoBehaviour
{
    public PickableObject currentItem;

    FlashLight flashLight;

    private void Awake()
    {
        currentItem = GetComponent<PickableObject>();
    }

    private void Update()
    {
        if (flashLight == null )
        {
            flashLight = transform.parent.gameObject.GetComponentInChildren<FlashLight>();
            return;
        }
        if (flashLight.flashLightBattery == 100)
            return;

        if (flashLight.isPickedUp && currentItem.isPickedUp)
        {
            if(gameObject.activeInHierarchy)
            {
                if (Input.GetButtonDown(GameManager.instance.useItemButton))
                {
                    flashLight.IncreaseBattery(100);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPickAndPlace>().UseObject(currentItem);
                }
            }
            
        }
            
        

        flashLight.UpdateBatteryPercentage();
    }
}
