using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerPickAndPlace : MonoBehaviour
{
    public Transform pickupPoint;
    public int maxHeldObjects = 3;

    private List<PickableObject> pickableObjects = new List<PickableObject>();
    private List<PickableObject> heldObjects = new List<PickableObject>();
    private int currentObjectIndex = 0;
    private bool isHandOnly = false;

    [SerializeField] private float pickupDistance = 2f; // The maximum distance to pick up objects
    [SerializeField] private LayerMask pickableLayerMask,surfaceLayerMask; // The layer mask for pickable objects

    public TextMeshProUGUI hoverText; // Reference to the TextMeshProUGUI component for hover text

    // Highlighting variables
    private Dictionary<Renderer, List<Color>> originalEmissionColors = new Dictionary<Renderer, List<Color>>();
    private Color highlightColor;

    private void Awake()
    {
        highlightColor = Color.blue;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * pickupDistance);
    }

    void Update()
    {
        CheckHandOnly();
        if (Input.GetButtonDown(GameManager.instance.pickUpButton))
        {

            // Perform raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, pickupDistance, pickableLayerMask);

            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    PickableObject pickableObject = hit.collider.GetComponent<PickableObject>();

                     if (heldObjects.Count >= maxHeldObjects && pickableObject.canBeDropped)
                        return;
                    if (pickableObject != null && !pickableObject.isPickedUp)
                    {
                        // Check if the object is already in the pickableObjects list
                        if (!pickableObjects.Contains(pickableObject))
                        {
                            pickableObjects.Add(pickableObject);
                        }
                        PickUpObject(pickableObject);
                        break;
                    }
                }
            }
        }
        else if (Input.GetButtonDown(GameManager.instance.dropButton) && heldObjects[currentObjectIndex].canBeDropped)
        {
            if (heldObjects.Count > 0)
            {
                if (heldObjects[currentObjectIndex].isActiveAndEnabled)
                {
                    PlaceObject(heldObjects[currentObjectIndex]);
                    if(heldObjects.Count > 0)
                    heldObjects[currentObjectIndex].gameObject.SetActive(true);
                }
                else
                    return;
            }
        }
        else if (Input.GetButtonDown(GameManager.instance.switchItemButton))
        {
            if (isHandOnly)
            {
                SwitchToNextObject();
            }
            else
            {
                SwitchObject();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToObjectByIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToObjectByIndex(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToObjectByIndex(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchToObjectByIndex(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchToObjectByIndex(4);
        }
        // Add more key mappings for additional objects as needed
    }

    void PickUpObject(PickableObject pickableObject)
    {
        if (pickableObject.canBeDropped)
        {
            pickableObjects.Remove(pickableObject);
            heldObjects.Add(pickableObject);
            pickableObject.PickUp(pickupPoint);
            InventoryManager.Instance.Add(pickableObject.item);

            if (heldObjects.Count == 1)
            {
                pickableObject.gameObject.SetActive(true); // Activate the main held object
            }
            else
            {
                pickableObject.gameObject.SetActive(false); // Deactivate other held objects
            }


        }
        else
            pickUpPermenantObject(pickableObject);

            InventoryManager.Instance.ListItems();
        Debug.Log("Player Picked Up a " + pickableObject.ObjectType);
        UnhighlightObject();
        UpdateHoverText();
    }

    void PlaceObject(PickableObject pickableObject)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, pickupDistance, surfaceLayerMask))
        {
            pickableObject.Place(hit.point, hit.normal);
            heldObjects.Remove(pickableObject);

            if (currentObjectIndex != 0)
                currentObjectIndex--;

            InventoryManager.Instance.Remove(pickableObject.item);
            pickableObjects.Add(pickableObject);
        }

        InventoryManager.Instance.ListItems();
        UpdateHoverText();
        CheckHandOnly();
    }

    public void UseObject(PickableObject pickableObject)
    {
        heldObjects.Remove(pickableObject);
        if (currentObjectIndex != 0)
            currentObjectIndex--;
        if (heldObjects.Count > 0)
            heldObjects[currentObjectIndex].gameObject.SetActive(true);
        InventoryManager.Instance.Remove(pickableObject.GetComponent<PickableObject>().item);
        InventoryManager.Instance.ListItems();
        Destroy(pickableObject.gameObject);
        CheckHandOnly();
        UpdateHoverText();
    }
    public void pickUpPermenantObject(PickableObject pickableObject)
    {
        pickableObject.PickUpPermenantObject(pickupPoint);

    }
    void SwitchToNextObject()
    {
        if (heldObjects.Count > 0)
        {
            PickableObject previousObject = heldObjects[currentObjectIndex];
            previousObject.gameObject.SetActive(false); // Deactivate the previous main object

            currentObjectIndex = (currentObjectIndex + 1) % heldObjects.Count;
            PickableObject nextObject = heldObjects[currentObjectIndex];
            nextObject.gameObject.SetActive(true); // Activate the new main object
        }

        UpdateHoverText();
    }

    void SwitchObject()
    {
        if (heldObjects.Count > 1)
        {
            PickableObject previousObject = heldObjects[currentObjectIndex];
            previousObject.gameObject.SetActive(false); // Deactivate the previous main object

            currentObjectIndex = (currentObjectIndex + 1) % heldObjects.Count;
            PickableObject nextObject = heldObjects[currentObjectIndex];
            nextObject.gameObject.SetActive(true); // Activate the new main object
        }

        UpdateHoverText();
    }

    void SwitchToObjectByIndex(int index)
    {
        if (index >= 0 && index < heldObjects.Count)
        {
            

            PickableObject previousObject = heldObjects[currentObjectIndex];
            previousObject.gameObject.SetActive(false); // Deactivate the previous main object

            currentObjectIndex = index;
            PickableObject nextObject = heldObjects[currentObjectIndex];
            nextObject.gameObject.SetActive(true); // Activate the new main object


            UpdateHoverText();
        }
    }

    void CheckHandOnly()
    {
        if (heldObjects.Count < maxHeldObjects)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                isHandOnly = true;
                foreach (PickableObject obj in heldObjects)
                {
                    obj.gameObject.SetActive(false); // Deactivate all held objects
                }
            }
        }
        else
        {
            isHandOnly = false;
        }
        foreach (var item in heldObjects)
        {
            if (item.isActiveAndEnabled)
            {
                isHandOnly = false;
            }
        }
    }

    void FixedUpdate()
    {
        // Perform raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, pickupDistance, pickableLayerMask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                PickableObject pickableObject = hit.collider.GetComponent<PickableObject>();
                if (pickableObject != null && !pickableObject.isPickedUp)
                {
                    HighlightObject(hit.collider.gameObject);
                    SetHoverText("Press E to Pick up: " + pickableObject.item.itemName);
                    return;
                }
            }
        }


        UnhighlightObject();
        ClearHoverText();
    }

    private void HighlightObject(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponents<Renderer>();
        if (renderers == null)
            renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (!originalEmissionColors.ContainsKey(renderer))
            {
                // Store the original emission colors for each material
                List<Color> colors = new List<Color>();
                foreach (Material material in renderer.materials)
                {
                    colors.Add(material.GetColor("_EmissionColor"));
                }
                originalEmissionColors.Add(renderer, colors);
            }

            // Set the new emission color to highlightColor for each material
            foreach (Material material in renderer.materials)
            {
                if (obj.GetComponent<PickableObject>().canBeDropped)
                    material.SetColor("_EmissionColor", highlightColor);
                else
                    material.SetColor("_EmissionColor", Color.gray);
            }
        }
    }

    private void UnhighlightObject()
    {
        foreach (KeyValuePair<Renderer, List<Color>> pair in originalEmissionColors)
        {
            Renderer renderer = pair.Key;
            List<Color> colors = pair.Value;

            // Restore the original emission colors for each material
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].SetColor("_EmissionColor", colors[i]);
            }
        }
        originalEmissionColors.Clear();
    }
    private void UpdateHoverText()
    {
        if (heldObjects.Count > 0)
        {
            string text = "Press G to drop";
            if (heldObjects.Count > 1 && !isHandOnly)
            {
                text += " / Press Tab to switch object (" + (currentObjectIndex + 1) + "/" + heldObjects.Count + ")";
            }
            SetHoverText(text);
        }
        else
        {
            ClearHoverText();
        }
    }

    private void SetHoverText(string text)
    {
        if (hoverText != null)
        {
            hoverText.text = text;
        }
    }

    private void ClearHoverText()
    {
        SetHoverText("");
    }
}
