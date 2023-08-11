using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FlashLight : MonoBehaviour
{
    public Light flashLight;
    public bool isPickedUp;
    public float flashLightBattery = 100;
    public int decreaseRate;
    public TextMeshProUGUI batteryTxt;


    private void Start()
    {
        flashLight.enabled = false;
        batteryTxt = GameObject.Find("Battery Percentage").GetComponent<TextMeshProUGUI>();
        
    }

    private void Update()
    {
        isPickedUp = GetComponentInParent<PickableObject>().IsPickedUp;

        UpdateBatteryPercentage();
        Debug.Log(flashLightBattery);
        if( isPickedUp)
        {
            if (Input.GetButtonDown(GameManager.instance.useItemButton))
            {
            if (flashLight.isActiveAndEnabled == false && flashLightBattery > 0)
            {
                flashLight.enabled = true;
                
            }
            else
            {
                flashLight.enabled = false;

            }
          }
        }
        else
        {
            flashLight.enabled = false;
        }

        if (flashLightBattery == 0)
            flashLight.enabled = false;

        if(flashLight.enabled == true)
        {
            DecreaseBattery();
        }
        else
        {
            return;
        }

    }

    public void DecreaseBattery()
    {
        flashLightBattery -= decreaseRate * Time.deltaTime;
        flashLightBattery = Mathf.Clamp(flashLightBattery, 0, 100);
        
    }
    public void IncreaseBattery(int increaseBatteryValue)
    {
        flashLightBattery += increaseBatteryValue;
        flashLightBattery = Mathf.Clamp(flashLightBattery, 0, 100);
      
    }

    public void UpdateBatteryPercentage()
    {
        if (isPickedUp)
            batteryTxt.text = "Battery Percentage: " + (int)flashLightBattery;
        else batteryTxt.text = string.Empty;
            
    }
}
