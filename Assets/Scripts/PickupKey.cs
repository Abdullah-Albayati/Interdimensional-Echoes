using UnityEngine;

namespace EZDoor
{
    public class PickupKey : MonoBehaviour
    {
        public Key key;
        public string containerTag;
        private KeyContainer keyContainer;
        private PickableObject currentItem;

        private void Awake()
        {
            keyContainer = GameObject.FindWithTag(containerTag).GetComponent<KeyContainer>();
            currentItem = GetComponent<PickableObject>();
        }

        private void Update()
        {
            if (currentItem.IsPickedUp)
            {
                Pickup();
            }
        }
        public void Pickup()
        {
            Debug.Log("picked up key");
            keyContainer.keys.Add(key);
            Destroy(gameObject);
        }

        public void Interact()
        {
            Pickup();
        }
    }
}
