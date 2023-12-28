using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashLight : MonoBehaviour
{
    public Light flashLight;
    public bool isPickedUp;
    public float flashLightBattery = 100;
    public int decreaseRate;
    public TextMeshProUGUI batteryTxt;

    private Transform playerTransform;
    private Transform cameraTransform;

    private void Start()
    {
        flashLight.enabled = false;
        batteryTxt = GameObject.Find("Battery Percentage").GetComponent<TextMeshProUGUI>();

        // Cache the references for performance
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        
        isPickedUp = GetComponentInParent<PickableObject>().isPickedUp;

        
        if (flashLight.enabled)
        {
            UpdateBatteryPercentage();
            DecreaseBattery();
        }

       
        if (isPickedUp && Input.GetButtonDown(GameManager.instance.flashLightButton))
        {
            flashLight.enabled = flashLightBattery > 0 && !flashLight.enabled;
        }

        
        if (flashLightBattery < 100)
        {
            
            int rand = Random.Range(0, 10); 
            int intensity = Mathf.Clamp((int)flashLightBattery / 2 + rand, 0, 15); 

            if(flashLightBattery > 0)
            flashLight.intensity = intensity;
            else flashLight.intensity = 0;
            
            
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
        {
            batteryTxt.text = "Battery Percentage: " + (int)flashLightBattery;
        }
        else
        {
            batteryTxt.text = string.Empty;
        }
    }
}
