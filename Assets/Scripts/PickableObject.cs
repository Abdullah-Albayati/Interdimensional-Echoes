using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    
    public bool isPickedUp { get; private set; } = false;
    private Transform originalParent;
    [SerializeField] private ObjectType objectType;
    public bool canBeDropped = true;
    [SerializeField] private bool hintBar;
    [SerializeField] private string hintText = "";
    [SerializeField] private Image hintIcon;

    public ObjectType ObjectType
    {
        get { return objectType; }
        set { objectType = value; }
    }

    public void PickUp(Transform parent)
    {
        isPickedUp = true;
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

        if (hintBar == true)
        {
            UIManager.Instance.ShowHintBar(hintText, hintIcon);
        }
    }

    public void Place(Vector3 position, Vector3 normal)
    {
        isPickedUp = false;
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
        isPickedUp = true;
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

        if (hintBar == true)
        {
            UIManager.Instance.ShowHintBar(hintText, hintIcon);
        }
    }
    public void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
    }

#if UNITY_EDITOR
[CustomEditor(typeof(PickableObject))]
[CanEditMultipleObjects]
class PickableObjectEditor : Editor
{
    SerializedProperty hintBarProp;
    SerializedProperty hintTextProp;
    SerializedProperty hintIconProp;
    SerializedProperty canBeDroppedProp;  // Add serialized property for canBeDropped
    SerializedProperty objectTypeProp;
        SerializedProperty itemProp;


    void OnEnable()
    {
        hintBarProp = serializedObject.FindProperty("hintBar");
        hintTextProp = serializedObject.FindProperty("hintText");
        hintIconProp = serializedObject.FindProperty("hintIcon");
        itemProp = serializedObject.FindProperty("item");
        canBeDroppedProp = serializedObject.FindProperty("canBeDropped");  // Initialize canBeDropped property
        objectTypeProp = serializedObject.FindProperty("objectType");
            
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(hintBarProp, new GUIContent("Hintbar"));

        if (hintBarProp.boolValue)
        {
            EditorGUILayout.PropertyField(hintTextProp, new GUIContent("Hint Text"));
            EditorGUILayout.PropertyField(hintIconProp, new GUIContent("Hint Icon"));
        }

            // Add other properties you want to display
            EditorGUILayout.PropertyField(itemProp, new GUIContent("Item"));
            EditorGUILayout.PropertyField(objectTypeProp, new GUIContent("Object Type"));
            EditorGUILayout.PropertyField(canBeDroppedProp, new GUIContent("Can Be Dropped"));


        serializedObject.ApplyModifiedProperties();
    }
}
#endif

}
