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
        isPickedUp = GetComponentInParent<PickableObject>().IsPickedUp;

        UpdateBatteryPercentage();

        if (isPickedUp)
        {
            if (Input.GetButtonDown(GameManager.instance.useItemButton))
            {
                if (!flashLight.isActiveAndEnabled && flashLightBattery > 0)
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
        {
            flashLight.enabled = false;
        }

        if (flashLight.enabled)
        {
            DecreaseBattery();
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
