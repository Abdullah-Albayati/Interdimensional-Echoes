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


    }

    private void LateUpdate()
    {
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
            if (!light.enabled) continue;  // Skip inactive lights

            Vector3 lightViewportPos = mainCamera.WorldToViewportPoint(light.transform.position);
            Vector3 playerToLight = light.transform.position - transform.position;


            Vector3 lightDirection = light.transform.forward;
            Vector3 lightToPlayer = transform.position - light.transform.position;

            float angleBetween = Vector3.Angle(lightDirection, lightToPlayer);

            if (angleBetween <= light.spotAngle * 0.5f) // Using half of spotAngle
            {
                inLightedArea = true;
                return true;
            }

            if (lightViewportPos.x >= 0 && lightViewportPos.x <= 1 && lightViewportPos.y >= 0 && lightViewportPos.y <= 1 && lightViewportPos.z > 0)
            {
                // Check for obstacles between the camera and light source
                Ray ray = new Ray(mainCamera.transform.position, playerToLight.normalized);

                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit) || hit.collider.gameObject == light.gameObject || hit.distance > playerToLight.magnitude)
                {
                    return true;  // A light source is within the screen space and not obstructed by obstacles
                }
            }

            inLightedArea = false;  // If we reached this point in the loop, we're not in a lighted area
        }

        return false;  // No light source is within the screen space
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
