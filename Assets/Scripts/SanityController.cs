using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SanityController : MonoBehaviour
{
    public float maxSanity = 100;
    public float currentSanity = 100;

    public int increaseRate = 1;
    public int decreaseRate = 1;
    public int increaseMultiPlier = 1;
    public int decreaseMultiplier = 1;

    public CanvasGroup sanityCanvasGroup;

   [SerializeField] private bool inLightedArea = false;


    public Image sanityMeter;

    public string monsterObjectTag = "Monster";

    private void Start()
    {
        sanityCanvasGroup.alpha = sanityCanvasGroup.alpha = 0;
    }
    private void Update()
    {

        if (currentSanity == maxSanity)
        {
            sanityCanvasGroup.alpha = Mathf.Lerp(sanityCanvasGroup.alpha, 0, Time.deltaTime * 5);
        }
        else
            sanityCanvasGroup.alpha = Mathf.Lerp(sanityCanvasGroup.alpha, 1, Time.deltaTime * 5);
        UpdateSanity();


    }

    private void UpdateSanity()
    {
        Debug.Log("Player is seeing Light : " + IsLightWithinScreenSpace());
        Debug.Log("Player is seeing a Monster : " + IsUnwantedObjectWithinScreenSpace());


        
        if (!IsLightWithinScreenSpace())
        {
            DecreaseSanity(decreaseRate);
        }
        if (IsUnwantedObjectWithinScreenSpace())
        {
            DecreaseSanity(decreaseRate * decreaseMultiplier);
        }
        else if (IsUnwantedObjectWithinScreenSpace() == false && IsLightWithinScreenSpace() == true)
            IncreaseSanity(increaseRate * increaseMultiPlier);
        increaseMultiPlier = inLightedArea ? 3 : 1;
        UpdateSanityMeter();
    }

    public void DecreaseSanity(int decreaseRate)
    {
        currentSanity -= Time.deltaTime * decreaseRate;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }

    public void IncreaseSanity(int increaseRate)
    {
        currentSanity += Time.deltaTime * increaseRate;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }

    private bool IsUnwantedObjectWithinScreenSpace()
    {
        GameObject[] unwantedObjects = GameObject.FindGameObjectsWithTag(monsterObjectTag);
        Camera mainCamera = Camera.main;

        foreach (GameObject unwantedObject in unwantedObjects)
        {
            Vector3 objectViewportPos = mainCamera.WorldToViewportPoint(unwantedObject.transform.position);

            if (objectViewportPos.x >= 0 && objectViewportPos.x <= 1 && objectViewportPos.y >= 0 && objectViewportPos.y <= 1 && objectViewportPos.z > 0)
            {
                if (!IsObjectObstructed(mainCamera.transform.position, unwantedObject.transform.position))
                {
                    return true; // The unwanted object is within the screen space and not obstructed
                }
            }
        }

        return false; // No unwanted object is within the screen space or not obstructed
    }


    private bool IsLightWithinScreenSpace()
    {
        Light[] lights = FindObjectsOfType<Light>();

        Camera mainCamera = Camera.main;
        foreach (Light light in lights)
        {
            if (!light.enabled) return false; // Skip inactive lights

            Vector3 lightViewportPos = mainCamera.WorldToViewportPoint(light.transform.position);

            Vector3 playerToLight = light.transform.position - transform.position;
            float angle = Vector3.Angle(playerToLight, transform.position);

            if (lightViewportPos.x >= 0 && lightViewportPos.x <= 1 && lightViewportPos.y >= 0 && lightViewportPos.y <= 1 && lightViewportPos.z > 0)
            {
                // Check for obstacles between the camera and light source
                Ray ray = new Ray(mainCamera.transform.position, light.transform.position - mainCamera.transform.position);

                RaycastHit hit;
                if (Physics.Linecast(Camera.main.transform.position, light.transform.position, out hit) && hit.collider.gameObject != light.gameObject)
                {
                    continue; // Skip if there is an obstacle between the camera and light source
                }

                return true; // A light source is within the screen space and not obstructed by obstacles
            }

            if (angle <= light.spotAngle)
            {
                inLightedArea = true;
                return true; // Player is within the spot angle of the light
            }
            else
            {
                inLightedArea = false;
            }

        }

        return false; // No light source is within the screen space
    }

    private bool IsObjectObstructed(Vector3 fromPosition, Vector3 toPosition)
    {
        RaycastHit hit;
        if (Physics.Linecast(fromPosition, toPosition, out hit))
        {
            if (!hit.collider.CompareTag(monsterObjectTag))
            {
                return true; // An obstacle is hit before reaching the unwanted object
            }
        }

        return false; // No obstacles are hit, the path to the unwanted object is clear
    }
    
    public void TakingMeds(int increaseSanity)
    {
        currentSanity += increaseSanity;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }
    private void UpdateSanityMeter()
    {
        if (sanityMeter != null)
        {
            float fillAmount = currentSanity / maxSanity;
            sanityMeter.rectTransform.localScale = new Vector3(fillAmount,sanityMeter.rectTransform.localScale.y,sanityMeter.rectTransform.localScale.z);
        }

    }
}
