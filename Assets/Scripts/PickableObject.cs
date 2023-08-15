using UnityEngine;

public enum ObjectType
{
    FlashLight,
    Pill,
    Batteries,
    Journal,
    Key,
    RecordingPlayer,
    Recording
}

public class PickableObject : MonoBehaviour
{
    public Item item;
    
    public bool IsPickedUp { get; private set; } = false;
    private Transform originalParent;
    [SerializeField] private ObjectType objectType;
    public bool canBeDropped = true;

    public ObjectType ObjectType
    {
        get { return objectType; }
        set { objectType = value; }
    }

    public void PickUp(Transform parent)
    {
        IsPickedUp = true;
        originalParent = transform.parent;
        transform.SetParent(parent);
        transform.position = parent.position;
        transform.rotation = parent.rotation;

        
            Quaternion initialRot = gameObject.transform.rotation;
            if (ObjectType == ObjectType.FlashLight)
            {
                gameObject.transform.rotation = initialRot * Quaternion.Euler(-90f, 0, 0);
            }
            else if (ObjectType == ObjectType.Batteries)
            {
                gameObject.transform.rotation = initialRot * Quaternion.Euler(0, 0, 0);
            }
            else if(ObjectType == ObjectType.Journal)
            {
            gameObject.transform.rotation = initialRot * Quaternion.Euler(0, 90f, 0);
            }
        // Disable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider collider = GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    public void Place(Vector3 position, Vector3 normal)
    {
        IsPickedUp = false;
        transform.SetParent(originalParent);
        transform.position = position;
        transform.rotation = Quaternion.identity;
        // Enable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider collider = GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    public void PickUpPermenantObject(Transform parent)
    {
        IsPickedUp = true;
        originalParent = transform.parent;
        transform.SetParent(parent);
        transform.position = parent.position;
        transform.rotation = parent.rotation;

        Quaternion initialRot = gameObject.transform.rotation;
        if (ObjectType == ObjectType.FlashLight)
        {
            gameObject.transform.rotation = initialRot * Quaternion.Euler(90f, 0, 0);
        }
        else if (ObjectType == ObjectType.Batteries)
        {
            gameObject.transform.rotation = initialRot * Quaternion.Euler(0, 0, 0);
            
        }
        else if (ObjectType == ObjectType.Journal)
        {
            gameObject.transform.rotation = initialRot * Quaternion.Euler(0, 90f, 0);
        }

        var rend = gameObject.GetComponent<Renderer>();
        rend.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        Collider collider = GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
    public void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
    }
}
